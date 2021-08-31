using SQLite;

namespace BlizzardWind.Desktop.Business.Entities
{
    public class MarkResource
    {
        [PrimaryKey]
        public Guid ID { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string FileName { get; set; }

        public MarkResourceType Type { get; set; }

        public long Length { get; set; }

        [MaxLength(32)]
        public string Extension {  get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public enum MarkResourceType
    {
        image,
        word,
        excel,
        powerpoint,
        pdf,
        txt,
        audio,
        video,
    }
}
