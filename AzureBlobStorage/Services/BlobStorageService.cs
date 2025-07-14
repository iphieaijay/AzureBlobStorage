using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorage.Interfaces;
using System.IO;
using System.Text;

namespace AzureBlobStorage.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration config)
        {
            var connectionString = config["AzureBlobStorage:ConnectionString"];
            var containerName = config["AzureBlobStorage:ContainerName"];

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists(); 
        }
       public async Task<bool> DeleteBlobAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);

            var response = await blobClient.DeleteIfExistsAsync();
            return response.Value;

        }

        public async Task<Stream> GetBlobAsync(string blobName)
        {

            var blobClient = _containerClient.GetBlobClient(blobName);

            if (await blobClient.ExistsAsync())
            {
                var response = await blobClient.DownloadAsync();
                return response.Value.Content;
            }

            return null;
        }
        

        public async Task<IEnumerable<string>> ListBlobAsync()
        {
            var items = new List<string>();

            await foreach (var blob in _containerClient.GetBlobsAsync())
            {
                items.Add(blob.Name);
            }

            return items;
        }

        public async Task UploadBlobContentAsync(string content, string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            var bytes = Encoding.UTF8.GetBytes(content);
            using var memoryStream = new MemoryStream(bytes);
            await blobClient.UploadAsync(memoryStream, overwrite: true);
        }

        public async Task UploadBlobFileAsync(IFormFile file)
        {

            var blobClient = _containerClient.GetBlobClient(file.FileName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);
        }
    }
}
