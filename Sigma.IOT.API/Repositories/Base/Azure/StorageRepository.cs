using Azure.Storage.Blobs;

namespace Sigma.IOT.API.Repositories.Base.Azure
{
    public class StorageRepository : IStorageRepository
    {
        private readonly IConfiguration? _configuration;

        private string? BlobEndpoint { get => _configuration?.GetSection("Azure")["BlobEndpoint"]; }
        private string? ContainerName { get => _configuration?.GetSection("Azure")["ContainerName"]; }

        public IConfiguration? configuration
        {
            get
            {
                return _configuration;
            }
        }

        private BlobServiceClient _client;

        public StorageRepository(IConfiguration? configuration)
        {
            _configuration = configuration;

            _client = new BlobServiceClient(BlobEndpoint);
        }
        public async Task<MemoryStream> GetBlobStorage(string blobName)
        {
            MemoryStream memoryStream = new MemoryStream();

            await GetStorage(blobName, memoryStream);

            return memoryStream;
        }

        private async Task GetStorage(string blobName, MemoryStream memoryStream)
        {
            if (ContainerExists(ContainerName))
            {
                var container = _client.GetBlobContainerClient(ContainerName);

                if (BlobClientExists(container, blobName))
                {
                    BlobClient blobClient = container.GetBlobClient(blobName);

                    await blobClient.DownloadToAsync(memoryStream);

                    memoryStream.Position = 0;
                }
            }
        }

        public bool ContainerExists(string? ContainerName)
        {
            return _client.GetBlobContainerClient(ContainerName).Exists();
        }

        public bool BlobClientExists(BlobContainerClient container, string? blobName)
        {
            return container.GetBlobClient(blobName).Exists();
        }
    }
}
