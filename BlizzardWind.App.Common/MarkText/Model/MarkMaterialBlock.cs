using System.Collections.Generic;

namespace BlizzardWind.App.Common.MarkText
{
    /// <summary>
    /// 带解析的文本块
    /// </summary>
    public class MarkMaterialBlock
    {
        /// <summary>
        /// 行文本集合
        /// </summary>
        public List<string> Values { get; set; }

        /// <summary>
        /// 块元素类别
        /// </summary>
        public MarkNoteElementType Type { get; set; }

        /// <summary>
        /// 是完整的块信息
        /// </summary>
        public bool IsComplete { get; set; }

        public void AddValue(string value)
        {
            if(Values == null)
                Values = new List<string>();
            Values.Add(value);
        }
    }
}
