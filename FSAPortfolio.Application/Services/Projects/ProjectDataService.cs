using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.Models;
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

        public async Task<GetProjectExportDTO> GetProjectExportDTOAsync(string viewKey)
        {
            var context = ServiceContext.PortfolioContext;
            List<int> reservationIds = await getReservationIdsForPortfolio(viewKey);

            var projectQuery = from p in context.Projects.IncludeProject()
                               where reservationIds.Contains(p.ProjectReservation_Id)
                               select p;

            var config = await portfolioService.GetConfigAsync(viewKey);
            var projects = await projectQuery.OrderByDescending(p => p.Priority).ToArrayAsync();
            GetProjectExportDTO result = new GetProjectExportDTO()
            {
                Config = PortfolioMapper.GetProjectLabelConfigModel(config, includedOnly: true),
                Projects = PortfolioMapper.ExportMapper.Map<IEnumerable<ProjectExportModel>>(projects)
            };
            return result;
        }

        private async Task<List<int>> getReservationIdsForPortfolio(string viewKey)
        {
            return await (
                from p in ServiceContext.PortfolioContext.Projects
                where p.Reservation.Portfolio.ViewKey == viewKey
                select p.ProjectReservation_Id
                ).ToListAsync();
        }


    }
}
