using RestSharp.Authenticators;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.IO;
using System;

namespace hockeylizer.Services
{
    public static class Mailgun
    {
        private static readonly string MailKey = "key-1151be6372bbd04e9e2891123ceba58d";
        private static readonly string ValKey = "pubkey-06788b0de391c0575096b7879e696de4";

        private static RestClient Client(bool mail)
        {
            RestClient client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", mail ? Mailgun.MailKey : Mailgun.ValKey)
            };

            return client;
        }

        private static async Task<T> ExecuteRequest<T>(RestRequest request)
        {
            var client = Mailgun.Client(false);

            var source = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, restResponse =>
            {
                if (restResponse.ErrorException != null)
                {
                    const string message = "Error retrieving response.";
                    throw new Exception(message, restResponse.ErrorException);
                }
                source.SetResult(restResponse);
            });

            var result = await source.Task;
            return JsonConvert.DeserializeObject<T>(result.Content);
        }

        private static async Task<T> TryExecuteRequest<T>(RestRequest request)
        {
            var client = Mailgun.Client(true);

            var source = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, restResponse =>
            {
                if (restResponse.ErrorException != null)
                {
                    const string message = "Error retrieving response.";
                    throw new Exception(message, restResponse.ErrorException);
                }
                source.SetResult(restResponse);
            });

            var result = await source.Task;
            return JsonConvert.DeserializeObject<T>(result.Content);
        }

        public static async Task<EmailValidationResult> ValidateEmail(string email)
        {
            RestRequest request = new RestRequest
            {
                Resource = "/address/validate"
            };

            request.AddParameter("address", email);

            return await Mailgun.ExecuteRequest<EmailValidationResult>(request);
        }

        public static async Task<SendMessageResult> SendMessage(string email, string subject, string text, byte[] attachment = null, string attachmentName = null, string attachmentMimeType = null)
        {
            RestRequest request = new RestRequest
            {
                Method = Method.POST,
                Resource = "/mg.dispono.nu/messages"
            };

            request.AddParameter("domain", "mg.dispono.nu", ParameterType.UrlSegment);

            request.AddParameter("to", email);
            request.AddParameter("from", "noreply@drhockey.com");

            request.AddParameter("subject", subject);
            request.AddParameter("text", text);

            if (!(attachment == null) && attachment.Length > 0)
            {
                if (attachmentName == null) attachmentName = "attachment";
                request.AddFile("attachment", attachment, attachmentName, attachmentMimeType);
            }

            var res = await Mailgun.TryExecuteRequest<SendMessageResult>(request);
            return res;
        }
    }

    public class SendMessageResult
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("id")]
        public string MessageId { get; set; }
    }

    public class EmailValidationResult
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("did_you_mean")]
        public string Suggestions { get; set; }

        [JsonProperty("is_valid")]
        public bool Valid { get; set; }
    }
}