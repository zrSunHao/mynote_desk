namespace BlizzardWind.App.Common.MarkText
{
    public class MarkRow
    {
        public string Value { get; set; }

        public MarkType Type { get; set; }
    }

    public class MarkElement
    {
        public MarkType Type { get; set; }

        public string Content { get; set; }

        public List<string> List { get; set; }

        public MarkKeyValue KeyValue { get; set; }

        public int RowType => (int)Type;

        public int Level => GetLevel();

        public string TypeName => GetTypeName();

        public string ShortContent => GetContent();

        public string FilePath => _FilePath;

        private string _FilePath = string.Empty;

        public void SetIFilePath(string path) => _FilePath = path;


        private string GetTypeName()
        {
            switch (Type)
            {
                case MarkType.h1:
                    return "H1";
                case MarkType.h2:
                    return "H2";
                case MarkType.h3:
                    return "H3";
                case MarkType.key:
                    return "Keys";
                case MarkType.profile:
                    return "Profile";
                case MarkType.img:
                    return "Img";
                case MarkType.txt:
                    return "Txt";
                case MarkType.link:
                    return "Link";
                case MarkType.list:
                    return "List";
                case MarkType.summary:
                    return "Summary";
                case MarkType.quote:
                    return "Quote";
                case MarkType.p:
                    return "P";
            }
            return "";
        }

        private int GetLevel()
        {
            return Type switch
            {
                MarkType.h1 => MarkTypeLevel.Skip,
                MarkType.h2 => MarkTypeLevel.Title_1,
                MarkType.key => MarkTypeLevel.Single,
                MarkType.profile => MarkTypeLevel.Single,
                MarkType.summary => MarkTypeLevel.Single,
                MarkType.quote => MarkTypeLevel.Single,
                MarkType.h3 => MarkTypeLevel.Title_2,
                _ => MarkTypeLevel.Leaf,
            };
        }

        private string GetContent()
        {
            switch (Type)
            {
                case MarkType.h1:
                    return SubstringContent();
                case MarkType.h2:
                    return SubstringContent();
                case MarkType.h3:
                    return SubstringContent();
                case MarkType.key:
                    return SubstringContent();
                case MarkType.profile:
                    return SubstringContent();
                case MarkType.img:
                    return GetValue();
                case MarkType.txt:
                    return GetValue();
                case MarkType.link:
                    return GetValue();
                case MarkType.list:
                    return "";
                case MarkType.summary:
                    return SubstringContent();
                case MarkType.quote:
                    return "引用";
                case MarkType.p:
                    return SubstringContent();
            }
            return "";
        }

        private string SubstringContent()
        {
            if (string.IsNullOrEmpty(Content))
                return "";
            var msg = Content.Trim();
            if (msg.Length <= 12)
                return msg;
            return msg.Substring(0,12) + "...";
        }

        private string GetValue()
        {
            if (KeyValue == null)
                return "";
            return KeyValue.Name;
        }
    }

    public class MarkTypeLevel
    {
        public const int Skip = 1024;


        public const int Title_1 = 512;

        public const int Single = 511;


        public const int Title_2 = 256;


        public const int Leaf = 0;
    }

    public class MarkKeyValue
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
