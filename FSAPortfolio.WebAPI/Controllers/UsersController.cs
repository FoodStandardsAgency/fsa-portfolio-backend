using FSAPortfolio.Entities;
using FSAPortfolio.PostgreSQL;
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

        // POST: api/ADUsers
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

        // POST: api/Users
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
