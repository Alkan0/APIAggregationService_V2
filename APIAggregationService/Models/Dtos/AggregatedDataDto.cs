namespace APIAggregationService.Models.Dtos
{
    public class AggregatedDataDto
    {
        public DateTime Date { get; set; }
        public int Relevance { get; set; }
        public string Category { get; set; }
        public string Data { get; set; } // Placeholder for actual data
    }
}
