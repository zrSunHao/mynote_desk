using SQLite;

namespace BlizzardWind.Desktop.Business.Entities
{
    public class ArticleFolder
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public Guid FamilyId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
