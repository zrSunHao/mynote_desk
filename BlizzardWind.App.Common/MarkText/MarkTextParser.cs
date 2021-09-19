using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BlizzardWind.App.Common.MarkText
{
    /// <summary>
    /// 标记文本解析器
    /// </summary>
    public class MarkTextParser
    {
        //判断是否为【块起始行】的正则 即：#*]
        private readonly Regex _senseRg = new(@"\#.*?\]");
        //提取【块起始行】标识的正则
        private readonly Regex _tagRg = new(@"(?<=\#).*?(?=\])");
        //提取【块】名称的正则
        private readonly Regex _nameRg = new(@"(?<=(\<)).*?(?=(\>))", RegexOptions.Multiline | RegexOptions.Singleline);
        //提取【单行块】值的正则
        private readonly Regex _valueRg = new(@"(?<=(\()).*?(?=(\)))", RegexOptions.Multiline | RegexOptions.Singleline);

        /// <summary>
        /// 获取标记文本显示元素
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<MarkElement> GetMarkElements(string text)
        {
            // 1、通过换行符切割标记文本,获得行文本
            List<string> contents = text.Split("\n").ToList();
            // 2、行文本解析、转换为行元素
            List<MarkRow> rows = ConvertRows(contents);
            // 3、将行元素解析、合并为显示元素
            List<MarkElement> elements = ConvertElements(rows);
            return elements;
        }



        /// <summary>
        /// 行文本解析、转换为行元素
        /// </summary>
        /// <param name="contents"></param>
        /// <returns></returns>
        private List<MarkRow> ConvertRows(List<string> contents)
        {
            List<MarkRow> rows = new();
            foreach (string c in contents)
            {
                string value = c.Trim();
                if (string.IsNullOrEmpty(value)) continue; // 如果行为空，则跳过

                if (value == "--") // 判断是否为文本【块结束行】
                {
                    rows.Add(new MarkRow { Value = value, Type = MarkNoteElementType.end });
                    continue;
                }

                if (_senseRg.IsMatch(value)) // 判断是否为文本【块起始行】
                {
                    MarkRow? row = RowTextParse(value);
                    if (row != null) rows.Add(row);
                }
                else
                    rows.Add(new MarkRow { Value = value, Type = MarkNoteElementType.other });
            }
            return rows;
        }

        /// <summary>
        /// 【块起始行】解析
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private MarkRow? RowTextParse(string text)
        {
            MatchCollection? rs = _tagRg.Matches(text);
            if (!rs.Any()) // 判断是否提取出【块起始行】标识
                return new MarkRow { Value = text, Type = MarkNoteElementType.other };
            string tag = rs[0].ToString();

            MarkNoteElementType type = tag switch
            {
                MarkNoteElementTypeConsts.H1 => MarkNoteElementType.h1,
                MarkNoteElementTypeConsts.H2 => MarkNoteElementType.h2,
                MarkNoteElementTypeConsts.H3 => MarkNoteElementType.h3,
                MarkNoteElementTypeConsts.KEY => MarkNoteElementType.key,
                MarkNoteElementTypeConsts.PROFILE => MarkNoteElementType.profile,
                MarkNoteElementTypeConsts.P => MarkNoteElementType.p,
                MarkNoteElementTypeConsts.IMG => MarkNoteElementType.img,
                MarkNoteElementTypeConsts.TXT => MarkNoteElementType.txt,
                MarkNoteElementTypeConsts.LINK => MarkNoteElementType.link,
                MarkNoteElementTypeConsts.LIST => MarkNoteElementType.list,
                MarkNoteElementTypeConsts.SUMMARY => MarkNoteElementType.summary,
                MarkNoteElementTypeConsts.QUOTE => MarkNoteElementType.quote,
                MarkNoteElementTypeConsts.END => MarkNoteElementType.end,
                _ => MarkNoteElementType.other,
            };

            // list【块起始行】只有标识，没有其他信息
            if (type == MarkNoteElementType.list || type == MarkNoteElementType.profile 
                || type == MarkNoteElementType.summary || type == MarkNoteElementType.quote) 
                return new MarkRow() { Type = type };
            // 移除标识
            string value = text.Replace(@$"#{tag}]", "").Trim();
            if (string.IsNullOrEmpty(value)) return null;//没有其他信息的行不规范
            return new MarkRow() { Type = type, Value = value };
        }

        /// <summary>
        /// 行元素解析
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        private List<MarkElement> ConvertElements(List<MarkRow> rows)
        {
            List<MarkElement> elements = new();
            MarkElement? blockElement = null; // 当前块元素
            List<string> rowsBuffer = new();  // 块的行缓冲器
            foreach (MarkRow row in rows)
            {
                // 不是【块起始行】，当作【多行块】的内容，缓存
                if (row.Type == MarkNoteElementType.other)
                {
                    rowsBuffer.Add(row.Value);
                    continue;
                }

                // 是【多行块】的【块结束行】,根据块类型组装块元素
                if (row.Type == MarkNoteElementType.end && blockElement != null)
                {
                    MarkElement? ele = null;
                    switch (blockElement.Type)
                    {
                        case MarkNoteElementType.profile:
                        case MarkNoteElementType.summary:
                            var builder = new StringBuilder();
                            foreach (var item in rowsBuffer)
                                builder.Append($"\u3000\u3000{item}\u000A");
                            blockElement.Content = builder.ToString();
                            ele = blockElement;
                            break;
                        case MarkNoteElementType.list:
                            ele = GetListElement(blockElement, rowsBuffer);
                            break;
                        case MarkNoteElementType.quote:
                            ele = GetListElement(blockElement, rowsBuffer);
                            break;
                        default:
                            break;
                    }
                    if (ele != null) elements.Add(ele);
                    // 清理当前块元素信息
                    blockElement = null;
                    rowsBuffer.Clear();
                    continue;
                }

                // 清理块的行缓冲器，内容当作段落处理
                if (rowsBuffer.Any() && blockElement == null)
                {
                    foreach (var li in rowsBuffer)
                        elements.Add(new MarkElement { Type = MarkNoteElementType.p, Content = $"\u3000\u3000{li}" });
                }
                rowsBuffer.Clear();

                // 【块起始行】处理
                MarkElement element = new MarkElement { Type = row.Type };
                switch (row.Type)
                {
                    case MarkNoteElementType.h1:
                    case MarkNoteElementType.h2:
                    case MarkNoteElementType.h3:
                        element.Content = row.Value;
                        break;
                    case MarkNoteElementType.p:
                        element.Content = $"\u3000\u3000{row.Value}";
                        break;
                    case MarkNoteElementType.key:
                        string value = row.Value.Replace("，", ",");
                        element.List = value.Split(",").ToList();
                        element.Content = row.Value;
                        break;
                    case MarkNoteElementType.img:
                    case MarkNoteElementType.txt:
                    case MarkNoteElementType.link:
                        element.KeyValue = new MarkKeyValue
                        {
                            Name = _nameRg.Match(row.Value).Value,
                            Value = _valueRg.Match(row.Value).Value
                        };
                        break;
                    case MarkNoteElementType.profile:
                    case MarkNoteElementType.summary:
                    case MarkNoteElementType.list:
                    case MarkNoteElementType.quote:
                        blockElement = element;
                        continue;
                    default:
                        break;
                }

                elements.Add(element);
            }
            return elements;
        }



        /// <summary>
        /// 获取list元素
        /// </summary>
        /// <param name="blockElement"></param>
        /// <param name="rowsBuffer"></param>
        /// <returns></returns>
        private MarkElement? GetListElement(MarkElement blockElement, List<string> rowsBuffer)
        {
            if (rowsBuffer == null || !rowsBuffer.Any())
                return null;
            blockElement.List =new List<string>();
            blockElement.List.AddRange(rowsBuffer);
            return blockElement;
        }
    }
}
