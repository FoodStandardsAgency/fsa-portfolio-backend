using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSAPortfolio.Entities.Organisation;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using FSAPortfolio.WebAPI.App.Projects;
using FSAPortfolio.WebAPI.App.Users;
using AutoMapper;
using FSAPortfolio.Common.Logging;
using FSAPortfolio.WebAPI.App.Mapping.ImportExport;

namespace FSAPortfolio.Application.Services.Projects
{
    public class ProjectDataService : BaseService, IProjectDataService
    {
        private IPortfolioService portfolioService;
        private IProjectService projectService;
        public ProjectDataService(IServiceContext context, IPortfolioService portfolioService, IProjectService projectService) : base(context)
        {
            this.portfolioService = portfolioService;
            this.projectService = projectService;
        }

        public async Task<ProjectCollectionModel> GetProjectDataAsync(string portfolio, string[] projectIds)
        {
            List<int> reservationIds = await getReservationIdsForPortfolio(portfolio, projectIds);
            var projectData = await getProjectsAsArrayWitUpdatesAsync(reservationIds);
            var projectModel = PortfolioMapper.ExportMapper.Map<IEnumerable<ProjectExportModel>>(projectData, 
                opts => {
                    opts.Items[ExportUpdateTextResolver.OnKey] = true;
                });

            return new ProjectCollectionModel() { Projects = projectModel };
        }

        public async Task<GetProjectDTO<ProjectEditViewModel>> GetProjectForEdit(string projectId)
        {
            return await GetProject<ProjectEditViewModel>(projectId,
                                                          includeOptions: true,
                                                          includeHistory: false,
                                                          includeLastUpdate: true,
                                                          includeConfig: true,
                                                          flags: PortfolioFieldFlags.Update,
                                                          ServiceContext.AssertEditor);
        }

        public async Task<GetProjectDTO<ProjectViewModel>> GetProjectAsync(string projectId,
                                                               bool includeOptions,
                                                               bool includeHistory,
                                                               bool includeLastUpdate,
                                                               bool includeConfig)
        {
            return await GetProject<ProjectViewModel>(projectId, includeOptions, includeHistory, includeLastUpdate, includeConfig);
        }

        public async Task<ProjectUpdateCollectionModel> GetProjectUpdateDataAsync(string portfolio, string[] projectIds)
        {
            ProjectUpdateItem[] projectData = await getProjectUpdatesAsArrayAsync(portfolio, projectIds);
            var updates = PortfolioMapper.ExportMapper.Map<ProjectUpdateCollectionModel>(projectData);
            return updates;
        }

        public async Task<ProjectChangeCollectionModel> GetProjectChangeDataAsync(string portfolio, string[] projectIds)
        {
            ProjectUpdateItem[] projectData = await getProjectUpdatesAsArrayAsync(portfolio, projectIds);
            var updates = PortfolioMapper.ExportMapper.Map<ProjectChangePrecursorCollection>(projectData);
            var changes = PortfolioMapper.ExportMapper.Map<ProjectChangeCollectionModel>(updates);
            return changes;
        }

        public async Task<GetProjectExportDTO> GetProjectExportDTOAsync(string viewKey)
        {
            // Get the data
            List<int> reservationIds = await getReservationIdsForPortfolio(viewKey);
            var projects = await getProjectsAsArrayAsync(reservationIds);
            var config = await portfolioService.GetConfigAsync(viewKey);

            // To the mapping
            GetProjectExportDTO result = new GetProjectExportDTO()
            {
                Config = PortfolioMapper.GetProjectLabelConfigModel(config, includedOnly: true),
                Projects = PortfolioMapper.ExportMapper.Map<IEnumerable<ProjectExportModel>>(projects)
            };
            return result;
        }

        public async Task<GetProjectDTO<ProjectEditViewModel>> CreateNewProjectAsync(string portfolio)
        {
            var context = ServiceContext.PortfolioContext;
            var config = await portfolioService.GetConfigAsync(portfolio);
            ServiceContext.AssertPermission(config.Portfolio);
            var reservation = await portfolioService.GetProjectReservationAsync(config);
            await context.SaveChangesAsync();

            var newProject = new Project() { Reservation = reservation };

            ProjectEditViewModel newProjectModel = ProjectModelFactory.GetProjectEditModel(newProject);

            var result = new GetProjectDTO<ProjectEditViewModel>()
            {
                Config = PortfolioMapper.GetProjectLabelConfigModel(config, PortfolioFieldFlags.Create),
                Options = await portfolioService.GetNewProjectOptionsAsync(config),
                Project = newProjectModel
            };
            return result;
        }

        public async Task<Project> DeleteProjectAsync(string projectId)
        {
            var logMessage = new StringBuilder();
            logMessage.AppendLine($"Beginning delete of {projectId}:");
            try
            {
                var context = ServiceContext.PortfolioContext;
                var project = await (from p in context.Projects.IncludeProjectForDelete()
                                     where p.Reservation.ProjectId == projectId
                                     select p).SingleOrDefaultAsync();
                if (project != null)
                {
                    ServiceContext.AssertAdmin(project.Reservation.Portfolio);

                    logMessage.AppendLine("    - Deleting collections");
                    project.DeleteCollections(context);
                    await context.SaveChangesAsync();

                    logMessage.AppendLine("    - Deleting project");
                    project.Reservation.Portfolio.Projects.Remove(project);
                    context.ProjectReservations.Remove(project.Reservation);
                    context.Projects.Remove(project);
                    await context.SaveChangesAsync();
                }
                return project;
            }
            finally {
                AppLog.TraceWarning(logMessage.ToString());
            }
        }

