using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Sigma.IOT.API.Repositories.Base.Azure;

namespace Sigma.IOT.MSTest
{
    [TestClass]
    public class AzureBlobStorage 
    {
        private IConfiguration? _configuration;
        public IConfiguration? configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder();
                    builder.AddJsonFile("appsettings.json");
                    _configuration = builder.Build();
                }
                return _configuration;
            }
        }

        [TestMethod]
        public void BlobEndPointTest()
        {
            var storage = new StorageRepository(configuration);

            Assert.AreEqual(configuration?.GetSection("Azure")["BlobEndpoint"], storage.configuration?.GetSection("Azure")["BlobEndpoint"]);
        }

        [TestMethod]
        public void ContainerTest()
        {
            var storage = new StorageRepository(configuration);

            var containerName = storage.configuration?.GetSection("Azure")["ContainerName"];

            Assert.IsTrue(storage.ContainerExists(containerName));
        }

        [TestMethod]
        public void BlobClientTest()
        {
            var storage = new StorageRepository(configuration);

            var blobeEndPoint = storage.configuration?.GetSection("Azure")["BlobEndpoint"];
            var containerName = storage.configuration?.GetSection("Azure")["ContainerName"];
            var blobName = $"dockan/temperature/2019-01-10";

            var client = new BlobServiceClient(blobeEndPoint);
            var container = client.GetBlobContainerClient(containerName);

            Assert.IsTrue(storage.BlobClientExists(container, $"{blobName}.csv"));
        }
    }
}