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
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.App.Projects;
using FSAPortfolio.WebAPI.Mapping.Projects;
using FSAPortfolio.Entities.Organisation;
using System.Linq.Expressions;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class ProjectsController : ApiController
    {
        // POST: api/Projects
        [HttpPost]
        public async Task Post([FromBody] ProjectUpdateModel update)
        {
            using (var context = new PortfolioContext())
            {
                // Load and map the project
                var provider = new ProjectProvider(context, update.project_id);
                var reservation = await provider.GetProjectReservationAsync();
                if (reservation == null) throw new HttpResponseException(HttpStatusCode.NotFound);
                else
                {
                    var project = reservation?.Project;
                    if (project == null)
                    {
                        project = provider.CreateNewProject(reservation);
                    }
                    PortfolioMapper.ProjectMapper.Map(update, project, opt =>
                    {
                        opt.Items[nameof(PortfolioContext)] = context;
                    });
                    if (project.AuditLogs != null) provider.LogAuditChanges(project);
                    await context.SaveChangesAsync();

                    // Get the last update and create a new one if necessary
                    ProjectUpdateItem lastUpdate = project.LatestUpdate;
                    ProjectUpdateItem projectUpdate = lastUpdate;
                    if (projectUpdate == null || projectUpdate.Timestamp.Date != DateTime.Today)
                    {
                        // Create a new update
                        projectUpdate = new ProjectUpdateItem() { Project = project };
                    }

                    // Map the data to the update and add if not a duplicate
                    PortfolioMapper.ProjectMapper.Map(update, projectUpdate, opt => opt.Items[nameof(PortfolioContext)] = context);
                    if (!projectUpdate.IsDuplicate(lastUpdate))
                    {
                        project.Updates.Add(projectUpdate);
                        project.LatestUpdate = projectUpdate;
                        project.LatestUpdate.Timestamp = DateTime.Now;
                    }

                    // Save
                    await context.SaveChangesAsync();
                }

            }
        }


        // GET: api/Projects?portfolio={portfolio}&filter={filter}
        [HttpGet]
        public async Task<IEnumerable<latest_projects>> Get([FromUri]string portfolio, [FromUri] string filter)
        {
            try
            {
                IEnumerable<latest_projects> result = null;
                using (var context = new PortfolioContext())
                {
                    var config = await context.Portfolios
                        .Where(p => p.ViewKey == portfolio)
                        .Select(p => p.Configuration)
                        .Include(c => c.CompletedPhase)
                        .SingleAsync();

                    IQueryable<Project> query = ProjectWithIncludes(context, portfolio);
                    switch (filter)
                    {
                        case "new":
                            var newCutoff = DateTime.Now.AddDays(-PortfolioSettings.NewProjectLimitDays);
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id != config.CompletedPhase.Id && p.FirstUpdate.Timestamp > newCutoff
                                    orderby p.Priority descending, p.Name
                                    select p;
                            break;
                        case "complete":
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id == config.CompletedPhase.Id
                                    orderby p.LatestUpdate.Timestamp descending, p.Name
                                    select p;
                            break;
                        case "current":
                        case "latest":
                        default:
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id != config.CompletedPhase.Id
                                    orderby p.Priority descending, p.Name
                                    select p;
                            break;
                    }
                    var projects = await query.ToListAsync();
                    result = PortfolioMapper.ProjectMapper.Map<IEnumerable<latest_projects>>(projects);
                }
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets a new project with any default settings and reserves an project_id.
        /// Gets the label configuration for the view, with options set for any view components such as drop down lists.
        /// </summary>
        /// <param name="portfolio">The portfolio to create the new project for</param>
        /// <returns>A DTO with the label config, options and default project data.</returns>
        /// <remarks>Labels must have the Create flag set in order to be included in the config data.</remarks>
        [HttpGet]
        public async Task<GetProjectDTO<ProjectEditViewModel>> GetNewProject([FromUri] string portfolio)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var provider = new PortfolioProvider(context, portfolio);
                    var config = await provider.GetConfigAsync();
                    var reservation = await provider.GetProjectReservationAsync(config);
                    await context.SaveChangesAsync();

                    var result = new GetProjectDTO<ProjectEditViewModel>()
                    {
                        Config = PortfolioMapper.GetProjectLabelConfigModel(config, PortfolioFieldFlags.Create),
                        Options = await provider.GetNewProjectOptionsAsync(config),
                        Project = new ProjectEditViewModel() { project_id = reservation.ProjectId }
                    };
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        [HttpGet]
        public async Task<GetProjectDTO<ProjectViewModel>> Get([FromUri] string projectId, [FromUri] bool includeOptions = false, [FromUri] bool includeHistory = false, [FromUri] bool includeLastUpdate = false, [FromUri] bool includeConfig = false)
        {
            return await GetProject<ProjectViewModel>(projectId, includeOptions, includeHistory, includeLastUpdate, includeConfig);
        }
        [HttpGet]
        public async Task<GetProjectDTO<ProjectEditViewModel>> GetForEdit([FromUri] string projectId)
        {
            return await GetProject<ProjectEditViewModel>(projectId, includeOptions: true, includeHistory: false, includeLastUpdate: true, includeConfig: true);
        }

        private static async Task<GetProjectDTO<T>> GetProject<T>(string projectId, bool includeOptions, bool includeHistory, bool includeLastUpdate, bool includeConfig)
            where T : ProjectModel, new()
        {
            string portfolio;
            GetProjectDTO<T> result;
            using (var context = new PortfolioContext())
            {
                var query = (from p in context.Projects
                             .IncludeProject()
                             .IncludeLabelConfigs() // Need label configs so can map project data fields
                             where p.Reservation.ProjectId == projectId
                             select p);
                if (includeHistory || includeLastUpdate) query = query.IncludeUpdates();

                var project = await query.SingleOrDefaultAsync();
                if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);

                portfolio = project.Reservation.Portfolio.ViewKey;

                // Build the result
                result = new GetProjectDTO<T>()
                {
                    Project = PortfolioMapper.ProjectMapper.Map<T>(project, opt =>
                    {
                        opt.Items[nameof(ProjectViewModel.UpdateHistory)] = includeHistory;
                        opt.Items[nameof(ProjectViewModel.LastUpdate)] = includeLastUpdate;
                    })
                };
                if (includeConfig) result.Config = PortfolioMapper.GetProjectLabelConfigModel(project.Reservation.Portfolio.Configuration);
                if (includeOptions)
                {
                    var provider = new PortfolioProvider(context, portfolio);
                    var config = await provider.GetConfigAsync();
                    result.Options = await provider.GetNewProjectOptionsAsync(config);
                }

            }

            return result;
        }




        private static IQueryable<Project> ProjectWithIncludes(PortfolioContext context, string portfolio)
        {
            return context.Projects.IncludeProject().Where(p => p.Portfolios.Any(po => po.ViewKey == portfolio));
        }

    }
}