        public async Task ImportProjectsAsync(string viewKey, MultipartFormDataStreamProvider files)
        {
            var context = ServiceContext.PortfolioContext;

            // Get the config and options
            var config = await portfolioService.GetConfigAsync(viewKey);
            ServiceContext.AssertAdmin(config.Portfolio);
            var options = await portfolioService.GetNewProjectOptionsAsync(config);

            // Import the projects
            var importer = new PropertyImporter();
            var projects = await importer.ImportProjectsAsync(files, config, options);

            // Update/create the projects
            foreach (var project in projects)
            {
                if (string.IsNullOrWhiteSpace(project.project_id))
                {
                    // Create a reservation
                    var reservation = await portfolioService.GetProjectReservationAsync(config);
                    project.project_id = reservation.ProjectId;
                    await projectService.UpdateProject(project, reservation);
                }
                else
                {
                    await projectService.UpdateProject(project);
                }
            }
        }

        public async Task UpdateProjectAsync(ProjectUpdateModel update)
        {
            try
            {
                var context = ServiceContext.PortfolioContext;
                // Load and map the project
                await projectService.UpdateProject(update, permissionCallback: ServiceContext.AssertEditor);
            }
            catch (AutoMapperMappingException amex)
            {
                if (amex.InnerException is ProjectDataValidationException)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = amex.InnerException.Message
                    };
                    throw new HttpResponseException(resp);
                }
                else
                {
                    throw amex;
                }
            }
        }



        private async Task<Project[]> getProjectsAsArrayAsync(List<int> reservationIds)
        {
            var projectQuery = from p in ServiceContext.PortfolioContext.Projects
                                .IncludeProject()
                               where reservationIds.Contains(p.ProjectReservation_Id)
                               select p;
            var projects = await projectQuery.OrderByDescending(p => p.Priority).ToArrayAsync();
            return projects;
        }
        private async Task<Project[]> getProjectsAsArrayWitUpdatesAsync(List<int> reservationIds)
        {
            var projectQuery = from p in ServiceContext.PortfolioContext.Projects
                                .IncludeProject()
                                .Include(pr => pr.Updates)
                               where reservationIds.Contains(p.ProjectReservation_Id)
                               select p;
            var projects = await projectQuery.OrderByDescending(p => p.Priority).ToArrayAsync();
            return projects;
        }

        private async Task<ProjectUpdateItem[]> getProjectUpdatesAsArrayAsync(string portfolio, string[] projectIds)
        {
            List<int> reservationIds = await getReservationIdsForPortfolio(portfolio, projectIds);
            var projectQuery = from u in ServiceContext.PortfolioContext.ProjectUpdates.IncludeUpdates()
                               where reservationIds.Contains(u.Project.ProjectReservation_Id)
                               select u;
            var updates = await projectQuery.OrderByDescending(p => p.Project.Reservation.ProjectId).ThenBy(u => u.Timestamp).ToArrayAsync();
            return updates;
        }

        private async Task<List<int>> getReservationIdsForPortfolio(string viewKey, string[] projectIds = null)
        {
            var query = from p in ServiceContext.PortfolioContext.Projects
                        where p.Reservation.Portfolio.ViewKey == viewKey
                        select p;

            if (projectIds != null && projectIds.Length > 0)
                query = query.Where(p => projectIds.Contains(p.Reservation.ProjectId));

            return await (from p in query select p.ProjectReservation_Id).ToListAsync();
        }

        private async Task<GetProjectDTO<T>> GetProject<T>(string projectId,
                                                                  bool includeOptions,
                                                                  bool includeHistory,
                                                                  bool includeLastUpdate,
                                                                  bool includeConfig,
                                                                  PortfolioFieldFlags flags = PortfolioFieldFlags.Read,
                                                                  Action<Portfolio> permissionCallback = null)
            where T : ProjectModel, new()
        {
            string portfolio;
            GetProjectDTO<T> result;


            var context = ServiceContext.PortfolioContext;
            var reservation = await context.ProjectReservations
                .SingleOrDefaultAsync(r => r.ProjectId == projectId);

            if (reservation == null) throw new HttpResponseException(HttpStatusCode.NotFound);

            var query = (from p in context.Projects
                         .IncludeProject()
                         .IncludeLabelConfigs() // Need label configs so can map project data fields
                         where p.ProjectReservation_Id == reservation.Id
                         select p);
            if (includeHistory || includeLastUpdate) query = query.IncludeUpdates();

            var project = await query.SingleOrDefaultAsync();
            if (project == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            if (permissionCallback != null)
                permissionCallback(project.Reservation.Portfolio);
            else
                ServiceContext.AssertPermission(project.Reservation.Portfolio);

            portfolio = project.Reservation.Portfolio.ViewKey;

            // Build the result
            result = new GetProjectDTO<T>()
            {
                Project = ProjectModelFactory.GetProjectModel<T>(project, includeHistory, includeLastUpdate)
            };

            if (includeConfig)
            {
                var userIsFSA = ServiceContext.UserHasFSAClaim();
                result.Config = PortfolioMapper.GetProjectLabelConfigModel(project.Reservation.Portfolio.Configuration, flags: flags, fsaOnly: !userIsFSA);
            }
            if (includeOptions)
            {
                var config = await portfolioService.GetConfigAsync(portfolio);
                result.Options = await portfolioService.GetNewProjectOptionsAsync(config, result.Project as ProjectEditViewModel);
            }

            return result;
        }


    }
}
