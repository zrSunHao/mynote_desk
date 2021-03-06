using SQLite;
using System;

namespace BlizzardWind.Desktop.Business.Entities
{
    public class NoteFamily
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [MaxLength(64)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
