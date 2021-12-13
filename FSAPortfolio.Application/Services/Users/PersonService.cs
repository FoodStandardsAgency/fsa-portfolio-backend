using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.Application.Services.Microsoft;
using FSAPortfolio.Application.Mapping;
using FSAPortfolio.Application.Models;
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
using FSAPortfolio.Application.Services;
using FSAPortfolio.Common;
using FSAPortfolio.Common.Logging;

namespace FSAPortfolio.Application.Services.Users
{
    public class PersonService : BaseService, IPersonService
    {
        private const string TeamKeyPrefix = "AzureAD.Team.Name.";
        private Lazy<Dictionary<string, string>> lazyTeamViewKeyMap; // Maps team name to team viewkey
        private IMicrosoftGraphUserStoreService msgraphService;

        public PersonService(IServiceContext serviceContext, IMicrosoftGraphUserStoreService msgraphService) : base(serviceContext)
        {
            lazyTeamViewKeyMap = new Lazy<Dictionary<string, string>>(() =>
                ConfigurationManager.AppSettings.AllKeys.Where(k => k.StartsWith(TeamKeyPrefix))
                .ToDictionary(k => ConfigurationManager.AppSettings[k], k => k.Substring(TeamKeyPrefix.Length)));

            this.msgraphService = msgraphService;
        }
        public async Task<AddSupplierResponseModel> AddSupplierAsync(string portfolioViewKey, string userName, string passwordHash)
        {
            var response = new AddSupplierResponseModel();
            try
            {
                var context = ServiceContext.PortfolioContext;
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
            catch (DbUpdateException updateException)
            {
                var duplicateException = updateException.InnerException?.InnerException as SqlException;

                if (duplicateException != null && duplicateException.Message.StartsWith("Cannot insert duplicate key row in object 'dbo.Users' with unique index 'IX_UserName'"))
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

        public async Task MapPeopleAsync(ProjectUpdateModel update, Project project)
        {
            project.Lead = await EnsureForAsync(update.oddlead, project.Lead, project.Reservation.Portfolio);
            project.KeyContact1 = await EnsureForAsync(update.key_contact1, project.KeyContact1, project.Reservation.Portfolio);
            project.KeyContact2 = await EnsureForAsync(update.key_contact2, project.KeyContact2, project.Reservation.Portfolio);
            project.KeyContact3 = await EnsureForAsync(update.key_contact3, project.KeyContact3, project.Reservation.Portfolio);
            await MapTeamMembersAsync(update, project);
        }

        public async Task ResetADReferencesAsync()
        {
            ServiceContext.AssertAdmin();
            var context = ServiceContext.PortfolioContext;
            foreach (var person in context.People.Where(p => p.ActiveDirectoryPrincipalName == null))
            {
                if (person.Surname == null && person.Firstname == null && person.Email != null && !person.Email.Contains("@"))
                {
                    await EnsurePersonForPrincipalName(person.Email);
                }
            }
            await context.SaveChangesAsync();
        }

        public async Task RemoveDuplicatesAsync()
        {
            ServiceContext.AssertAdmin();
            var context = ServiceContext.PortfolioContext;

            var dupes = await (from p in context.People
                               where p.ActiveDirectoryId != null
                               group p by p.ActiveDirectoryId into adPeople
                               where adPeople.Count() > 1
                               select adPeople).ToListAsync();

            foreach(var dupe in dupes)
            {
                Person latest = null;
                foreach(var person in dupe.OrderByDescending(p => p.Timestamp))
                {
                    if(latest == null)
                    {
                        latest = person;
                    }
                    else
                    {
                        var projects = await context.Projects
                            .Include(p => p.KeyContact1)
                            .Include(p => p.KeyContact2)
                            .Include(p => p.KeyContact3)
                            .Include(p => p.People)
                            .Where(p => 
                            p.Lead_Id == person.Id ||
                            p.KeyContact1.Id == person.Id ||
                            p.KeyContact2.Id == person.Id ||
                            p.KeyContact3.Id == person.Id ||
                            p.People.Any(pp => pp.Id == person.Id) ||
                            p.Updates.Any(u => u.Person.Id == person.Id)
                            ).ToListAsync();
                        foreach(var project in projects)
                        {
                            if (project.Lead_Id == person.Id) project.Lead = latest;
                            if (project.KeyContact1?.Id == person.Id) project.KeyContact1 = latest;
                            if (project.KeyContact2?.Id == person.Id) project.KeyContact2 = latest;
                            if (project.KeyContact3?.Id == person.Id) project.KeyContact3 = latest;
                            if (project.People.Contains(person))
                            {
                                project.People.Remove(person);
                                if (!project.People.Contains(latest)) project.People.Add(latest);
                            }
                        }
                        context.People.Remove(person);
                    }
                }
            }

            await context.SaveChangesAsync();
        }


        private async Task<Person> EnsurePersonForPrincipalName(string name, Portfolio portfolio = null)
        {
            Person person = null;
            if (!string.IsNullOrWhiteSpace(name))
            {
                try
                {
                    var context = ServiceContext.PortfolioContext;
                    MicrosoftGraphUserModel user = null;
                    person =
                        context.People.Local.SingleOrDefault(p => p.ActiveDirectoryPrincipalName == name || p.Email == name) ??
                        await context.People.SingleOrDefaultAsync(p => p.ActiveDirectoryPrincipalName == name || p.Email == name);
                    if (person == null || person.ActiveDirectoryId == null)
                    {
                        if (name.Contains("@"))
                        {
                            user = await msgraphService.GetUserForPrincipalNameAsync(name);
                        }
                        else
                        {
                            var result = await msgraphService.GetUsersAsync(name);
                            var users = result.value.Where(u => u.companyName == "Food Standards Agency" && u.department != null).ToList();
                            if (users.Count == 1)
                            {
                                user = users[0];
                            }
                        }

                        if (user != null)
                        {
                            if (person == null)
                            {
                                // Assume an email was passed in
                                person = new Person() { Email = name };
                                person.Timestamp = DateTime.Now;
                                context.People.Add(person);
                            }
                            PortfolioMapper.ActiveDirectoryMapper.Map(user, person);
                        }
                    }

                    // Set the team
                    if (person.Team_Id == null)
                    {
                        // Get the AD user if haven't already
                        if (user == null && !string.IsNullOrWhiteSpace(person.ActiveDirectoryId))
                        {
                            user = await msgraphService.GetUserForPrincipalNameAsync(name);
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
                            var team = await GetExistingTeamAsync(teamViewKey);
                            if (team == null)
                            {
                                int order = await GetNextOrderAsync();
                                team = new Team()
                                {
                                    ViewKey = teamViewKey,
                                    Name = user.department,
                                    Order = order
                                };
                            }

                            person.Team = team;
                            if (portfolio?.Teams != null)
                            {
                                if (!portfolio.Teams.Contains(team))
                                {
                                    portfolio.Teams.Add(team);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    AppLog.TraceError($"Exception ensuring Person record for '{name}'");
                    AppLog.Trace(e);
                    throw e;
                }

            }
            return person;
        }

        private async Task<int> GetNextOrderAsync()
        {
            var context = ServiceContext.PortfolioContext;
            var storeMax = await context.Teams.Select(t => t.Order).DefaultIfEmpty(-1).MaxAsync();
            var localMax = context.Teams.Local.Select(t => t.Order).DefaultIfEmpty(-1).Max();
            return (localMax > storeMax ? localMax : storeMax) + 1;
        }

        private async Task<Team> GetExistingTeamAsync(string teamViewKey)
        {
            var context = ServiceContext.PortfolioContext;
            // Check local first, then the database
            return context.Teams.Local.SingleOrDefault(t => t.ViewKey == teamViewKey) ?? (await context.Teams.SingleOrDefaultAsync(t => t.ViewKey == teamViewKey));
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