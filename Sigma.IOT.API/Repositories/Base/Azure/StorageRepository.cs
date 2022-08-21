using Azure.Storage.Blobs;

namespace Sigma.IOT.API.Repositories.Base.Azure
{
    public class StorageRepository : IStorageRepository
    {
        private const string BlobEndpoint = "BlobEndpoint=https://sigmaiotexercisetest.blob.core.windows.net/;SharedAccessSignature=sv=2017-11-09&ss=bfqt&srt=sco&sp=rl&se=2028-09-27T16:27:24Z&st=2018-09-27T08:27:24Z&spr=https&sig=eYVbQneRuiGn103jUuZvNa6RleEeoCFx1IftVin6wuA%3D";
        private const string ContainerName = "iotbackend";
        public async Task<MemoryStream> GetBlobStorage(string blobName)
        {
            MemoryStream memoryStream = new MemoryStream();

            BlobServiceClient serviceClient = new BlobServiceClient(BlobEndpoint);
            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(ContainerName);

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            await blobClient.DownloadToAsync(memoryStream);

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
