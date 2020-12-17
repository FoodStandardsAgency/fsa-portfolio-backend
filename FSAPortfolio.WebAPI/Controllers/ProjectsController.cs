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

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class ProjectsController : ApiController
    {
        // POST: api/Projects
        [HttpPost]
        public async Task Post([FromBody] ProjectUpdateModel update)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    // Load and map the project
                    var provider = new ProjectProvider(context, update.project_id);
                    var userProvider = new UsersProvider(context);
                    var reservation = await provider.GetProjectReservationAsync();
                    if (reservation == null) throw new HttpResponseException(HttpStatusCode.NotFound);
                    else
                    {
                        var project = reservation?.Project;
                        if (project == null)
                        {
                            project = provider.CreateNewProject(reservation);
                        }

                        // Map the model to the project - map the leads manually because they can require an async AD lookup
                        PortfolioMapper.ProjectMapper.Map(update, project, opt =>
                        {
                            opt.Items[nameof(PortfolioContext)] = context;
                        });
                        await userProvider.MapPeopleAsync(update, project);

                        // Audit and save
                        if (project.AuditLogs != null) provider.LogAuditChanges(project);
                        await context.SaveChangesAsync();

                        // Get the last update and create a new one if necessary
                        ProjectUpdateItem lastUpdate = project.LatestUpdate;
                        ProjectUpdateItem projectUpdate = lastUpdate;
                        if (projectUpdate == null || projectUpdate.Timestamp.Date != DateTime.Today)
                        {
                            // Create a new update
                            projectUpdate = new ProjectUpdateItem() { Project = project };
                            if(project.FirstUpdate_Id == null)
                            {
                                project.FirstUpdate = projectUpdate;
                            }
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
            catch(AutoMapperMappingException ame)
            {
                if (ame.InnerException is FSAPortfolio.WebAPI.App.Config.PortfolioConfigurationException)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.Forbidden)
                    {
                        ReasonPhrase = ame.InnerException.Message
                    };
                    throw new HttpResponseException(resp);
                }
                else throw ame;
            }
        }

        // Get: api/Projects
        [AcceptVerbs("GET")]
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
            try
            {
                using (var context = new PortfolioContext())
                {
                    var provider = new PortfolioProvider(context, portfolio);
                    var config = await provider.GetConfigAsync();
                    this.AssertPermission(config.Portfolio);
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


        [HttpGet, Route("api/Projects/{projectId}")]
        public async Task<GetProjectDTO<ProjectViewModel>> Get([FromUri] string projectId,
                                                               [FromUri] bool includeOptions = false,
                                                               [FromUri] bool includeHistory = false,
                                                               [FromUri] bool includeLastUpdate = false,
                                                               [FromUri] bool includeConfig = false)
        {
            return await GetProject<ProjectViewModel>(projectId, includeOptions, includeHistory, includeLastUpdate, includeConfig);
        }

        [HttpGet, Route("api/Projects/{projectId}/edit")]
        public async Task<GetProjectDTO<ProjectEditViewModel>> GetForEdit([FromUri] string projectId)
        {
            return await GetProject<ProjectEditViewModel>(projectId,
                                                          includeOptions: true,
                                                          includeHistory: false,
                                                          includeLastUpdate: true,
                                                          includeConfig: true,
                                                          flags: PortfolioFieldFlags.Update);
        }

        [HttpDelete, Route("api/Projects/{projectId}")]
        public async Task<IHttpActionResult> DeleteProject([FromUri] string projectId)
        {
            using(var context = new PortfolioContext())
            {
                var project = await (from p in context.Projects.IncludeProjectForDelete() 
                                     where p.Reservation.ProjectId == projectId 
                                     select p).SingleOrDefaultAsync();
                if (project == null) return NotFound();
                this.AssertPermission(project.Reservation.Portfolio);

                project.DeleteCollections(context);
                await context.SaveChangesAsync();

                project.Reservation.Portfolio.Projects.Remove(project);
                context.ProjectReservations.Remove(project.Reservation);
                context.Projects.Remove(project);
                await context.SaveChangesAsync();
                return Ok();
            }
        }

        private async Task<GetProjectDTO<T>> GetProject<T>(string projectId,
                                                                  bool includeOptions,
                                                                  bool includeHistory,
                                                                  bool includeLastUpdate,
                                                                  bool includeConfig,
                                                                  PortfolioFieldFlags flags = PortfolioFieldFlags.Read)
            where T : ProjectModel, new()
        {
            string portfolio;
            GetProjectDTO<T> result;
            using (var context = new PortfolioContext())
            {
                var reservation = await context.ProjectReservations
                    .SingleOrDefaultAsync(r => r.ProjectId == projectId);

                if(reservation == null) throw new HttpResponseException(HttpStatusCode.NotFound);

                var query = (from p in context.Projects
                             .IncludeProject()
                             .IncludeLabelConfigs() // Need label configs so can map project data fields
                             where p.ProjectReservation_Id == reservation.Id
                             select p);
                if (includeHistory || includeLastUpdate) query = query.IncludeUpdates();

                var project = await query.SingleOrDefaultAsync();
                if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);
                this.AssertPermission(project.Reservation.Portfolio);

                portfolio = project.Reservation.Portfolio.ViewKey;

                // Build the result
                result = new GetProjectDTO<T>()
                {
                    Project = PortfolioMapper.ProjectMapper.Map<T>(project, opt =>
                    {
                        opt.Items[nameof(ProjectViewModel.UpdateHistory)] = includeHistory;
                        opt.Items[nameof(ProjectEditViewModel.LastUpdate)] = includeLastUpdate;
                    })
                };
                if (includeConfig)
                {
                    var userIsFSA = this.UserHasFSAClaim();
                    result.Config = PortfolioMapper.GetProjectLabelConfigModel(project.Reservation.Portfolio.Configuration, flags: flags, fsaOnly: !userIsFSA);
                }
                if (includeOptions)
                {
                    var provider = new PortfolioProvider(context, portfolio);
                    var config = await provider.GetConfigAsync();
                    result.Options = await provider.GetNewProjectOptionsAsync(config, result.Project as ProjectEditViewModel);
                }

            }

            return result;
        }
    }
}
