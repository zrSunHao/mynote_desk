using SQLite;
using System;

namespace BlizzardWind.Desktop.Business.Entities
{
    public class Information
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed, MaxLength(128)]
        public string Key { get; set; }

        [MaxLength(1024)]
        public string Value { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
