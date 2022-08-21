using Microsoft.AspNetCore.Mvc;
using Sigma.IOT.API.Services.Forecast;

namespace Sigma.IOT.API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/")]
    [ApiVersion("1.0")]
    public class ForecastController : ControllerBase
    {
        private readonly ILogger<ForecastController> _logger;
        private readonly IForecastService _forecastService;

        public ForecastController(ILogger<ForecastController> logger,
                                  IForecastService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService;
        }

        [HttpGet("devices/{device}/data/{date}/{sensor}")]
        public async Task<IActionResult> CollectMeasurementsByUnitAndSensor(string device, string date, string sensor, int pageNumber, int pageSize)
        {
            var results = await _forecastService.List(device, date, sensor, pageNumber, pageSize);

            return Ok(results);
        }

        [HttpGet("devices/{device}/data/{date}")]
        public async Task<IActionResult> CollectAllMeasurementsByUnit(string device, string date, int pageNumber, int pageSize)
        {
            var results = await _forecastService.List(device, date, pageNumber, pageSize);

            return Ok(results);
        }

    }
}
