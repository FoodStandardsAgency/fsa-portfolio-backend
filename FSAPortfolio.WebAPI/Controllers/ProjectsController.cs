using FSAPortfolio.Entities;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.PostgreSQL.Projects;
using System.Text;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.App.Projects;
using FSAPortfolio.WebAPI.App.Mapping.Projects;
using FSAPortfolio.Entities.Organisation;
using System.Linq.Expressions;
using AutoMapper;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.Application.Services.Projects;
using FSAPortfolio.Common.Logging;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
    {
        private readonly IPortfolioService portfolioService;
        private readonly IProjectDataService projectDataService;

        public ProjectsController(IPortfolioService provider, IProjectDataService projectDataService)
        {
            this.portfolioService = provider;
            this.projectDataService = projectDataService;

#if DEBUG
            AppLog.TraceVerbose($"{nameof(ProjectsController)} created.");
#endif

        }

        // POST: api/Projects
        [HttpPost, Route("api/Projects")]
        public async Task Post([FromBody] ProjectUpdateModel update)
        {
            await projectDataService.UpdateProjectAsync(update);
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

        //[HttpGet, Route("api/Projects")]
        //public async Task<ProjectCollectionModel> GetProjects([FromUri] string portfolio)
        //{
        //    return await projectDataService.GetProjectDataAsync(portfolio);
        //}

        [HttpGet, Route("api/Projects/updates")]
        public async Task<ProjectUpdateCollectionModel> GetProjectUpdates([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            return await projectDataService.GetProjectUpdateDataAsync(portfolio, id);
        }

        [HttpGet, Route("api/Projects/changes")]
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
            return await projectDataService.GetProjectAsync(projectId, includeOptions, includeHistory, includeLastUpdate, includeConfig);
        }

        [HttpGet, Route("api/Projects/{projectId}/edit")]
        public async Task<GetProjectDTO<ProjectEditViewModel>> GetForEdit([FromUri] string projectId)
        {
            return await projectDataService.GetProjectForEdit(projectId);
        }


        [HttpDelete, Route("api/Projects/{projectId}")]
        public async Task<IHttpActionResult> DeleteProject([FromUri] string projectId)
        {
            var project = await projectDataService.DeleteProjectAsync(projectId);
            if (project == null) return NotFound();
            else return Ok();
        }

    }
}
