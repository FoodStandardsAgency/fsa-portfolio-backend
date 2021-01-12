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
using FSAPortfolio.WebAPI.App.Users;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public class UserStore : IUserStore<ApplicationUser>, IUserStore<ApplicationUser, string>
    {
        private PortfolioContext context;
        private PortfolioRoleManager roleManager;
        public UserStore(PortfolioContext context, PortfolioRoleManager roleManager)
        {
            this.context = context;
            this.roleManager = roleManager;
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

                // The roles added for the user in the store
                var userRoleList = user.RoleList == null ? new string[0] : user.RoleList.Split(';', ',').Select(r => r.Trim()).ToArray();

                // Merge and take the intersection with required portfolio roles...
                var isSupplier = result.AccessGroupViewKey == AccessGroupConstants.SupplierViewKey;
                var roleList = await roleManager.GetFilteredRoleListAsync(userRoleList, isSupplier);

                if (user.RoleList != null)
                {
                    result.Roles = roleList
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