﻿using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using hockeylizer.Models;
using hockeylizer.Data;
using System.Linq;
using System.IO;
using System;

namespace hockeylizer.Services
{
    public static class FileHandler
    {
        private static readonly BlobUtility utility = new BlobUtility("hockeydata", "QmoOoi3h8htf3+Luqz7GhVe9WZavcDvn/DHqEzc25f9/Ii4zKeqTwuP+x9M9UbZWSVTGKnNW2rF89X/D6yza+A==");

        public async static Task<string> UploadVideo(IFormFile file, string containerName, string fileStart)
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
                return result.Uri.ToString();
            }

            return string.Empty;
        }

        public static bool DownloadBlob(string path, string blobname, string container)
        {
            try
            {
				var blob = utility.DownloadBlob(blobname, container);
				blob.DownloadToFileAsync(path, FileMode.Create);

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

        public static async Task<string> GetShareableVideoUrl(string vp)
        {
            return await utility.GetBlobSas(vp);
        }
    }
}
