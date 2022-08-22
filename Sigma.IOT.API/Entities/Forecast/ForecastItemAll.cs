namespace Sigma.IOT.API.Entities.Forecast
{
    public class ForecastItemAll
    {
        public DateTime Date { get; set; }
        public IEnumerable<Measurement>? Measurements { get; set; }
    }
}
