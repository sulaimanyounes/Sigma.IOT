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

        /// <summary>
        /// Endpoint to list the forecast measurments by sensor
        /// </summary>
        /// <param name="device">device unit name, example "dockan"</param>
        /// <param name="date">date of the collected forcasts, format yyyy-MM-dd, example "2019-01-10"</param>
        /// <param name="sensor"> Sensor type, example temperature, humidity or rainfall</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of objetcs per page</param>
        /// <returns>List of measurments for a specified sensor</returns>
        [HttpGet("devices/{device}/data/{date}/{sensor}")]
        public async Task<IActionResult> CollectMeasurementsByUnitAndSensor(string device, string date, string sensor, int pageNumber, int pageSize)
        {
            var results = await _forecastService.List(device, date, sensor, pageNumber, pageSize);

            return Ok(results);
        }

        /// <summary>
        /// Endpoint to list the forecast measurments for all sensors
        /// </summary>
        /// <param name="device">device unit name, example "dockan"</param>
        /// <param name="date">date of the collected forcasts, format yyyy-MM-dd, example "2019-01-10"</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Number of objetcs per page</param>
        /// <returns>List of measurments for all sensors</returns>
        [HttpGet("devices/{device}/data/{date}")]
        public async Task<IActionResult> CollectAllMeasurementsByUnit(string device, string date, int pageNumber, int pageSize)
        {
            var results = await _forecastService.List(device, date, pageNumber, pageSize);

            return Ok(results);
        }

    }
}
