namespace Sigma.IOT.API.Entities.Forecast
{
    public class ForecastItem
    {
        public DateTime Date { get; set; }
        public float Value { get; set; }
        public string? SensorType { get; set; }
    }
}
