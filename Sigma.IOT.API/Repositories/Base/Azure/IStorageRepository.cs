using Azure.Storage.Blobs;

namespace Sigma.IOT.API.Repositories.Base.Azure
{
    public interface IStorageRepository
    {
        IConfiguration? Configuration { get; }

        Task<MemoryStream> GetBlobStorage(string blobName);
        bool ContainerExists(string? ContainerName);
        bool BlobClientExists(BlobContainerClient container, string? blobName);

    }
}
