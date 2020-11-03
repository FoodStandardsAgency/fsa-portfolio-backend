using FSAPortfolio.Entities;
using FSAPortfolio.PostgreSQL;
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
    public class UsersController : ApiController
    {

        // Get: api/Users/search
        [AcceptVerbs("GET")]
        public async Task<UserSearchResponseModel> SearchUsers([FromUri] string portfolio, [FromUri] string term)
        {
            var provider = new UsersProvider();
            var result = await provider.GetUsersAsync(term);
            var response = PortfolioMapper.ActiveDirectoryMapper.Map<UserSearchResponseModel>(result);
            return response;
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
                        AccessGroup = user.AccessGroup.Name 
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
                        AccessGroup = user.AccessGroup.Name 
                    };
                }
            }
            return result;
        }

    }
}
