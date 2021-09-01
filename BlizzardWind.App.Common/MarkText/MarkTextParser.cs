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
            List<string> contents = text.Split("\r\n").ToList();
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
                    rows.Add(new MarkRow { Value = value, Type = MarkType.end });
                    continue;
                }

                if (_senseRg.IsMatch(value)) // 判断是否为文本【块起始行】
                {
                    MarkRow? row = RowTextParse(value);
                    if (row != null) rows.Add(row);
                }
                else
                    rows.Add(new MarkRow { Value = value, Type = MarkType.other });
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
                return new MarkRow { Value = text, Type = MarkType.other };
            string tag = rs[0].ToString();

            MarkType type = tag switch
            {
                MarkTypeConsts.H1 => MarkType.h1,
                MarkTypeConsts.H2 => MarkType.h2,
                MarkTypeConsts.H3 => MarkType.h3,
                MarkTypeConsts.KEY => MarkType.key,
                MarkTypeConsts.PROFILE => MarkType.profile,
                MarkTypeConsts.P => MarkType.p,
                MarkTypeConsts.IMG => MarkType.img,
                MarkTypeConsts.LINK => MarkType.link,
                MarkTypeConsts.LIST => MarkType.list,
                MarkTypeConsts.TABLE => MarkType.table,
                MarkTypeConsts.SUMMARY => MarkType.summary,
                MarkTypeConsts.QUOTE => MarkType.quote,
                MarkTypeConsts.END => MarkType.end,
                _ => MarkType.other,
            };

            // list【块起始行】只有标识，没有其他信息
            if (type == MarkType.list) return new MarkRow() { Type = type };
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
                if (row.Type == MarkType.other)
                {
                    rowsBuffer.Add(row.Value);
                    continue;
                }

                // 是【多行块】的【块结束行】,根据块类型组装块元素
                if (row.Type == MarkType.end && blockElement != null)
                {
                    MarkElement? ele = null;
                    switch (blockElement.Type)
                    {
                        case MarkType.list:
                            ele = GetListElement(blockElement, rowsBuffer);
                            break;
                        case MarkType.quote:
                            ele = GetQuoteElement(blockElement, rowsBuffer);
                            break;
                        case MarkType.table:
                            ele = GetTableElement(blockElement, rowsBuffer);
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
                        elements.Add(new MarkElement { Type = MarkType.p, Content = li });
                }
                rowsBuffer.Clear();

                // 【块起始行】处理
                MarkElement element = new MarkElement { Type = row.Type };
                switch (row.Type)
                {
                    case MarkType.h1:
                    case MarkType.h2:
                    case MarkType.h3:
                    case MarkType.p:
                    case MarkType.profile:
                    case MarkType.summary:
                        element.Content = row.Value;
                        break;
                    case MarkType.key:
                        string value = row.Value.Replace("，", ",");
                        element.List = value.Split(",").ToList();
                        element.Content = row.Value;
                        break;
                    case MarkType.img:
                    case MarkType.link:
                        element.KeyValue = new MarkKeyValue
                        {
                            Name = _nameRg.Match(row.Value).Value,
                            Value = _valueRg.Match(row.Value).Value
                        };
                        break;
                    case MarkType.list:
                    case MarkType.quote:
                        blockElement = element;
                        continue;
                    case MarkType.table:
                        element.Table = GetMarkTable(row.Value);
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
            blockElement.List = rowsBuffer;
            return blockElement;
        }

        /// <summary>
        /// 获取引用元素
        /// </summary>
        /// <param name="blockElement"></param>
        /// <param name="rowsBuffer"></param>
        /// <returns></returns>
        private MarkElement? GetQuoteElement(MarkElement blockElement, List<string> rowsBuffer)
        {
            if (rowsBuffer == null || !rowsBuffer.Any())
                return null;
            List<MarkKeyValue> pieces = new List<MarkKeyValue>();
            foreach (string text in rowsBuffer)
            {
                var piece = new MarkKeyValue
                {
                    Name = _nameRg.Match(text).Value,
                    Value = _valueRg.Match(text).Value
                };
                pieces.Add(piece);
            }
            blockElement.Pieces = pieces;
            return blockElement;
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private MarkTable GetMarkTable(string text)
        {
            string v = text.Replace("<", "");
            v = v.Replace(">", "");
            List<string> ts = v.Split("|").ToList();
            return new MarkTable { Ts = ts };
        }

        /// <summary>
        /// 获取表元素
        /// </summary>
        /// <param name="blockElement"></param>
        /// <param name="rowsBuffer"></param>
        /// <returns></returns>
        private MarkElement? GetTableElement(MarkElement blockElement, List<string> rowsBuffer)
        {
            int? count = blockElement.Table?.Ts != null ? blockElement.Table?.Ts.Count : 0;
            if (rowsBuffer == null || !rowsBuffer.Any() || blockElement.Table == null
                || !count.HasValue || count < 1)
                return null;

            var vs = new List<List<string>>();
            foreach (var text in rowsBuffer)
            {
                List<string> cs = text.Split("|").ToList();
                vs.Add(cs);
            }
            blockElement.Table.Vs = vs;
            return blockElement;
        }
    }
}
