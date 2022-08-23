using Sigma.IOT.API.Repositories.Base.Azure;
using Sigma.IOT.API.Repositories.Forecast;
using Sigma.IOT.API.Services.Forecast;

namespace Sigma.IOT.API.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            RegisterServices(services);
            RegisterRepositories(services);

            return services;
        }

        public static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IForecastStorageRepository, ForecastStorageRepository>();
            services.AddScoped<IStorageRepository, StorageRepository>();
        }

        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IForecastService, ForecastService>();

        }

    }
}
