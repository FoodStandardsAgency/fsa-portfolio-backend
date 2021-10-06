using FSAPortfolio.Application.Models;
using FSAPortfolio.Application.Services.Index;
using FSAPortfolio.Application.Services.Projects;
using FSAPortfolio.Common.Logging;
using FSAPortfolio.Entities;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.DTO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
    {
        private readonly IPortfolioService portfolioService;
        private readonly IProjectDataService projectDataService;
        private readonly IIndexService indexService;

        public ProjectsController(IPortfolioService provider, IProjectDataService projectDataService, IIndexService indexService)
        {
            this.portfolioService = provider;
            this.projectDataService = projectDataService;
            this.indexService = indexService;

#if DEBUG
            AppLog.TraceVerbose($"{nameof(ProjectsController)} created.");
#endif

        }

        // POST: api/Projects
        [HttpPost, Route("api/Projects")]
        public async Task Post([FromBody] ProjectUpdateModel update)
        {
            await projectDataService.UpdateProjectAsync(update);
            await indexService.ReindexProjectAsync(update.project_id);
        }



        // Get: api/Projects
        /// <summary>
        /// This is used by front end for AJAX project search drop downs 
        /// </summary>
        /// <param name="term">Either the project ID or part of name</param>
        /// <param name="includeNone">Include a none option (not used)</param>
        /// <returns></returns>
        [HttpGet, Route("api/Projects/search")]
        [Authorize]
        public async Task<IEnumerable<SelectItemModel>> SearchProjects([FromUri] string term, [FromUri(Name = "addnone")] bool includeNone = false)
        {
            using (var context = new PortfolioContext())
            {
                // TODO: how to assert permissions here? Projects could be in different portfolios!
                IEnumerable<SelectItemModel> result = null;
                var filteredQuery = from p in context.Projects.Include(p => p.Reservation) 
                                    where p.Reservation.ProjectId.Contains(term) || p.Name.Contains(term)
                                    select p;
                result = PortfolioMapper.ProjectMapper.Map<IEnumerable<SelectItemModel>>(await filteredQuery.OrderByDescending(p => p.Priority).Take(10).ToArrayAsync());
                return result;
            }
        }

        /// <param name="portfolio"></param>
        /// <returns></returns>
        [HttpGet, Route("api/Projects")]
        [OverrideAuthorization]
        public async Task<ProjectCollectionModel> GetProjects([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            return await projectDataService.GetProjectDataAsync(portfolio, id);
        }

        [HttpGet, Route("api/Projects/updates")]
        [OverrideAuthorization]
        public async Task<ProjectUpdateCollectionModel> GetProjectUpdates([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            return await projectDataService.GetProjectUpdateDataAsync(portfolio, id);
        }

        [HttpGet, Route("api/Projects/changes")]
        [OverrideAuthorization]
        public async Task<ProjectChangeCollectionModel> GetProjectChanges([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            return await projectDataService.GetProjectChangeDataAsync(portfolio, id);
        }



        /// <summary>
        /// Gets a new project with any default settings and reserves a project_id.
        /// Gets the label configuration for the view, with options set for any view components such as drop down lists.
        /// </summary>
        /// <param name="portfolio">The portfolio to create the new project for</param>
        /// <returns>A DTO with the label config, options and default project data.</returns>
        /// <remarks>Labels must have the Create flag set in order to be included in the config data.</remarks>
        [HttpGet, Route("api/Projects/{portfolio}/newproject")]
        public async Task<GetProjectDTO<ProjectEditViewModel>> GetNewProject([FromUri] string portfolio)
        {
            return await projectDataService.CreateNewProjectAsync(portfolio);
        }

        [HttpGet, Route("api/Projects/{projectId}")]
        public async Task<GetProjectDTO<ProjectViewModel>> Get([FromUri] string projectId,
                                                               [FromUri] bool includeOptions = false,
                                                               [FromUri] bool includeHistory = false,
                                                               [FromUri] bool includeLastUpdate = false,
                                                               [FromUri] bool includeConfig = false)
        {
            AppLog.TraceVerbose(Request.RequestUri.PathAndQuery);

            var project = await projectDataService.GetProjectAsync(projectId, includeOptions, includeHistory, includeLastUpdate, includeConfig);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return project;
        }

        [HttpGet, Route("api/Projects/{projectId}/edit")]
        public async Task<GetProjectDTO<ProjectEditViewModel>> GetForEdit([FromUri] string projectId)
        {
            var project = await projectDataService.GetProjectForEdit(projectId);
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return project;
        }


        [HttpDelete, Route("api/Projects/{projectId}")]
        public async Task<IHttpActionResult> DeleteProject([FromUri] string projectId)
        {
            var project = await projectDataService.DeleteProjectAsync(projectId);
            await indexService.DeleteProjectAsync(projectId);
            if (project == null) return NotFound();
            else return Ok();
        }

    }
}
