using FSAPortfolio.PostgreSQL;
using FSAPortfolio.WebAPI.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class MigrateController : ApiController
    {
        // GET: api/Migrate
        public string Get()
        {
            using (var fromContext = ContextFactory.NewPostgresContext())
            {
                var users = fromContext.users.ToArray();
            }

            return "Success!";
        }


    }
}
