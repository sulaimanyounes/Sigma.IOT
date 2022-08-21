using Sigma.IOT.API.Entities.Base;
using Sigma.IOT.API.Entities.Forecast;

namespace Sigma.IOT.API.Services.Forecast
{
    public interface IForecastService
    {
        Task<ApiResult<ForecastItem>> List(string device, string date, string sensor, int pageNumber, int pageSize);
        Task<ApiResult<ForecastItem>> List(string device, string date, int pageNumber, int pageSize);
    }
}
