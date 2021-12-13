using FSAPortfolio.Entities;
using FSAPortfolio.Application.Services.Users;
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
        IUserService userService;
        public AccessGroupsController(IUserService userService)
        {
            this.userService = userService;
        }

        // GET: AccessGroup
        [AcceptVerbs("GET")]
        public async Task Init()
        {
            await userService.SeedAccessGroups();
        }
    }
}