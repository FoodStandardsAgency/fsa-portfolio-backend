using FSAPortfolio.Entites;
using FSAPortfolio.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class MigrateController : ApiController
    {
        // GET: api/Migrate
        public IEnumerable<string> Get()
        {
            using(var toContext = new PortfolioContext())
            using (var fromContext = new MigratePortfolioContext())
            {
                var users = fromContext.users.ToArray();
            }

            return new string[] { "value1", "value2" };
        }

   
    }
}
