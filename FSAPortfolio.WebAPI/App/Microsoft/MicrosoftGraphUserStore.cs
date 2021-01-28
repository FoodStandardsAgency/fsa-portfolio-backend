using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Identity;
using FSAPortfolio.WebAPI.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
namespace FSAPortfolio.WebAPI.App.Microsoft
{
    public class MicrosoftGraphUserStore
    {
        private static HttpClient client = new HttpClient();
        private static string TenantId = ConfigurationManager.AppSettings["Azure.TenantId"];
        private static string ClientId = ConfigurationManager.AppSettings["Azure.ClientId"];
        private static string ClientSecret = ConfigurationManager.AppSettings["Azure.ClientSecret"];
        private const string AuthorityFormat = "https://login.microsoftonline.com/{0}/v2.0";
        private const string userSelect = "$select=id,displayName,givenName,surname,mail,userPrincipalName,department,companyName";

        private AuthenticationResult _auth;
        private PortfolioRoleManager roleManager;

        public MicrosoftGraphUserStore(PortfolioRoleManager roleManager = null)
        {
            this.roleManager = roleManager;
        }

        internal async Task<MicrosoftGraphUserModel> GetUserForPrincipalNameAsync(string term)
        {
            MicrosoftGraphUserModel user;
            var auth = await AuthenticateAsync();

            // Build the uri
            var uri = new UriBuilder($"https://graph.microsoft.com/v1.0/users/{term}?{userSelect}");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<MicrosoftGraphUserModel>(json);
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        user = null;
                        break;
                    default:
                        throw new HttpResponseException(response.StatusCode);
                }
            }
            return user;
        }
        internal async Task<MicrosoftGraphUserListResponse> GetUsersAsync(string term, int count = 10)
        {
            var auth = await AuthenticateAsync();
            var filter = $"$filter=startswith(displayName,'{term}') or startswith(givenName,'{term}') or startswith(surname,'{term}') or startswith(mail,'{term}') or startswith(userPrincipalName,'{term}')";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/users?{filter}&{userSelect}&$top={count}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) throw new HttpResponseException(response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            MicrosoftGraphUserListResponse users = JsonConvert.DeserializeObject<MicrosoftGraphUserListResponse>(json);
            return users;
        }

        internal async Task<IEnumerable<Role>> GetUserRolesAsync(string userId)
        {
            // First get group memberships
            var groups = await GetGroupMemberships(userId);
            var groupIds = groups.value.Select(g => g.Id);
            var roles = groupIds.Where(gid => GroupMembershipToRoleMap.GroupMap.ContainsKey(gid)).SelectMany(gid => GroupMembershipToRoleMap.GroupMap[gid]);
            var filteredRoles = await roleManager.GetFilteredRoleListAsync(roles, false);
            return filteredRoles.Select(r => new Role() { ViewKey = r });
        }

        private async Task<MicrosoftGraphGroupMemberListResponse> GetGroupMemberships(string userId)
        {
            MicrosoftGraphGroupMemberListResponse result = null;
            var auth = await AuthenticateAsync();
            var uri = new UriBuilder($"https://graph.microsoft.com/v1.0/users/{userId}/memberOf");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<MicrosoftGraphGroupMemberListResponse>(json);
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        break;
                    default:
                        throw new HttpResponseException(response.StatusCode);
                }
            }

            return result;
        }

        internal async Task<MicrosoftGraphUserModel> GetUserForAccessToken(string accessToken)
        {
            MicrosoftGraphUserModel user;

            // Build the uri
            var uri = new UriBuilder($"https://graph.microsoft.com/v1.0/me?{userSelect}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<MicrosoftGraphUserModel>(json);
            }
            else
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        user = null;
                        break;
                    default:
                        throw new HttpResponseException(response.StatusCode);
                }
            }


            return user;
        }

        private async Task<AuthenticationResult> AuthenticateAsync()
        {
            if (_auth == null)
            {
                IConfidentialClientApplication daemonClient = ConfidentialClientApplicationBuilder.Create(ClientId)
                    .WithAuthority(string.Format(AuthorityFormat, TenantId))
                    .WithClientSecret(ClientSecret)
                    .Build();

                _auth = await daemonClient
                    .AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
                    .ExecuteAsync();
            }
            return _auth;
        }




    }
}