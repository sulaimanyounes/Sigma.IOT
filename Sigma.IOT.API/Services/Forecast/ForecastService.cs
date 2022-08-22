using Sigma.IOT.API.Entities.Base;
using Sigma.IOT.API.Entities.Enums;
using Sigma.IOT.API.Entities.Forecast;
using Sigma.IOT.API.Repositories.Forecast;
using System.Text.RegularExpressions;

namespace Sigma.IOT.API.Services.Forecast
{
    public class ForecastService : IForecastService
    {
        #region VARS
        private readonly IForecastStorageRepository _forecastStorageRepository;
        #endregion

        #region CONSTRUCTOR
        public ForecastService(IForecastStorageRepository forecastStorageRepository)
        {
            _forecastStorageRepository = forecastStorageRepository;
        }
        #endregion

        #region PUBLIC METHODS
        public async Task<ApiResult<Entities.Forecast.Forecast>> List(string device, string date, string sensor, int pageNumber, int pageSize)
        {
            try
            {
                var blobName = $"{device}/{sensor.ToLower()}/{date}.csv";

                var memoryStream = await _forecastStorageRepository.GetBlobStorage(blobName);

                ProcessMemoryStream(out List<Entities.Forecast.Forecast> response, sensor, memoryStream, ref pageNumber, ref pageSize, out long totalLines);

                return MapResult(pageNumber, pageSize, totalLines, response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResult<Entities.Forecast.Forecast>> List(string device, string date, int pageNumber, int pageSize)
        {
            long totLines = 0;

            List<Entities.Forecast.Forecast> allResponse = new List<Entities.Forecast.Forecast>();

            try
            {
                foreach (string sensor in Enum.GetNames(typeof(SensorEnum)))
                {
                    var blobName = $"{device}/{sensor.ToLower()}/{date}.csv";

                    var memoryStream = await _forecastStorageRepository.GetBlobStorage(blobName);

                    ProcessMemoryStream(out List<Entities.Forecast.Forecast> response, sensor, memoryStream, ref pageNumber, ref pageSize, out long totalLines);

                    allResponse.AddRange(response);

                    if (totLines < totalLines) totLines = totalLines;
                }

                IEnumerable<Entities.Forecast.Forecast> responseGroup = ApplyGroup(allResponse);

                return MapResult(pageNumber, pageSize, totLines, responseGroup);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private static IEnumerable<Entities.Forecast.Forecast> ApplyGroup(List<Entities.Forecast.Forecast> allResponse)
        {
            return from response in allResponse
                   group (response.Measurements?.FirstOrDefault()) by response.Date into g
                   orderby g.Key
                   select new Entities.Forecast.Forecast()
                   {
                       Date = g.Key,
                       Measurements = g.Select(s => new Measurement()
                       {
                           SensorType = s.SensorType,
                           Value = s.Value
                       })
                   };
        }

        private static ApiResult<Entities.Forecast.Forecast> MapResult(int pageNumber, int pageSize, long totLines, IEnumerable<Entities.Forecast.Forecast> response)
        {
            var result = new ApiResult<Entities.Forecast.Forecast>()
            {
                Object = response.ToList(),
                TotalLines = totLines,
                PageSize = pageSize,
                PageNumber = pageNumber,
            };

            result.GetResult();
            return result;
        }

        private void ProcessMemoryStream(out List<Entities.Forecast.Forecast> response, 
                                         string sensor, 
                                         MemoryStream memoryStream, 
                                         ref int pageNumber, 
                                         ref int pageSize,
                                         out long totalLines)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 100;

            response = new List<Entities.Forecast.Forecast>();

            string[] read;
            char[] seperators = { ';' };
            string? line;
            Regex regex = new Regex(@"^\-,");

            List<Entities.Forecast.Forecast> items = new List<Entities.Forecast.Forecast>();

            using (StreamReader reader = new StreamReader(memoryStream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    read = line.Split(seperators, StringSplitOptions.None);

                    if (read.Length > 0)
                        items.Add(new Entities.Forecast.Forecast
                        {
                            Date = DateTime.Parse(read[0]),
                            Measurements = new List<Measurement>()
                            { new Measurement 
                                {
                                    Value = regex.IsMatch(read[1]) ? float.Parse("-0." + read[1].Substring(2))
                                                                                   : float.Parse(read[1].Replace(",", ".")),
                                    SensorType = sensor 
                                }
                            }
                        });
                }
            }

            totalLines = items.Count();

            response.AddRange(items.Skip(pageSize*(pageNumber-1)).Take(pageSize));
        }

        #endregion
    }
}
