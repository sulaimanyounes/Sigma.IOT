using Sigma.IOT.API.Repositories.Base.Azure;

namespace Sigma.IOT.API.Repositories.Forecast
{
    public class ForecastStorageRepository : StorageRepository, IForecastStorageRepository
    {
        public ForecastStorageRepository(IConfiguration configuration) :base(configuration)
        {

        }
    }
    
}
