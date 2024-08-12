namespace WebApi.Filters
{
    public class Filter
    {
        public DateTime From { get; set; } = DateTime.MinValue;
        public DateTime To { get; set; } = DateTime.MaxValue;
        public string Search { get; set; } = string.Empty;
    }
}
