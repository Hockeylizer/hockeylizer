using RestSharp.Authenticators;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
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

        public static async void SendMessage(string email)
        {
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "YOUR_DOMAIN_NAME", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Excited User <YOU@YOUR_DOMAIN_NAME>");
            request.AddParameter("to", "foo@example.com");
            request.AddParameter("subject", "Hello");
            request.AddParameter("text", "Testing some Mailgun awesomness!");
            request.AddParameter("html",
                                  "<html>HTML version of the body</html>");
            //request.AddFile("attachment", Path.Combine("files", "test.jpg"));
            //request.AddFile("attachment", Path.Combine("files", "test.txt"));
            request.Method = Method.POST;

            await Mailgun.ExecuteRequest<object>(request);
        }
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