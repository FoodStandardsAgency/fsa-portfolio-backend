using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Microsoft;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Text.RegularExpressions;

namespace FSAPortfolio.WebAPI.App.Users
{
    public class UsersProvider
    {
        private PortfolioContext context;

        private const string TeamKeyPrefix = "AzureAD.Team.Name.";

        internal async Task<AddSupplierResponseModel> AddSupplierAsync(string portfolioViewKey, string userName, string passwordHash)
        {
            var response = new AddSupplierResponseModel();
            try
            {
                var accessGroup = await context.AccessGroups.SingleAsync(a => a.ViewKey == AccessGroupConstants.SupplierViewKey);
                var portfolio = await context.Portfolios.SingleAsync(p => p.ViewKey == portfolioViewKey);
                var roleList = $"{portfolio.IDPrefix}.Read";
                context.Users.Add(new User()
                {
                    Timestamp = DateTime.Now,
                    UserName = userName,
                    PasswordHash = passwordHash,
                    AccessGroup = accessGroup,
                    RoleList = roleList
                });
                await context.SaveChangesAsync();
                response.result = "Ok";
            }
            catch(DbUpdateException updateException)
            {
                var duplicateException = updateException.InnerException?.InnerException as SqlException;

                if(duplicateException != null && duplicateException.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.Users' with unique index 'IX_UserName'"))
                {
                    response.result = "Duplicate";
                }
                else
                {
                    throw updateException;
                }
            }
            return response;
        }


        private Lazy<Dictionary<string, string>> lazyTeamViewKeyMap; // Maps team name to team viewkey
        private Lazy<MicrosoftGraphUserStore> lazyMsGraph;

        public UsersProvider(PortfolioContext context = null)
        {
            this.context = context;
            lazyTeamViewKeyMap = new Lazy<Dictionary<string, string>>(() => 
                ConfigurationManager.AppSettings.AllKeys.Where(k => k.StartsWith(TeamKeyPrefix))
                .ToDictionary(k => ConfigurationManager.AppSettings[k], k => k.Substring(TeamKeyPrefix.Length)));

            lazyMsGraph = new Lazy<MicrosoftGraphUserStore>(() => new MicrosoftGraphUserStore());
        }

        internal async Task MapPeopleAsync(ProjectUpdateModel update, Project project)
        {
            project.Lead = await EnsureForAsync(update.oddlead, project.Lead, project.Reservation.Portfolio);
            project.KeyContact1 = await EnsureForAsync(update.key_contact1, project.KeyContact1, project.Reservation.Portfolio);
            project.KeyContact2 = await EnsureForAsync(update.key_contact2, project.KeyContact2, project.Reservation.Portfolio);
            project.KeyContact3 = await EnsureForAsync(update.key_contact3, project.KeyContact3, project.Reservation.Portfolio);
            await MapTeamMembersAsync(update, project);
        }

        private async Task<Person> EnsurePersonForPrincipalName(string name, Portfolio portfolio = null)
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
                    user = await lazyMsGraph.Value.GetUserForPrincipalNameAsync(name);
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
                if (person.Team_Id == null)
                {
                    // Get the AD user if haven't already
                    if (user == null && !string.IsNullOrWhiteSpace(person.ActiveDirectoryId))
                    {
                        user = await lazyMsGraph.Value.GetUserForPrincipalNameAsync(name);
                    }

                    // Look up the team from the user's department
                    if (user != null && user.department != null)
                    {
                        // Get the viewkey from the map (or create one from the department)
                        string teamViewKey;
                        if (!lazyTeamViewKeyMap.Value.TryGetValue(user.department, out teamViewKey))
                        {
                            // Strip none alpha characters from dept to get a viewkey, and lowercase it
                            Regex rgx = new Regex("[^a-zA-Z0-9]");
                            teamViewKey = rgx.Replace(user.department, "").ToLower();
                        }

                        // Find the team (or create one)
                        var team = await context.Teams.SingleOrDefaultAsync(t => t.ViewKey == teamViewKey);
                        if (team == null)
                        {
                            int order = await context.Teams.Select(t => t.Order).DefaultIfEmpty(0).MaxAsync();
                            team = new Team()
                            {
                                ViewKey = teamViewKey,
                                Name = user.department,
                                Order = order
                            };
                        }

                        person.Team = team;
                        if (portfolio.Teams != null)
                        {
                            if (!portfolio.Teams.Contains(team))
                            {
                                portfolio.Teams.Add(team);
                            }
                        }
                    }
                }
            }
            return person;
        }

        private async Task MapTeamMembersAsync(ProjectUpdateModel update, Project project)
        {
            if (project.People == null)
                project.People = new List<Person>();
            else
                project.People.Clear();

            if (update?.team != null && update.team.Length > 0)
            {
                foreach (var id in update.team)
                {
                    var person = await EnsurePersonForPrincipalName(id, project.Reservation.Portfolio);
                    if (person != null)
                    {
                        project.People.Add(person);
                    }

                }
            }

        }

        private async Task<Person> EnsureForAsync(ProjectPersonModel model, Person currentValue, Portfolio portfolio)
        {
            Person result = null;
            if (model?.Value != null)
            {
                if (currentValue?.ViewKey != model.Value) result = await EnsurePersonForPrincipalName(model.Value, portfolio);
                else result = currentValue;
            }
            return result;
        }

    }
}