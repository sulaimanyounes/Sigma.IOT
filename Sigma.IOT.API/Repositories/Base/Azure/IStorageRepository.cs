namespace Sigma.IOT.API.Repositories.Base.Azure
{
    public interface IStorageRepository
    {
        Task<MemoryStream> GetBlobStorage(string blobName);
    }
}
