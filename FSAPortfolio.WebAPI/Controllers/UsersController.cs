using FSAPortfolio.Entities;
using FSAPortfolio.PostgreSQL;
using FSAPortfolio.WebAPI.App.Identity;
using FSAPortfolio.WebAPI.App.Microsoft;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class UsersController : ApiController
    {

        // Get: api/Users/search
        [AcceptVerbs("GET")]
        [Authorize]
        public async Task<UserSearchResponseModel> SearchUsers([FromUri] string portfolio, [FromUri] string term, [FromUri(Name = "addnone")] bool includeNone = false)
        {
            var provider = new MicrosoftGraphUserStore();
            var result = await provider.GetUsersAsync(term);
            var response = PortfolioMapper.ActiveDirectoryMapper.Map<UserSearchResponseModel>(result, opt => opt.Items[nameof(ActiveDirectoryUserSelectModel.NoneOption)] = includeNone);
            return response;
        }

        // Get: api/Users/suppliers
        [AcceptVerbs("GET")]
        [Authorize]
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
        [Authorize]
        public async Task<AddSupplierResponseModel> AddSupplier([FromBody] AddSupplierModel model)
        {
            using (var context = new PortfolioContext())
            {
                var provider = new PersonProvider(context);
                return await provider.AddSupplierAsync(model.Portfolio, model.UserName, model.PasswordHash);
            }
        }

        // POST: api/Users/LegacyADUsers
        [AcceptVerbs("POST")]
        [Authorize]
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
        [Authorize]
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

        [AcceptVerbs("GET")]
        public IdentityResponseModel GetIdentity()
        {
            IdentityResponseModel result = null;

            var principal = RequestContext.Principal;
            var identity = principal?.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                result = new IdentityResponseModel()
                {
                    Roles = identity.Claims.Where(c => c.Type == identity.RoleClaimType).Select(c => c.Value.ToLower()).ToArray(),
                    AccessGroup = identity.Claims.SingleOrDefault(c => c.Type == ApplicationUser.AccessGroupClaimType)?.Value?.ToLower()
                };
            }
            return result;
        }

        [AcceptVerbs("POST")]
        public async Task CreateUser([FromBody] AddUserModel model)
        {
            using (var context = new PortfolioContext())
            {
                var sha256 = SHA256.Create();
                var hash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password))).Replace("-", "");
                var users = new UserProvider(context);
                await users.CreateUser(model.UserName, hash, model.AccessGroup);
            }
        }
    }
}
