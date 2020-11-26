using FSAPortfolio.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;

using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Identity
{
    // TODO: this doesn't need to implement these interfaces - created by ApplicationUserManager
    public class UserStore : IUserStore<ApplicationUser>, IUserStore<ApplicationUser, string>
    {
        private PortfolioContext context;
        private string TenantId = ConfigurationManager.AppSettings["Azure.TenantId"];
        private string ClientId = ConfigurationManager.AppSettings["Azure.ClientId"];
        private string ClientSecret = ConfigurationManager.AppSettings["Azure.ClientSecret"];
        private const string AuthorityFormat = "https://login.microsoftonline.com/{0}/v2.0";
        private const string userSelect = "$select=id,displayName,givenName,surname,mail,userPrincipalName,department";

        private const string TeamKeyPrefix = "AzureAD.Team.Name.";

        public UserStore(PortfolioContext context)
        {
            this.context = context;
        }

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // TODO: dispose
            //throw new NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            ApplicationUser result = null;
            var user = await context.Users
                .Include(u => u.AccessGroup)
                .FirstOrDefaultAsync(u => u.UserName == userName);
            if (user != null)
            {
                result = new ApplicationUser()
                {
                    UserName = user.UserName,
                    Id = user.Id.ToString(),
                    AccessGroupViewKey = user.AccessGroup.ViewKey,
                    PasswordHash = user.PasswordHash
                };
            }
            return result;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}