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
            RestRequest request = new RestRequest
            {
                Resource = "{domain}/messages"
            };

            request.AddParameter("domain", "sandboxef2625f2f544472cb4b3d3311b8149b8.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandboxef2625f2f544472cb4b3d3311b8149b8.mailgun.org>");
            request.AddParameter("to", "Lennart Hammarström <" + email + ">");
            request.AddParameter("subject", "Hello Lennart Hammarström");
            request.AddParameter("text", "Congratulations Lennart Hammarström, you just sent an email with Mailgun!  You are truly awesome!");

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