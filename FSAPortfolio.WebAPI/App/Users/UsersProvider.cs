using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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
        private const string userSelect = "$select=id,displayName,givenName,surname,mail,userPrincipalName,department";

        private const string TeamKeyPrefix = "AzureAD.Team.Name.";
        private Lazy<Dictionary<string, string>> lazyTeamViewKeyMap; // Maps team name to team viewkey

        public UsersProvider(PortfolioContext context = null)
        {
            this.context = context;
            lazyTeamViewKeyMap = new Lazy<Dictionary<string, string>>(() => 
                ConfigurationManager.AppSettings.AllKeys.Where(k => k.StartsWith(TeamKeyPrefix))
                .ToDictionary(k => ConfigurationManager.AppSettings[k], k => k.Substring(TeamKeyPrefix.Length)));
        }

        public async Task<MicrosoftGraphUserListResponse> GetUsersAsync(string term, int count = 10)
        {
            AuthenticationResult auth = await AuthenticateAsync();
            HttpClient client = new HttpClient();

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
            var uri = new UriBuilder($"https://graph.microsoft.com/v1.0/users/{term}?{userSelect}");
            AuthenticationResult auth = await AuthenticateAsync();
            using (HttpClient client = new HttpClient())
            {
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
            }

            return user;
        }

        internal async Task<Person> EnsurePersonForPrincipalName(string name, Portfolio portfolio = null)
        {
            Person person = null;
            if (!string.IsNullOrWhiteSpace(name))
            {
                MicrosoftGraphUserModel user = null;
                person = 
                    context.People.Local.SingleOrDefault(p => p.ActiveDirectoryPrincipalName == name || p.Email == name) ??
                    await context.People.SingleOrDefaultAsync(p => p.ActiveDirectoryPrincipalName == name || p.Email == name);
                if (person == null)
                {
                    user = await GetUserForPrincipalNameAsync(name);
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
                    context.People.Add(person);
                }

                // Set the team
                if(person.Team_Id == null)
                {
                    // Get the AD user if haven't already
                    if (user == null && !string.IsNullOrWhiteSpace(person.ActiveDirectoryId))
                    {
                        user = await GetUserForPrincipalNameAsync(name);
                    }

                    // Look up the team from the user's department
                    if(user != null)
                    {
                        // Get the viewkey from the map
                        string teamViewKey;
                        if(lazyTeamViewKeyMap.Value.TryGetValue(user.department, out teamViewKey))
                        {
                            // Find the team
                            var team = await context.Teams.SingleOrDefaultAsync(t => t.ViewKey == teamViewKey);
                            if(team != null)
                            {
                                person.Team_Id = team.Id;
                                if(portfolio.Teams != null)
                                {
                                    if(!portfolio.Teams.Contains(team))
                                    {
                                        portfolio.Teams.Add(team);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return person;
        }


        internal async Task MapPeopleAsync(ProjectUpdateModel update, Project project)
        {
            project.Lead = await EnsureForAsync(update.oddlead, project.Lead, project.Reservation.Portfolio);
            project.ServiceLead = await EnsureForAsync(update.servicelead, project.ServiceLead, project.Reservation.Portfolio);
            project.KeyContact1 = await EnsureForAsync(update.key_contact1, project.KeyContact1);
            project.KeyContact2 = await EnsureForAsync(update.key_contact2, project.KeyContact2);
            project.KeyContact3 = await EnsureForAsync(update.key_contact3, project.KeyContact3);
            await MapTeamMembersAsync(update, project);
        }

        internal async Task MapTeamMembersAsync(ProjectUpdateModel update, Project project)
        {
            project.People.Clear();
            if (update?.team != null && update.team.Length > 0)
            {
                foreach (var id in update.team)
                {
                    var person = await EnsurePersonForPrincipalName(id);
                    if (person != null)
                    {
                        project.People.Add(person);
                    }

                }
            }

        }

        private async Task<Person> EnsureForAsync(ProjectPersonModel model, Person currentValue, Portfolio portfolio = null)
        {
            Person result = null;
            if (model?.Value != null)
            {
                if (currentValue?.ViewKey != model.Value) result = await EnsurePersonForPrincipalName(model.Value, portfolio);
                else result = currentValue;
            }
            return result;
        }

        internal async Task<string> TestAsync(string term)
        {
            MicrosoftGraphUserModel user;
            var select = "$select=id,displayName,givenName,surname,mail,userPrincipalName,department";

            // Build the uri
            var uri = new UriBuilder($"https://graph.microsoft.com/v1.0/users/{term}?{select}");
            string json = "No response";
            AuthenticationResult auth = await AuthenticateAsync();
            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri.ToString());
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
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
            }

            return json;
        }
    }
}