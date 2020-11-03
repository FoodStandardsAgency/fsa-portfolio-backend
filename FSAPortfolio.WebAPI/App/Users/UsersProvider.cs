using FSAPortfolio.WebAPI.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.App.Users
{
    public class UsersProvider
    {
        private string TenantId = ConfigurationManager.AppSettings["Azure.TenantId"];
        private string ClientId = ConfigurationManager.AppSettings["Azure.ClientId"];
        private string ClientSecret = ConfigurationManager.AppSettings["Azure.ClientSecret"];
        private const string AuthorityFormat = "https://login.microsoftonline.com/{0}/v2.0";

        public async Task<MicrosoftGraphUserListResponse> GetUsersAsync(string term)
        {
            IConfidentialClientApplication daemonClient;
            daemonClient = ConfidentialClientApplicationBuilder.Create(ClientId)
                .WithAuthority(string.Format(AuthorityFormat, TenantId))
                .WithClientSecret(ClientSecret)
                .Build();

            AuthenticationResult auth = await daemonClient
                .AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
                .ExecuteAsync();

            HttpClient client = new HttpClient();

            var filter = $"$filter=startswith(displayName,'{term}') or startswith(givenName,'{term}') or startswith(surname,'{term}') or startswith(mail,'{term}') or startswith(userPrincipalName,'{term}')";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/users?{filter}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new HttpResponseException(response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            MicrosoftGraphUserListResponse users = JsonConvert.DeserializeObject<MicrosoftGraphUserListResponse>(json);
            return users;
        }
    }
}