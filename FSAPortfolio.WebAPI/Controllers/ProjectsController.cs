using FSAPortfolio.Entities;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.PostgreSQL.Projects;
using System.Text;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class ProjectsController : ApiController
    {
        // POST: api/Projects
        [HttpPost]
        public async Task Post([FromBody]project update)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    // Load and map the project
                    var project = await ProjectWithIncludes(context).SingleAsync(p => p.ProjectId == update.project_id);
                    PortfolioMapper.Mapper.Map(update, project, opt => opt.Items["portfolioContext"] = context);
                    LogChanges(context, project);

                    // Create a new update
                    var projectUpdate = new ProjectUpdateItem();
                    PortfolioMapper.Mapper.Map(update, projectUpdate, opt => opt.Items["portfolioContext"] = context);
                    projectUpdate.Timestamp = DateTime.Now;
                    project.Updates.Add(projectUpdate);
                    project.LatestUpdate = projectUpdate;

                    // Save
                    await context.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        // GET: api/Projects?portfolio={portfolio}&filter={filter}
        [HttpGet]
        public async Task<IEnumerable<latest_projects>> Get([FromUri]string portfolio, [FromUri] string filter)
        {
            try
            {
                IEnumerable<latest_projects> result = null;
                var directorate = DirectorateContext.Current;
                using (var context = new PortfolioContext())
                {
                    IQueryable<Project> query = ProjectWithIncludes(context, portfolio);
                    switch (filter)
                    {
                        case "new":
                            var newCutoff = DateTime.Now.AddDays(-PortfolioSettings.NewProjectLimitDays);
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id != directorate.CompletedPhase.Id && p.FirstUpdate.Timestamp > newCutoff
                                    orderby p.Priority descending, p.Name
                                    select p;
                            break;
                        case "complete":
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id == directorate.CompletedPhase.Id
                                    orderby p.LatestUpdate.Timestamp descending, p.Name
                                    select p;
                            break;
                        case "current":
                        case "latest":
                        default:
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id != directorate.CompletedPhase.Id
                                    orderby p.Priority descending, p.Name
                                    select p;
                            break;
                    }
                    var projects = await query.ToListAsync();
                    result = PortfolioMapper.Mapper.Map<IEnumerable<latest_projects>>(projects);
                }
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public async Task<latest_projects> Get(string projectId)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var project = (from p in ProjectWithIncludes(context) where p.ProjectId == projectId select p).Single();
                    var result = PortfolioMapper.Mapper.Map<latest_projects>(project);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private static IQueryable<Project> ProjectWithIncludes(PortfolioContext context, string portfolio)
        {
            return ProjectWithIncludes(context).Where(p => p.Portfolios.Any(po => po.Route == portfolio));
        }

        private static IQueryable<Project> ProjectWithIncludes(PortfolioContext context)
        {
            return context.Projects
                .Include(p => p.Category)
                .Include(p => p.FirstUpdate.OnHoldStatus)
                .Include(p => p.FirstUpdate.RAGStatus)
                .Include(p => p.FirstUpdate.Phase)
                .Include(p => p.LatestUpdate.OnHoldStatus)
                .Include(p => p.LatestUpdate.RAGStatus)
                .Include(p => p.LatestUpdate.Phase)
                .Include(p => p.Category)
                .Include(p => p.Size)
                .Include(p => p.BudgetType)
                .Include(p => p.RelatedProjects)
                .Include(p => p.Lead);
        }

        private static void LogChanges(PortfolioContext context, Project project)
        {
            var changes = context.ChangeTracker.Entries().Where(c => c.State == EntityState.Modified);
            if (changes.Count() > 0)
            {
                var log = new ProjectAuditLog();
                var logText = new StringBuilder();
                foreach (var change in changes)
                {
                    var originalValues = change.OriginalValues;
                    var currentValues = change.CurrentValues;
                    foreach (string pname in originalValues.PropertyNames)
                    {
                        var originalValue = originalValues[pname];
                        var currentValue = currentValues[pname];
                        if (!Equals(originalValue, currentValue))
                        {
                            logText.Append($"{pname}: [{originalValue}] to [{currentValue}];");
                        }
                    }
                }
                log.Text = logText.ToString();
                project.AuditLogs.Add(log);
            }
        }

    }
}
