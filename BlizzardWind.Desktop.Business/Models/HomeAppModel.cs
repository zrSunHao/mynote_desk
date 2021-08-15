namespace BlizzardWind.Desktop.Business.Models
{
    public class HomeAppModel
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        public int Type { get; set; }
    }

    public class HomeAppType
    {
        public const int EDITOR = 1;
    }
}
