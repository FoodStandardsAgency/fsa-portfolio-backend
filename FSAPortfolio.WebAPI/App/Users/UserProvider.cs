using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Users
{
    public class UserProvider
    {
        private PortfolioContext context;

        public UserProvider(PortfolioContext context)
        {
            this.context = context;
        }

        public async Task SeedAccessGroups()
        {
            context.AccessGroups.AddOrUpdate(ag => ag.ViewKey, 
                new AccessGroup() { ViewKey = "editor", Description = "editor" },
                new AccessGroup() { ViewKey = "admin", Description = "admin" }, 
                new AccessGroup() { ViewKey = "superuser", Description = "superuser" });
            await context.SaveChangesAsync();
        }

        public async Task CreateUser(string userName, string passwordHash, string accessGroupViewKey)
        {
            // Only allow this for a fresh platform...
            if(context.Users.Count() == 0)
            {
                var accessGroup = await context.AccessGroups.SingleAsync(ag => ag.ViewKey == accessGroupViewKey);
                var user = new User()
                {
                    Timestamp = DateTime.Now,
                    UserName = userName,
                    PasswordHash = passwordHash,
                    AccessGroup = accessGroup
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
        }
    }
}