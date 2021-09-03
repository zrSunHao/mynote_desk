using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Entities
{
    public class Article
    {
        [PrimaryKey]
        public Guid ID { get; set; }

        public Guid? FolderID { get; set; }

        public int State { get; set; }

        public Guid? CoverPictureID { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(1024)]
        public string Keys { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
