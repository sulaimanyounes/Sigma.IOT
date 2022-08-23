using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sigma.IOT.API.Repositories.Base.Azure;

namespace Sigma.IOT.MSTest
{
    [TestClass]
    public class AzureBlobStorageTest 
    {
        private readonly IStorageRepository? _storageRepository;

        private IConfiguration? _configuration;

        public IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder();
                    _configuration = builder.AddJsonFile("appsettings.json").Build();
                }

                return _configuration;
            }
        }

        public AzureBlobStorageTest()
        {
            var services = new ServiceCollection();

            services.AddScoped<IStorageRepository, StorageRepository>();
            services.AddSingleton(Configuration);

            var serviceProvider = services.BuildServiceProvider();

            _storageRepository = serviceProvider.GetRequiredService<IStorageRepository>();
        }

        [TestMethod]
        public void BlobEndPointTest()
        {
            Assert.AreEqual(Configuration.GetSection("Azure")["BlobEndpoint"], _storageRepository?.Configuration?.GetSection("Azure")["BlobEndpoint"]);
        }

        [TestMethod]
        public void ContainerTest()
        {
            var containerName = _storageRepository?.Configuration?.GetSection("Azure")["ContainerName"];

            Assert.IsTrue(_storageRepository?.ContainerExists(containerName));
        }

        [TestMethod]
        public void BlobClientTest()
        {
            var blobeEndPoint = _storageRepository?.Configuration?.GetSection("Azure")["BlobEndpoint"];
            var containerName = _storageRepository?.Configuration?.GetSection("Azure")["ContainerName"];
            var blobName = $"dockan/temperature/2019-01-10";

            var client = new BlobServiceClient(blobeEndPoint);
            var container = client.GetBlobContainerClient(containerName);

            Assert.IsTrue(_storageRepository?.BlobClientExists(container, $"{blobName}.csv"));
        }
    }
}