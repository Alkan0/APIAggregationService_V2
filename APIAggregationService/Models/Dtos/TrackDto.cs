namespace APIAggregationService.Models.Dtos
{
    public class TrackDto
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string[] Artists { get; set; }
        public int DurationMs { get; set; }
        public int Popularity { get; set; }
        public string ReleaseDate { get; set; }
        public string PreviewUrl { get; set; }
    }
}
