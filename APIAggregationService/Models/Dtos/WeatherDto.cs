namespace APIAggregationService.Models.Dtos
{
    public class WeatherDto
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double Pressure { get; set; }
        public string WeatherDescription { get; set; }
    }

}
