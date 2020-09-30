using FSAPortfolio.PostgreSQL;
using FSAPortfolio.PostgreSQL.Projects;
using FSAPortfolio.WebAPI.Models;
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
    public class ProjectsController : ApiController
    {
        // GET: api/Projects/Current
        public async Task<IEnumerable<latest_projects>> GetCurrent()
        {
            IEnumerable<latest_projects> result = null;
            using (var context = new MigratePortfolioContext())
            {
                result = await (from p in context.latest_projects
                                where p.phase != "completed"
                                orderby p.priority_main descending, p.project_name
                                select p)
                                .ToListAsync();
            }
            return result;
        }

        // GET: api/Projects/New
        public async Task<IEnumerable<latest_projects>> GetNew()
        {
            IEnumerable<latest_projects> result = null;
            using (var context = new MigratePortfolioContext())
            {
                var cutoff = DateTime.Today - TimeSpan.FromDays(14);
                result = await (from p in context.latest_projects
                                where p.phase != "completed" && p.min_time > cutoff
                                orderby p.priority_main descending, p.project_name
                                select p)
                                .ToListAsync();
            }
            return result;
        }

        // GET: api/Projects/Completed
        public async Task<IEnumerable<latest_projects>> GetCompleted()
        {
            IEnumerable<latest_projects> result = null;
            using (var context = new MigratePortfolioContext())
            {
                result = await (from p in context.latest_projects
                                where p.phase == "completed"
                                orderby p.timestamp descending
                                select p)
                                .ToListAsync();
            }
            return result;
        }

        // GET: api/Projects/Latest
        public async Task<IEnumerable<latest_projects>> GetLatest()
        {
            IEnumerable<latest_projects> result = null;
            using (var context = new MigratePortfolioContext())
            {
                result = await (from p in context.latest_projects
                                orderby p.timestamp descending
                                select p)
                                .ToListAsync();
            }
            return result;
        }

        // GET: api/Projects/ODDLeads
        public async Task<IEnumerable<ODDLead>> GetODDLeads()
        {
            IEnumerable<ODDLead> result = null;
            using (var context = new MigratePortfolioContext())
            {
                result = await (from p in context.latest_projects
                                where p.phase != "completed"
                                orderby p.oddlead
                                select new ODDLead() { Name = p.oddlead })
                                .Distinct()
                                .ToListAsync();
            }
            return result;
        }

        // GET: api/Projects/UnmatchedODDLeads
        public async Task<IEnumerable<ODDLead>> GetUnmatchedODDLeads()
        {
            IEnumerable<ODDLead> result = null;
            using (var context = new MigratePortfolioContext())
            {
                result = await (from p in context.latest_projects
                                where p.g6team == null && p.oddlead != string.Empty
                                orderby p.oddlead_email
                                select new ODDLead() { Name = p.oddlead, Email = p.oddlead_email })
                                .Distinct()
                                .ToListAsync();
            }
            return result;
        }

    }
}
