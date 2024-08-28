namespace APIAggregationService.Models.Dtos
{
    public class AlbumDto
    {
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public string[] Artists { get; set; }
        public List<TrackDto> Tracks { get; set; }
    }

}
