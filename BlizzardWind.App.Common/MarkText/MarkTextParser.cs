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


        #region 重写文本解析 -- 未完成

        /// <summary>
        /// 1、切割（根据换行符）
        /// </summary>
        /// <param name="text">带解析的文本</param>
        /// <returns></returns>
        public List<string> TextSplit(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<string>();
            List<string> strs = text.Split("\n").ToList();

            List<string> rows = new List<string>();
            foreach (var str in strs)
            {
                string row = str.Trim();
                if(!string.IsNullOrEmpty(row))
                    rows.Add(row);
            }
            return rows;
        }

        /// <summary>
        /// 2、识别（文本块）
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public List<MarkMaterialBlock> BlockDistinguish(List<string> rows)
        {
            List<MarkMaterialBlock> blocks = new();
            if(rows == null || !rows.Any())
                return blocks;

            MarkNoteElementType? type = null;
            List<string> rowBuffer = new List<string>();
            foreach (var row in rows)
            {
                if(row == MarkNoteSymbolConsts.END) //为【块结束行】
                {
                    if (type != null && rowBuffer != null && rowBuffer.Any())
                        blocks.Add(new MarkMaterialBlock { Type = type.Value, Values = rowBuffer });
                    rowBuffer = new List<string>();
                    type = null;
                    continue;
                }
                if (_senseRg.IsMatch(row)) //为【块起始行】
                {
                    MarkMaterialBlock block = BlockFirstRowParse(row);
                    if (block == null || block.Type == MarkNoteElementType.other)
                        continue;
                    else if (block.IsComplete) //是【单行块】
                        blocks.Add(block);
                    else //是【多行块】
                        type = block.Type;
                    continue;
                }
                rowBuffer.Add(row);
            }

            return blocks;
        }

        /// <summary>
        /// 3、格式化（文本块）
        /// </summary>
        /// <param name="materials"></param>
        /// <returns></returns>
        public List<MarkStandardBlock> BlockFormat(List<MarkMaterialBlock> materials)
        {
            List<MarkStandardBlock> blocks = new();
            if (materials == null || !materials.Any())
                return blocks;

            //TODO
            return blocks;
        }

        #endregion


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
                MarkNoteSymbolConsts.H1 => MarkNoteElementType.h1,
                MarkNoteSymbolConsts.H2 => MarkNoteElementType.h2,
                MarkNoteSymbolConsts.H3 => MarkNoteElementType.h3,
                MarkNoteSymbolConsts.KEY => MarkNoteElementType.key,
                MarkNoteSymbolConsts.PROFILE => MarkNoteElementType.profile,
                MarkNoteSymbolConsts.P => MarkNoteElementType.p,
                MarkNoteSymbolConsts.IMG => MarkNoteElementType.img,
                MarkNoteSymbolConsts.TXT => MarkNoteElementType.txt,
                MarkNoteSymbolConsts.LINK => MarkNoteElementType.link,
                MarkNoteSymbolConsts.LIST => MarkNoteElementType.list,
                MarkNoteSymbolConsts.SUMMARY => MarkNoteElementType.summary,
                MarkNoteSymbolConsts.QUOTE => MarkNoteElementType.quote,
                MarkNoteSymbolConsts.END => MarkNoteElementType.end,
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

        #region 重写文本解析私有方法

        /// <summary>
        /// 块起始行识别
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private MarkMaterialBlock BlockFirstRowParse(string row)
        {
            MatchCollection rs = _tagRg.Matches(row);
            string tag = rs[0].ToString();
            if (!rs.Any()) // 判断是否提取出【块起始行】标识
            {
                MarkMaterialBlock other = new() { Type = MarkNoteElementType.other };
                other.AddValue(row);
                return other;
            }

            MarkNoteElementType type = GetElementType(tag);
            var block = new MarkMaterialBlock { Type = type };
            bool isSingle = IsSingleRowElement(type);
            if (!isSingle) //多行块起始行没有其他信息
                return new MarkMaterialBlock { Type = type };

            // 移除标识
            string value = row.Replace(@$"#{tag}]", "").Trim();
            if (!string.IsNullOrEmpty(value))
            {
                block.AddValue(value);
                block.IsComplete = true;
            }
            return block;
        }

        /// <summary>
        /// 根据符号获取元素类型
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private static MarkNoteElementType GetElementType(string symbol)
        {
            MarkNoteElementType type = symbol switch
            {
                MarkNoteSymbolConsts.H1 => MarkNoteElementType.h1,
                MarkNoteSymbolConsts.H2 => MarkNoteElementType.h2,
                MarkNoteSymbolConsts.H3 => MarkNoteElementType.h3,
                MarkNoteSymbolConsts.KEY => MarkNoteElementType.key,
                MarkNoteSymbolConsts.PROFILE => MarkNoteElementType.profile,
                MarkNoteSymbolConsts.P => MarkNoteElementType.p,
                MarkNoteSymbolConsts.IMG => MarkNoteElementType.img,
                MarkNoteSymbolConsts.TXT => MarkNoteElementType.txt,
                MarkNoteSymbolConsts.LINK => MarkNoteElementType.link,
                MarkNoteSymbolConsts.LIST => MarkNoteElementType.list,
                MarkNoteSymbolConsts.SUMMARY => MarkNoteElementType.summary,
                MarkNoteSymbolConsts.QUOTE => MarkNoteElementType.quote,
                MarkNoteSymbolConsts.END => MarkNoteElementType.end,
                _ => MarkNoteElementType.other,
            };
            return type;
        }

        /// <summary>
        /// 判断是否为单行元素
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsSingleRowElement(MarkNoteElementType type)
        {
            bool isSingle;
            switch (type)
            {
                case MarkNoteElementType.profile:
                case MarkNoteElementType.list:
                case MarkNoteElementType.summary:
                case MarkNoteElementType.quote:
                case MarkNoteElementType.other:
                    isSingle = false;
                    break;
                default:
                    isSingle = true;
                    break;
            }
            return isSingle;
        }

        #endregion
    }
}
