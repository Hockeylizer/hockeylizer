using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using hockeylizer.Models;
using System.Linq;
using System.IO;
using System;

namespace hockeylizer.Services
{
    public static class BlobCredentials
    {
        public static readonly string AccountName = "hockeydata";
        public static readonly string Key = "0KDxpIRz6u5M6VechBj8YOAGnDlG9dz11X/CyzmEE5tBesbiaa8+oEJZjpu3AUA5aQdyi3RK1Q264DgNYk3Tvw==";
    }

    public static class FileHandler
    {
        private static readonly BlobUtility utility = new BlobUtility(BlobCredentials.AccountName, BlobCredentials.Key);

        public static async Task<UploadFileResult> UploadVideo(IFormFile file, string containerName, string fileStart)
        {
            var fileName = fileStart + "-0";
            var filetype = file.ContentType.Split('/').LastOrDefault() ?? "mp4";

            while (await utility.BlobExistsOnCloud(containerName, fileName + "." + filetype))
            {
                var ids = fileName.Split('-');
                var lastDigit = int.Parse(ids.Last());

                lastDigit += 1;
                fileName = fileStart + "-" + lastDigit;
            }

            fileName = fileName + "." + filetype;
            var imageStream = file.OpenReadStream();

            var result = await utility.UploadBlob(fileName, containerName, imageStream);
            if (result != null)
            {
                var response = new UploadFileResult(result.Uri.ToString(), fileName);
                return response;
            }

            return new UploadFileResult();
        }

        public static async Task<bool> DownloadBlob(string path, string blobname, string container)
        {
            try
            {
                var blob = utility.DownloadBlob(blobname, container);
                await blob.DownloadToFileAsync(path, FileMode.Create);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool DeleteVideo(string path, string containerName)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var blobToDelete = path.Split('/').Last();
                utility.DeleteBlob(blobToDelete, containerName);

                return true;
            }

            return false;
        }

        public static async Task<string> GetShareableBlobUrl(string vp)
        {
            return await utility.GetBlobSas(vp);
        }
    }
}
