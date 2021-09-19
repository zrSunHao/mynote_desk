using System;
using System.Collections.Generic;

namespace BlizzardWind.App.Common.MarkText
{
    public class MarkRow
    {
        public string Value { get; set; }

        public MarkNoteElementType Type { get; set; }
    }

    public class MarkElement
    {
        public MarkNoteElementType Type { get; set; }

        public string Content { get; set; }

        public List<string> List { get; set; }

        public MarkKeyValue KeyValue { get; set; }



        public int RowType => (int)Type;

        public int Level => GetLevel();

        public string TypeName => GetTypeName();

        public string ShortContent => GetContent();

        public string FilePath => _FilePath;

        private string _FilePath = string.Empty;

        public void SetFilePath(string path) => _FilePath = path;

        public Guid FileKey => _FileKey;

        private Guid _FileKey = Guid.Empty;

        public void SetFileKey(Guid key) => _FileKey = key;


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

        private int GetLevel()
        {
            return Type switch
            {
                MarkNoteElementType.h1 => MarkTypeLevel.Skip,
                MarkNoteElementType.h2 => MarkTypeLevel.Title_1,
                MarkNoteElementType.key => MarkTypeLevel.Single,
                MarkNoteElementType.profile => MarkTypeLevel.Single,
                MarkNoteElementType.summary => MarkTypeLevel.Single,
                MarkNoteElementType.quote => MarkTypeLevel.Single,
                MarkNoteElementType.h3 => MarkTypeLevel.Title_2,
                _ => MarkTypeLevel.Leaf,
            };
        }

        private string GetContent()
        {
            switch (Type)
            {
                case MarkNoteElementType.h1:
                    return SubstringContent();
                case MarkNoteElementType.h2:
                    return SubstringContent();
                case MarkNoteElementType.h3:
                    return SubstringContent();
                case MarkNoteElementType.key:
                    return SubstringContent();
                case MarkNoteElementType.profile:
                    return SubstringContent();
                case MarkNoteElementType.img:
                    return GetValue();
                case MarkNoteElementType.txt:
                    return GetValue();
                case MarkNoteElementType.link:
                    return GetValue();
                case MarkNoteElementType.list:
                    return "";
                case MarkNoteElementType.summary:
                    return SubstringContent();
                case MarkNoteElementType.quote:
                    return "引用";
                case MarkNoteElementType.p:
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
