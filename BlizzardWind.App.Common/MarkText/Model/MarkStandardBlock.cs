using System;
using System.Collections.Generic;

namespace BlizzardWind.App.Common.MarkText
{
    /// <summary>
    /// 格式化之后的文本块
    /// </summary>
    public partial class MarkStandardBlock
    {
        public MarkNoteElementType Type { get; set; }

        /// <summary>
        /// 文本信息
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 集合
        /// </summary>
        public List<string> List { get; set; }

        /// <summary>
        /// 键值对
        /// </summary>
        public MarkKeyValue Map { get; set; }

        /// <summary>
        /// 装饰符
        /// </summary>
        public string Ornaments { get; set; }


        public int Level => GetLevel();

        public string TypeName => GetTypeName();

        public string BriefText => GetBriefText();

        public string FilePath => _FilePath;

        public Guid FileKey => _FileKey;
    }

    public partial class MarkStandardBlock
    {
        private Guid _FileKey = Guid.Empty;
        public void SetFileKey(Guid key) => _FileKey = key;

        private string _FilePath = string.Empty;
        public void SetFilePath(string path) => _FilePath = path;

        private int GetLevel()
        {
            return Type switch
            {
                MarkNoteElementType.h1 => MarkElementLevel.Skip,
                MarkNoteElementType.h2 => MarkElementLevel.H2,
                MarkNoteElementType.key => MarkElementLevel.Single,
                MarkNoteElementType.profile => MarkElementLevel.Single,
                MarkNoteElementType.summary => MarkElementLevel.Single,
                MarkNoteElementType.quote => MarkElementLevel.Single,
                MarkNoteElementType.h3 => MarkElementLevel.H3,
                _ => MarkElementLevel.Leaf,
            };
        }

        private string GetTypeName()
        {
            switch (Type)
            {
                case MarkNoteElementType.h1:
                    return "H1";
                case MarkNoteElementType.h2:
                    return "H2";
                case MarkNoteElementType.h3:
                    return "H3";
                case MarkNoteElementType.key:
                    return "Keys";
                case MarkNoteElementType.profile:
                    return "Profile";
                case MarkNoteElementType.img:
                    return "Img";
                case MarkNoteElementType.txt:
                    return "Txt";
                case MarkNoteElementType.link:
                    return "Link";
                case MarkNoteElementType.list:
                    return "List";
                case MarkNoteElementType.summary:
                    return "Summary";
                case MarkNoteElementType.quote:
                    return "Quote";
                case MarkNoteElementType.p:
                    return "P";
            }
            return "";
        }

        private string GetBriefText()
        {
            switch (Type)
            {
                case MarkNoteElementType.h1:
                    return SubstringText();
                case MarkNoteElementType.h2:
                    return SubstringText();
                case MarkNoteElementType.h3:
                    return SubstringText();
                case MarkNoteElementType.key:
                    return SubstringText();
                case MarkNoteElementType.profile:
                    return SubstringText();
                case MarkNoteElementType.img:
                    return GetValue();
                case MarkNoteElementType.txt:
                    return GetValue();
                case MarkNoteElementType.link:
                    return GetValue();
                case MarkNoteElementType.list:
                    return "";
                case MarkNoteElementType.summary:
                    return SubstringText();
                case MarkNoteElementType.quote:
                    return "引用";
                case MarkNoteElementType.p:
                    return SubstringText();
            }
            return "";
        }

        private string SubstringText()
        {
            if (string.IsNullOrEmpty(Text))
                return "";
            var msg = Text.Trim();
            if (msg.Length <= 12)
                return msg;
            return msg.Substring(0, 12) + "...";
        }

        private string GetValue()
        {
            if (Map == null)
                return "";
            return Map.Name;
        }
    }
}
