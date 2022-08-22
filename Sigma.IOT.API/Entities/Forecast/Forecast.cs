using Sigma.IOT.API.Entities.Base;

namespace Sigma.IOT.API.Entities.Forecast
{
    public class Forecast : ForecastBase
    {
        public IEnumerable<Measurement>? Measurements { get; set; }
    }
}
