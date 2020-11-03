using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.App.Users
{
    public class UsersProvider
    {
        private PortfolioContext context;
        private string TenantId = ConfigurationManager.AppSettings["Azure.TenantId"];
        private string ClientId = ConfigurationManager.AppSettings["Azure.ClientId"];
        private string ClientSecret = ConfigurationManager.AppSettings["Azure.ClientSecret"];
        private const string AuthorityFormat = "https://login.microsoftonline.com/{0}/v2.0";

        public UsersProvider(PortfolioContext context = null)
        {
            this.context = context;
        }

        public async Task<MicrosoftGraphUserListResponse> GetUsersAsync(string term, int count = 10)
        {
            AuthenticationResult auth = await AuthenticateAsync();
            HttpClient client = new HttpClient();

            var filter = $"$filter=startswith(displayName,'{term}') or startswith(givenName,'{term}') or startswith(surname,'{term}') or startswith(mail,'{term}') or startswith(userPrincipalName,'{term}')";
            var select = "$select=displayName,givenName,surname,mail,userPrincipalName,department";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://graph.microsoft.com/v1.0/users?{filter}&{select}&$top={count}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode) throw new HttpResponseException(response.StatusCode);

            string json = await response.Content.ReadAsStringAsync();
            MicrosoftGraphUserListResponse users = JsonConvert.DeserializeObject<MicrosoftGraphUserListResponse>(json);
            return users;
        }

        private async Task<AuthenticationResult> AuthenticateAsync()
        {
            IConfidentialClientApplication daemonClient = ConfidentialClientApplicationBuilder.Create(ClientId)
                .WithAuthority(string.Format(AuthorityFormat, TenantId))
                .WithClientSecret(ClientSecret)
                .Build();

            AuthenticationResult auth = await daemonClient
                .AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" })
                .ExecuteAsync();
            return auth;
        }

        internal async Task<MicrosoftGraphUserModel> GetUserForPrincipalNameAsync(string term)
        {
            MicrosoftGraphUserModel user;

            // Build the uri
            var uri = new UriBuilder($"https://graph.microsoft.com/v1.0/users/{term}");
            AuthenticationResult auth = await AuthenticateAsync();
            HttpClient client = new HttpClient();
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

        internal async Task<Person> EnsurePersonForPrincipalName(string name)
        {
            Person person = null;
            if (!string.IsNullOrWhiteSpace(name))
            {
                person = context.People.SingleOrDefault(p => p.ActiveDirectoryPrincipalName == name || p.Email == name);
                if (person == null)
                {
                    var user = await GetUserForPrincipalNameAsync(name);
                    if (user != null)
                    {
                        person = PortfolioMapper.ActiveDirectoryMapper.Map<Person>(user);
                    }
                    else
                    {
                        // Assume an email was passed in
                        person = new Person() { Email = name };
                    }
                    person.Timestamp = DateTime.Now;
                }
            }
            return person;
        }


        internal async Task MapAsync(ProjectUpdateModel update, Project project)
        {
            project.Lead = await EnsureForAsync(update.oddlead, project.Lead);
            project.ServiceLead = await EnsureForAsync(update.servicelead, project.ServiceLead);
            project.KeyContact1 = await EnsureForAsync(update.key_contact1, project.KeyContact1);
            project.KeyContact2 = await EnsureForAsync(update.key_contact2, project.KeyContact2);
            project.KeyContact3 = await EnsureForAsync(update.key_contact3, project.KeyContact3);
        }

        private async Task<Person> EnsureForAsync(ProjectPersonModel model, Person currentValue)
        {
            Person result = null;
            if (model?.Value != null)
            {
                if (currentValue?.ViewKey != model.Value) result = await EnsurePersonForPrincipalName(model.Value);
                else result = currentValue;
            }
            return result;
        }



    }
}