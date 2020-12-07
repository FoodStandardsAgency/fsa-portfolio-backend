using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Projects;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class PortfoliosController : ApiController
    {
        [HttpGet]
        public async Task<IEnumerable<PortfolioModel>> Index()
        {
            IEnumerable<PortfolioModel> result = null;
            using (var context = new PortfolioContext())
            {
                result = PortfolioMapper.ConfigMapper.Map<IEnumerable<PortfolioModel>>(await context.Portfolios.ToListAsync());
            }
            return result;
        }

        [HttpGet]
        public async Task<PortfolioSummaryModel> Summary([FromUri(Name = "portfolio")] string viewKey, [FromUri(Name = "type")] string summaryType = PortfolioSummaryModel.ByCategory)
        {
            PortfolioSummaryModel result = null;
            using (var context = new PortfolioContext())
            {
                var portfolio = await context.Portfolios
                    .Include(p => p.Teams)
                    .IncludeConfig()
                    .IncludeProjects()
                    .SingleOrDefaultAsync(p => p.ViewKey == viewKey);

                if (portfolio == null) throw new HttpResponseException(HttpStatusCode.NotFound);

                result = PortfolioMapper.ConfigMapper.Map<PortfolioSummaryModel>(
                    portfolio, 
                    opt => {
                        opt.Items[nameof(PortfolioConfiguration)] = portfolio.Configuration;
                        opt.Items[nameof(PortfolioSummaryModel)] = summaryType;
                    });
            }
            return result;
        }

        [HttpGet]
        public async Task<GetProjectQueryDTO> FilterOptionsAsync([FromUri(Name = "portfolio")] string viewKey)
        {
            GetProjectQueryDTO result = null;
            using (var context = new PortfolioContext())
            {
                var provider = new PortfolioProvider(context, viewKey);
                var config = await provider.GetConfigAsync();
                var customFields = provider.GetCustomFilterLabels(config);

                result = new GetProjectQueryDTO()
                {
                    Config = PortfolioMapper.GetProjectLabelConfigModel(config, PortfolioFieldFlags.FilterProject|PortfolioFieldFlags.FilterRequired, customLabels: customFields),
                    Options = await provider.GetNewProjectOptionsAsync(config)
                };


            }
            return result;
        }

        [HttpPost]
        public async Task<ProjectQueryResultModel> GetFilteredProjectsAsync([FromBody] ProjectQueryModel searchTerms)
        {
            using (var context = new PortfolioContext())
            {
                ProjectQueryResultModel result = null;
                var filteredQuery = from p in context.Projects.IncludeQueryResult() where p.Reservation.Portfolio.ViewKey == searchTerms.PortfolioViewKey select p;
                var config = await context.PortfolioConfigurations.SingleAsync(p => p.Portfolio.ViewKey == searchTerms.PortfolioViewKey);
                var filterBuilder = new ProjectSearchFilters(searchTerms, filteredQuery, config);
                filterBuilder.BuildFilters();
                filteredQuery = filterBuilder.Query;

                result = PortfolioMapper.ProjectMapper.Map<ProjectQueryResultModel>(await filteredQuery.OrderByDescending(p => p.Priority).ToArrayAsync());
                return result;
            }
        }

        [HttpGet]
        public async Task<GetProjectExportDTO> GetExportProjectsAsync([FromUri(Name = "portfolio")] string viewKey)
        {
            using (var context = new PortfolioContext())
            {
                var reservationIds = await (
                    from p in context.Projects
                    where p.Reservation.Portfolio.ViewKey == viewKey && p.LatestUpdate.Phase.Id != p.Reservation.Portfolio.Configuration.CompletedPhase.Id
                    select p.ProjectReservation_Id
                    ).ToListAsync();

                var projectQuery = from p in context.Projects.IncludeProject()
                                   where  reservationIds.Contains(p.ProjectReservation_Id)
                                   select p;

                var provider = new PortfolioProvider(context, viewKey);
                var config = await provider.GetConfigAsync();
                var projects = await projectQuery.OrderByDescending(p => p.Priority).ToArrayAsync();
                GetProjectExportDTO result = new GetProjectExportDTO()
                {
                    Config = PortfolioMapper.GetProjectLabelConfigModel(config, includedOnly: true),
                    Projects = PortfolioMapper.ExportMapper.Map<IEnumerable<ProjectExportModel>>(projects)
                };
                return result;
            }
        }


    }
}
