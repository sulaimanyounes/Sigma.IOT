using Sigma.IOT.API.Entities.Base;
using Sigma.IOT.API.Entities.Enums;
using Sigma.IOT.API.Entities.Forecast;
using Sigma.IOT.API.Repositories.Forecast;
using System.Text.RegularExpressions;

namespace Sigma.IOT.API.Services.Forecast
{
    public class ForecastService : IForecastService
    {
        private readonly IForecastStorageRepository _forecastStorageRepository;

        public ForecastService(IForecastStorageRepository forecastStorageRepository)
        {
            _forecastStorageRepository = forecastStorageRepository;
        }

        public async Task<ApiResult<ForecastItem>> List(string device, string date, string sensor, int pageNumber, int pageSize)
        {
            try
            {
                var blobName = $"{device}/{sensor.ToLower()}/{date}.csv";

                var memoryStream = await _forecastStorageRepository.GetBlobStorage(blobName);

                ProcessMemoryStream(out List<ForecastItem> response, sensor, memoryStream, ref pageNumber, ref pageSize, out long totalLines);

                var result = new ApiResult<ForecastItem>()
                {
                    Object = response,
                    TotalLines = totalLines,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    Success = true
                };

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResult<ForecastItemAll>> List(string device, string date, int pageNumber, int pageSize)
        {
            long totLines = 0;

            List<ForecastItem> allResponse = new List<ForecastItem>();

            try
            {
                foreach (string sensor in Enum.GetNames(typeof(SensorEnum)))
                {
                    var blobName = $"{device}/{sensor.ToLower()}/{date}.csv";

                    var memoryStream = await _forecastStorageRepository.GetBlobStorage(blobName);

                    ProcessMemoryStream(out List<ForecastItem> response, sensor, memoryStream, ref pageNumber, ref pageSize, out long totalLines);

                    allResponse.AddRange(response);

                    if (totLines < totalLines) totLines = totalLines;
                }

                var responseGroup = from response in allResponse
                                    group (response.Measurement) by response.Date into g
                                    orderby g.Key
                                    select new ForecastItemAll() { 
                                        Date = g.Key,
                                        Measurements = g.Select(s => new Measurement() { 
                                        SensorType = s.SensorType, 
                                        Value = s.Value})
                                        };


                var result =  new ApiResult<ForecastItemAll>()
                {
                    Object = responseGroup.ToList(),
                    TotalLines = totLines,
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    Success = true
                };

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ProcessMemoryStream(out List<ForecastItem> response, 
                                         string sensor, 
                                         MemoryStream memoryStream, 
                                         ref int pageNumber, 
                                         ref int pageSize,
                                         out long totalLines)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 100;

            response = new List<ForecastItem>();

            string[] read;
            char[] seperators = { ';' };
            string? line;
            Regex regex = new Regex(@"^\-,");

            List<ForecastItem> items = new List<ForecastItem>();

            using (StreamReader reader = new StreamReader(memoryStream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    read = line.Split(seperators, StringSplitOptions.None);

                    if (read.Length > 0)
                        items.Add(new ForecastItem
                        {
                            Date = DateTime.Parse(read[0]),
                            Measurement = new Measurement()
                            {
                                Value = regex.IsMatch(read[1]) ? float.Parse("-0." + read[1].Substring(2))
                                                                               : float.Parse(read[1].Replace(",", ".")),
                                SensorType = sensor }
                        });
                }
            }

            totalLines = items.Count();

            response.AddRange(items.Skip(pageSize*(pageNumber-1)).Take(pageSize));
        }
    }
}
