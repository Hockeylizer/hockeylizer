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
        private static readonly string Key = "pubkey-06788b0de391c0575096b7879e696de4";

        private static RestClient Client()
        {
            RestClient client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api", Mailgun.Key)
            };

            return client;
        }

        private static async Task<T> ExecuteRequest<T>(RestRequest request)
        {
            var client = Mailgun.Client();

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

        public static async Task<SendMessageResult> SendMessage(string email, string subject, string text)
        {
            RestRequest request = new RestRequest
            {
                Method = Method.POST
            };

            request.AddParameter("domain", "sandboxef2625f2f544472cb4b3d3311b8149b8.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";

            request.AddParameter("to", email);
            request.AddParameter("subject", subject);
            request.AddParameter("text", text);

            // CSV-file link
            //request.AddFile("attachment", Path.Combine("files", "test.jpg"));

            return await Mailgun.ExecuteRequest<SendMessageResult>(request);
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