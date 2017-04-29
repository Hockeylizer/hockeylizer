using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using hockeylizer.Models;
using hockeylizer.Data;
using System.Linq;
using System.IO;

namespace hockeylizer.Services
{
    public class ImageHandler
    {
        private static readonly BlobUtility utility = new BlobUtility("hockeydata", "QmoOoi3h8htf3+Luqz7GhVe9WZavcDvn/DHqEzc25f9/Ii4zKeqTwuP+x9M9UbZWSVTGKnNW2rF89X/D6yza+A==");

        public async static Task<UploadFileResult> UploadVideo(IFormFile file, string containerName, string fileStart)
        {
            var fileName = fileStart + "-0";
            var filetype = file.ContentType;

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
                var result = new UploadFileResult(result.Uri.ToString(), fileName);
                return result.Uri.ToString();
            }

            return new UploadFileResult();
        }

        public static bool DeleteImage(int videoId, ApplicationDbContext db)
        {
            var video = db.Videos.Find(videoId);

            if (!string.IsNullOrEmpty(video?.VideoPath))
            {
                var containerName = video.Player.RetrieveContainerName();

                var blobToDelete = video.VideoPath.Split('/').Last();
                utility.DeleteBlob(blobToDelete, containerName);

                video.Delete();
                db.SaveChanges();

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
