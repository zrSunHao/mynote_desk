namespace BlizzardWind.Desktop.Business.Models
{
    public class PagingResult<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }

        public int FilterTotal { get; set; }

        public List<T> Items { get; set; }
    }
}
