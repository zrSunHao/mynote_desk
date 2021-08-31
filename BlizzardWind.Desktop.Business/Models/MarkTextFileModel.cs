using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Models
{
    public class MarkTextFileModel
    {
        public Guid ID { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string Extension { get; set; }

        public int Type { get; set; }
    }
}
