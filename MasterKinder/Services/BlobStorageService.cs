using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace MasterKinder.Services
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly string _blobName;

        public BlobStorageService(IConfiguration configuration)
        {
            _connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"];
            _blobName = configuration["AzureBlobStorage:BlobName"];
        }

        public async Task<Stream> GetBlobAsync()
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainerClient.GetBlobClient(_blobName);

            var blobDownloadInfo = await blobClient.DownloadAsync();
            var memoryStream = new MemoryStream();
            await blobDownloadInfo.Value.Content.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }

    }
}