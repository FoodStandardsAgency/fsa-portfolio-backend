using FSAPortfolio.Entites;
using FSAPortfolio.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class MigrateController : ApiController
    {
        // GET: api/Migrate
        public string Get()
        {
            //using (var fromContext = new MigratePortfolioContext())
            //{
            //    var users = fromContext.users.ToArray();
            //}

            return ConfigurationManager.ConnectionStrings["MigratePortfolioContext"].ConnectionString;
        }

   
    }
}
