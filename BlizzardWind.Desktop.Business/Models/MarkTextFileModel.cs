namespace BlizzardWind.Desktop.Business.Models
{
    public class MarkTextFileModel
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string Extension { get; set; }

        public int Type { get; set; }

        public Guid SecretKey { get; set; }
    }
}
