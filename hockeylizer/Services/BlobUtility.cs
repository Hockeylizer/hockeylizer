using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using System.IO;
using System;

namespace hockeylizer.Services
{
    public class BlobUtility
    {
        public CloudStorageAccount storageAccount;
        public BlobUtility(string accountName, string accountKey)
        {
            string UserConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", accountName, accountKey);
            storageAccount = CloudStorageAccount.Parse(UserConnectionString);
        }

        public async Task<CloudBlockBlob> UploadBlob(string BlobName, string ContainerName, Stream stream)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName.ToLower());

            await container.CreateIfNotExistsAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            // blockBlob.UploadFromByteArray()
            try
            {
                await blockBlob.UploadFromStreamAsync(stream);
                return blockBlob;
            }
            catch (Exception e)
            {
                var r = e.Message;
                return null;
            }
        }

        public async Task<bool> BlobExistsOnCloud(string containerName, string key)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName.ToLower());
            await container.CreateIfNotExistsAsync();

            return await blobClient.GetContainerReference(containerName).GetBlockBlobReference(key).ExistsAsync();
        }

        public void DeleteBlob(string BlobName, string ContainerName)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            blockBlob.DeleteAsync();
        }

        public async Task<string> getBlobSas(string url)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blob = await blobClient.GetBlobReferenceFromServerAsync(new Uri(url));

            var sasConstraints = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                Permissions = SharedAccessBlobPermissions.Read
            };

            var sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
            return blob.Uri + sasBlobToken;
        }

        public CloudBlockBlob DownloadBlob(string BlobName, string ContainerName)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(BlobName);
            //blockBlob.DownloadToStream(Response.OutputStream);
            return blockBlob;
        }
    }
}
