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

        public List<MarkPiece> Pieces { get; set; }

        public MarkTable Table { get; set; }
    }

    public class MarkPiece
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class MarkTable
    {
        public List<string> Ts { get; set; }

        public List<List<string>> Vs { get; set; }
    }
}
