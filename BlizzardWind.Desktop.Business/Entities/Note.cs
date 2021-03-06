using SQLite;
using System;

namespace BlizzardWind.Desktop.Business.Entities
{
    public class Note
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public Guid FolderId { get; set; }

        public int State { get; set; }

        public Guid? CoverPictureId { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(1024)]
        public string Keys { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }


        public string DisplayTime => CreatedAt.ToString("D");

        public string ContentLength => string.IsNullOrEmpty(Content)? "0": Content.Length.ToString();

        public string CoverPicturePath => _CoverPicturePath;

        private string _CoverPicturePath = string.Empty;

        public void SetCoverPicturePath(string path) => _CoverPicturePath = path;

        public Guid CoverPictureKey => _CoverPictureKey;

        private Guid _CoverPictureKey = Guid.Empty;

        public void SetCoverPictureKey(Guid key) => _CoverPictureKey = key;
    }
}
