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
        public async Task Post([FromBody]ProjectModel update)
        {
            try
            {
                using (var provider = new ProjectProvider(update.project_id))
                {
                    // Load and map the project
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
                            opt.Items[nameof(PortfolioContext)] = provider.Context;
                        });
                        if (project.AuditLogs != null) provider.LogAuditChanges(project);
                        await provider.SaveChangesAsync();

                        // Get the last update and create a new one if necessary
                        ProjectUpdateItem lastUpdate = project.LatestUpdate;
                        ProjectUpdateItem projectUpdate = lastUpdate;
                        if (projectUpdate == null || projectUpdate.Timestamp.Date != DateTime.Today)
                        {
                            // Create a new update
                            projectUpdate = new ProjectUpdateItem() { Project = project };
                        }

                        // Map the data to the update and add if not a duplicate
                        PortfolioMapper.ProjectMapper.Map(update, projectUpdate, opt => opt.Items[nameof(PortfolioContext)] = provider.Context);
                        if (!projectUpdate.IsDuplicate(lastUpdate))
                        {
                            project.Updates.Add(projectUpdate);
                            project.LatestUpdate = projectUpdate;
                            project.LatestUpdate.Timestamp = DateTime.Now;
                        }

                        // Save
                        await provider.SaveChangesAsync();
                    }

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
        public async Task<GetProjectDTO> GetNewProject([FromUri] string portfolio)
        {
            try
            {
                using (var provider = new PortfolioProvider(portfolio))
                {
                    var config = await provider.GetConfigAsync();
                    var reservation = await provider.GetProjectReservationAsync(config);
                    await provider.SaveChangesAsync();

                    var result = new GetProjectDTO()
                    {
                        Config = PortfolioMapper.GetProjectLabelConfigModel(config, PortfolioFieldFlags.Create),
                        Options = await provider.GetNewProjectOptionsAsync(config),
                        Project = new ProjectModel() { project_id = reservation.ProjectId }
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
        public async Task<GetProjectDTO> Get([FromUri] string projectId, [FromUri] bool includeOptions = false)
        {
            string portfolio;
            GetProjectDTO result;
            using (var context = new PortfolioContext())
            {
                var query = (from p in context.Projects.ProjectIncludes().ViewConfigIncludes()
                             where p.Reservation.ProjectId == projectId
                             select p);
                var project = query.Single();
                result = new GetProjectDTO()
                {
                    Config = PortfolioMapper.GetProjectLabelConfigModel(project.Reservation.Portfolio.Configuration),
                    Project = PortfolioMapper.ProjectMapper.Map<ProjectModel>(project)
                };
                portfolio = project.Reservation.Portfolio.ViewKey;
            }

            if (includeOptions)
            {
                using (var provider = new PortfolioProvider(portfolio))
                {
                    var config = await provider.GetConfigAsync();
                    result.Options = await provider.GetNewProjectOptionsAsync(config);
                }
            }
            return result;
        }

        [HttpGet]
        public Task<IEnumerable<ProjectModel>> GetFiltered(string projectId, [FromUri] string filter)
        {
            switch(filter)
            {
                case "related":
                    return GetRelatedProjects(projectId);
                case "dependent":
                    return GetDependantProjects(projectId);
                default:
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> GetRelatedProjects(string projectId)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var projects = await context.Projects.ProjectIncludes()
                        .Where(p => p.Reservation.ProjectId == projectId)
                        .SelectMany(p => p.RelatedProjects)
                        .ToListAsync();
                    var result = PortfolioMapper.ProjectMapper.Map<IEnumerable<ProjectModel>>(projects);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> GetDependantProjects(string projectId)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var projects = await context.Projects.ProjectIncludes()
                        .Where(p => p.Reservation.ProjectId == projectId)
                        .SelectMany(p => p.DependantProjects)
                        .ToListAsync(); ;
                    var result = PortfolioMapper.ProjectMapper.Map<IEnumerable<ProjectModel>>(projects);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets text for non-empty updates with timestamps and dates.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ProjectUpdateModel>> GetUpdates(string projectId)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var project = await (from u in context.Projects
                                         .Include(p => p.Updates)
                                         .Include(p => p.LatestUpdate)
                                         where u.Reservation.ProjectId == projectId
                                         select u)
                                   .SingleAsync();
                    var updates = (from u in project.Updates
                                   where !string.IsNullOrEmpty(u.Text)
                                   orderby u.Timestamp descending
                                   select u).ToList();
                    var result = PortfolioMapper.ProjectMapper.Map<IEnumerable<ProjectUpdateModel>>(updates);
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
            return context.Projects.ProjectIncludes().Where(p => p.Portfolios.Any(po => po.ViewKey == portfolio));
        }

    }
}
