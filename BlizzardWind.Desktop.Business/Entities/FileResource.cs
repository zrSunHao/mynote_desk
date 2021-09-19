using SQLite;
using System;

namespace BlizzardWind.Desktop.Business.Entities
{
    public class FileResource
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string FileName { get; set; }

        public int Type { get; set; }

        public long Length { get; set; }

        public Guid? NoteId { get; set; }

        public Guid SecretKey { get; set; }

        [MaxLength(32)]
        public string Extension { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
