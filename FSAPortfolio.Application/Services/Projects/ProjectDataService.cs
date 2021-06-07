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

namespace FSAPortfolio.Application.Services.Projects
{
    public class ProjectDataService : BaseService, IProjectDataService
    {
        private IPortfolioService portfolioService;
        public ProjectDataService(IServiceContext context, IPortfolioService portfolioService) : base(context)
        {
            this.portfolioService = portfolioService;
        }

        public async Task<ProjectCollectionModel> GetProjectDataAsync(string portfolio)
        {
            List<int> reservationIds = await getReservationIdsForPortfolio(portfolio);
            var projectData = await getProjectsAsArrayAsync(reservationIds);
            var projectModel = PortfolioMapper.ExportMapper.Map<IEnumerable<ProjectExportModel>>(projectData);

            return new ProjectCollectionModel() { Projects = projectModel };
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

        private async Task<Project[]> getProjectsAsArrayAsync(List<int> reservationIds)
        {
            var projectQuery = from p in ServiceContext.PortfolioContext.Projects.IncludeProject()
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


    }
}
