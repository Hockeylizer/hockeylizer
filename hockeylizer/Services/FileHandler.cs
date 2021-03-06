﻿using System.Collections.Generic;
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
        public static readonly string Key = "hlmNR03sKg8ju2kl4Lf9UMSxnCFBB9HR2r6K82phDHr0r0/sB3EwAANNw9ceWp/hi2ugNk2edoUmSNWZcP5SsA==";
    }

    public static class FileHandler
    {
        private static readonly BlobUtility utility = new BlobUtility(BlobCredentials.AccountName, BlobCredentials.Key);

        public static async Task<UploadFileResult> UploadVideo(IFormFile file, string containerName, string fileStart)
        {
            var fileName = fileStart + "-0";

            const string allowedFileTypes = "mp4 avi mpeg mov";
            string fileType;

            var firstAlt = file.ContentType.Split('/').LastOrDefault();

            if (allowedFileTypes.Contains(firstAlt))
            {
                fileType = firstAlt;
            }
            else if (file.ContentType.Contains("quicktime"))
            {
                fileType = "mov";
            }
            else
            {
                fileType = file.ContentType.Split('/').LastOrDefault() ?? "mp4";
            }
            

            while (await utility.BlobExistsOnCloud(containerName, fileName + "." + fileType))
            {
                var ids = fileName.Split('-');
                var lastDigit = int.Parse(ids.Last());

                lastDigit += 1;
                fileName = fileStart + "-" + lastDigit;
            }

            fileName = fileName + "." + fileType;
            var imageStream = file.OpenReadStream();

            var result = await utility.UploadBlob(fileName, containerName, imageStream);
            if (result != null)
            {
                var response = new UploadFileResult(result.Uri.ToString(), fileName);
                return response;
            }

            return new UploadFileResult();
        }

        public static async Task<KeyValuePair<bool, string>> DownloadBlob(string path, string blobname, string container)
        {
            try
            {
                var blob = utility.DownloadBlob(blobname, container);
                await blob.DownloadToFileAsync(path, FileMode.Create);

                return new KeyValuePair<bool, string>(true, "Lyckades.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new KeyValuePair<bool, string>(true, e.Message);
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
