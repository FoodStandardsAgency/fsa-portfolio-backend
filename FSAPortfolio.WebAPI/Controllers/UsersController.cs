using FSAPortfolio.Entities;
using FSAPortfolio.PostgreSQL;
using FSAPortfolio.WebAPI.App.Microsoft;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {

        // Get: api/Users/search
        [AcceptVerbs("GET")]
        public async Task<UserSearchResponseModel> SearchUsers([FromUri] string portfolio, [FromUri] string term, [FromUri(Name = "addnone")] bool includeNone = false)
        {
            var provider = new MicrosoftGraphUserStore();
            var result = await provider.GetUsersAsync(term);
            var response = PortfolioMapper.ActiveDirectoryMapper.Map<UserSearchResponseModel>(result, opt => opt.Items[nameof(ActiveDirectoryUserSelectModel.NoneOption)] = includeNone);
            return response;
        }

        // Get: api/Users/suppliers
        [AcceptVerbs("GET")]
        public async Task<SupplierResponseModel> GetSuppliers()
        {
            using (var context = new PortfolioContext())
            {
                var response = new SupplierResponseModel()
                {
                    Suppliers = await context.Users
                        .Where(u => u.AccessGroup.ViewKey == AccessGroupConstants.SupplierViewKey)
                        .Select(s => s.UserName)
                        .ToListAsync()
                };
                return response;
            }
        }

        // POST: api/Users/addsupplier
        [AcceptVerbs("POST")]
        public async Task<AddSupplierResponseModel> AddSupplier([FromBody] AddSupplierModel model)
        {
            using (var context = new PortfolioContext())
            {
                var provider = new UsersProvider(context);
                return await provider.AddSupplierAsync(model.UserName, model.PasswordHash);
            }
        }

        // POST: api/Users/LegacyADUsers
        [AcceptVerbs("POST")]
        public UserModel GetADUser(UserRequestModel userRequest)
        {
            UserModel result = null;
            using (var context = new PortfolioContext())
            {
                var user = context.Users
                    .Include(u => u.AccessGroup)
                    .FirstOrDefault(u => u.UserName == userRequest.UserName);

                if (user != null)
                {
                    result = new UserModel() { 
                        UserName = user.UserName, 
                        AccessGroup = user.AccessGroup.ViewKey 
                    };
                }
            }
            return result;
        }

        // POST: api/Users/legacy
        [AcceptVerbs("POST")]
        public UserModel GetUser(UserRequestModel userRequest)
        {
            UserModel result = null;
            using (var context = new PortfolioContext())
            {
                var user = context.Users
                    .Include(u => u.AccessGroup)
                    .FirstOrDefault(u => u.UserName == userRequest.UserName && u.PasswordHash == userRequest.PasswordHash);

                if (user != null)
                {
                    result = new UserModel() { 
                        UserName = user.UserName, 
                        AccessGroup = user.AccessGroup.ViewKey
                    };
                }
            }
            return result;
        }
    }
}
