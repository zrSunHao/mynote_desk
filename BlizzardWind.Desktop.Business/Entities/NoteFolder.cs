using SQLite;
using System;

namespace BlizzardWind.Desktop.Business.Entities
{
    public class NoteFolder
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public Guid FamilyId { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public Guid? CoverPictureId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string CoverPicturePath => _CoverPicturePath;

        private string _CoverPicturePath = string.Empty;

        public void SetCoverPicturePath(string path) => _CoverPicturePath = path;

        public Guid CoverPictureKey => _CoverPictureKey;

        private Guid _CoverPictureKey = Guid.Empty;

        public void SetCoverPictureKey(Guid key) => _CoverPictureKey = key;
    }
}
