using FSAPortfolio.Entites.Projects;
using FSAPortfolio.WebAPI.App;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class ProjectController : ApiController
    {
        // GET: api/Project/Current
        public async Task<IEnumerable<latest_projects>> GetCurrent()
        {
            IEnumerable<latest_projects> result = null;
            using (var context = ContextFactory.NewPortfolioContext())
            {
                result = await (from p in context.latest_projects
                                where p.phase != "completed"
                                orderby p.priority_main descending, p.project_name
                                select p)
                                .ToListAsync();
            }
            return result;
        }

        // POST: api/Project
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Project/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Project/5
        public void Delete(int id)
        {
        }
    }
}
