using Azure.Storage.Blobs.Models;

namespace AzureBlobStorage.Interfaces
{
    public interface IBlobStorageService
    {
        public Task<Stream> GetBlobAsync(string blobName);
        public Task<IEnumerable<string>> ListBlobAsync();
        public Task UploadBlobFileAsync(IFormFile file);
        
        public Task UploadBlobContentAsync(string content, string fileName);
        public Task<bool> DeleteBlobAsync(string blobName);
    }
}
