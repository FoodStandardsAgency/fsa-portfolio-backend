using FSAPortfolio.Application.Models;
using FSAPortfolio.Application.Services;
using FSAPortfolio.Common;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.App.Microsoft;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMicrosoftGraphUserStoreService microsoftGraphUserStoreService;

        public UserService(IServiceContext context, IMicrosoftGraphUserStoreService microsoftGraphUserStoreService) : base(context)
        {
            this.microsoftGraphUserStoreService = microsoftGraphUserStoreService;
        }

        public async Task<UserSearchResponseModel> SearchUsersAsync(string portfolio, string term, bool includeNone = false)
        {
            var result = await microsoftGraphUserStoreService.GetUsersAsync(term);
            var response = PortfolioMapper.ActiveDirectoryMapper.Map<UserSearchResponseModel>(result, opt => opt.Items[nameof(ActiveDirectoryUserSelectModel.NoneOption)] = includeNone);
            return response;
        }

        public async Task<SupplierResponseModel> GetSuppliersAsync()
        {
            var response = new SupplierResponseModel()
            {
                Suppliers = await ServiceContext.PortfolioContext.Users
                    .Where(u => u.AccessGroup.ViewKey == AccessGroupConstants.SupplierViewKey)
                    .Select(s => s.UserName)
                    .ToListAsync()
            };
            return response;
        }

        public async Task<UserModel> GetADUserAsync(string userName)
        {
            UserModel result = null;
            var user = await ServiceContext.PortfolioContext.Users
                .Include(u => u.AccessGroup)
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user != null)
            {
                result = new UserModel()
                {
                    UserName = user.UserName,
                    AccessGroup = user.AccessGroup.ViewKey
                };
            }
            return result;
        }

        public async Task<UserModel> GetUserAsync(string userName, string passwordHash)
        {
            UserModel result = null;
            var user = await ServiceContext.PortfolioContext.Users
                .Include(u => u.AccessGroup)
                .FirstOrDefaultAsync(u => u.UserName == userName && u.PasswordHash == passwordHash);

            if (user != null)
            {
                result = new UserModel()
                {
                    UserName = user.UserName,
                    AccessGroup = user.AccessGroup.ViewKey
                };
            }
            return result;
        }


        public async Task SeedAccessGroups()
        {
            ServiceContext.AssertAdmin();
            var context = ServiceContext.PortfolioContext;
            context.AccessGroups.AddOrUpdate(ag => ag.ViewKey,
                new AccessGroup() { ViewKey = AccessGroupConstants.FSAViewKey, Description = AccessGroupConstants.FSAViewKey },
                new AccessGroup() { ViewKey = AccessGroupConstants.EditorViewKey, Description = AccessGroupConstants.EditorViewKey },
                new AccessGroup() { ViewKey = AccessGroupConstants.AdminViewKey, Description = AccessGroupConstants.AdminViewKey },
                new AccessGroup() { ViewKey = AccessGroupConstants.SupplierViewKey, Description = AccessGroupConstants.SupplierViewKey },
                new AccessGroup() { ViewKey = AccessGroupConstants.SuperuserViewKey, Description = AccessGroupConstants.SuperuserViewKey }
                );
            await context.SaveChangesAsync();
        }

        public async Task CreateUser(string userName, string passwordHash, string accessGroupViewKey)
        {
            // Only allow this for a fresh platform...
            var context = ServiceContext.PortfolioContext;
            if (context.Users.Count() == 0)
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