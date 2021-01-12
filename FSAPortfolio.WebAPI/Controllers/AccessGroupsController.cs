using FSAPortfolio.Entities;
using FSAPortfolio.WebAPI.App.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class AccessGroupsController : ApiController
    {
        // GET: AccessGroup
        [AcceptVerbs("GET")]
        public async Task Init()
        {
            using (var context = new PortfolioContext())
            {
                var users = new UserProvider(context);
                await users.SeedAccessGroups();
            }
        }
    }
}