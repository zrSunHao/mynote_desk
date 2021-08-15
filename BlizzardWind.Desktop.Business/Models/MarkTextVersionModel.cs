using System;

namespace BlizzardWind.Desktop.Business.Models
{
    public class MarkTextVersionModel
    {
        public string Name { get; set; }

        public DateTime? Time { get; set; }

        public string TimeText
        {
            get => Time.HasValue ? Time.Value.ToString("f") : string.Empty;
        }
    }
}
