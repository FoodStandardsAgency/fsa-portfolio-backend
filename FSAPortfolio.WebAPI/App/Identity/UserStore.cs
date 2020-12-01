using FSAPortfolio.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;

using System.Threading.Tasks;
using System.Web;
using FSAPortfolio.Entities.Users;
using System.Linq.Expressions;

namespace FSAPortfolio.WebAPI.App.Identity
{
    // TODO: this doesn't need to implement these interfaces - created by ApplicationUserManager
    public class UserStore : IUserStore<ApplicationUser>, IUserStore<ApplicationUser, string>
    {
        private PortfolioContext context;
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

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            int id = int.Parse(userId);
            return await FindUser(u => u.Id == id);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await FindUser(u => u.UserName == userName);
        }

        private async Task<ApplicationUser> FindUser(Expression<Func<User, bool>> predicate)
        {
            ApplicationUser result = null;
            var user = await context.Users
                .Include(u => u.AccessGroup)
                .FirstOrDefaultAsync(predicate);
            if (user != null)
            {
                result = new ApplicationUser()
                {
                    UserName = user.UserName,
                    Id = user.Id.ToString(),
                    AccessGroupViewKey = user.AccessGroup.ViewKey,
                    PasswordHash = user.PasswordHash
                };
                if (user.RoleList != null)
                {
                    result.Roles = user.RoleList.Split(';', ',')
                        .Select(r => new Role() { ViewKey = r.Trim() })
                        .ToList();
                }
            }
            return result;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}