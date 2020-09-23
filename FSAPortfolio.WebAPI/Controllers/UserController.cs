using FSAPortfolio.Entites;
using FSAPortfolio.PostgreSQL;
using FSAPortfolio.WebAPI.App;
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
    public class UserController : ApiController
    {

        // POST: api/ADUser
        [AcceptVerbs("POST")]
        public UserModel GetADUser(UserRequestModel userRequest)
        {
            UserModel result = null;
            using (var context = ContextFactory.NewPortfolioContext())
            {
                var user = context.users.FirstOrDefault(u => u.username == userRequest.UserName);
                if (user != null)
                {
                    result = new UserModel() { UserName = user.username, AccessGroup = user.access_group };
                }
            }
            return result;
        }

        // POST: api/User
        [AcceptVerbs("POST")]
        public UserModel GetUser(UserRequestModel userRequest)
        {
            UserModel result = null;
            using (var context = ContextFactory.NewPortfolioContext())
            {
                var user = context.users.FirstOrDefault(u => u.username == userRequest.UserName && u.pass_hash == userRequest.PasswordHash);
                if (user != null)
                {
                    result = new UserModel() { UserName = user.username, AccessGroup = user.access_group };
                }
            }
            return result;
        }

    }
}
