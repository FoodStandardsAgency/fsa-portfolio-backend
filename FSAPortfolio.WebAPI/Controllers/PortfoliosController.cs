using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Projects;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FSAPortfolio.WebAPI.App.Sync;
using System.IO;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.Application.Services.Projects;
using FSAPortfolio.Common.Logging;
using FSAPortfolio.Common;
using FSAPortfolio.Entities.Projects;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class PortfoliosController : ApiController
    {
        private readonly IPortfolioService portfolioService;
        private readonly IProjectDataService projectDataService;
        private readonly ISyncService syncService;
        public PortfoliosController(IPortfolioService portfolioService, IProjectDataService projectDataService, ISyncService syncService)
        {
            this.portfolioService = portfolioService;
            this.projectDataService = projectDataService;
            this.syncService = syncService;

#if DEBUG
            AppLog.TraceVerbose($"{nameof(PortfoliosController)} created.");
#endif

        }

        [HttpGet]
        public async Task<IEnumerable<PortfolioModel>> Index()
        {
            return await portfolioService.GetPortfoliosAsync();
        }



        [HttpGet]
        public async Task<PortfolioSummaryModel> Summary([FromUri(Name = "portfolio")] string viewKey,
                                                         [FromUri(Name = "type")] string summaryType = PortfolioSummaryModel.ByCategory,
                                                         [FromUri(Name = "user")] string user = null,
                                                         [FromUri(Name = "projectType")] string projectType = null,
                                                         [FromUri(Name = "includeKeyData")] bool includeKeyData = false)
        {
            return await portfolioService.GetSummaryAsync(viewKey, summaryType, user, projectType, includeKeyData);
        }


        [HttpGet]
        public async Task<GetProjectQueryDTO> FilterOptionsAsync([FromUri(Name = "portfolio")] string viewKey)
        {
            GetProjectQueryDTO result = null;
            using (var context = new PortfolioContext())
            {
                var config = await portfolioService.GetConfigAsync(viewKey);
                var customFields = portfolioService.GetCustomFilterLabels(config);

                result = new GetProjectQueryDTO()
                {
                    Config = PortfolioMapper.GetProjectLabelConfigModel(config, PortfolioFieldFlags.FilterProject|PortfolioFieldFlags.FilterRequired, customLabels: customFields),
                    Options = await portfolioService.GetNewProjectOptionsAsync(config)
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
            return await projectDataService.GetProjectExportDTOAsync(viewKey);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> ImportAsync([FromUri(Name = "portfolio")] string viewKey)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Get the files
            var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());
            var files = await Request.Content.ReadAsMultipartAsync(provider);

            await projectDataService.ImportProjectsAsync(viewKey, files);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet, OverrideAuthorization]
        public async Task<ArchiveResponse> ArchiveProjectsAsync([FromUri(Name = "portfolio")] string viewKey)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(viewKey)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            ArchiveResponse response = new ArchiveResponse();

            using (var context = new PortfolioContext())
            {
                response.ArchivedProjectIds = await portfolioService.ArchiveProjectsAsync(viewKey);
            }

            return response;
        }

        [HttpPost]
        public async Task CreateAsync([FromBody] NewPortfolioModel model)
        {
            using (var context = new PortfolioContext())
            {
                var portfolio = syncService.AddPortfolio(context, model.Name, model.ShortName, model.ViewKey);
                var labels = new DefaultFieldLabels(portfolio.Configuration);
                portfolio.Configuration.Labels = labels.GetDefaultLabels();
                portfolio.IDPrefix = portfolio.ViewKey.ToUpper();
                context.Portfolios.Add(portfolio);
                await context.SaveChangesAsync();

                portfolio.Configuration.CompletedPhase = portfolio.Configuration.Phases.Single(p => p.ViewKey == PhaseConstants.BacklogViewKey);
                portfolio.Configuration.ArchivePhase = portfolio.Configuration.Phases.Single(p => p.ViewKey == PhaseConstants.ArchiveViewKey);
                await context.SaveChangesAsync();
            }
        }

        [AcceptVerbs("POST"), Authorize(Roles = "superuser")]
        public async Task AddPermissionAsync([FromUri(Name = "portfolio")] string viewKey, [FromBody] PortfolioPermissionModel model)
        {
            using (var context = new PortfolioContext())
            {
                var portfolio = await context.Portfolios.SingleAsync(p => p.ViewKey == viewKey);
                var user = await context.Users.SingleAsync(u => u.UserName == model.UserName);
                var role = $"{portfolio.IDPrefix}.{model.Permission}";
                user.RoleList = (string.IsNullOrWhiteSpace(user.RoleList) ? role : $"{user.RoleList};{role}");
                await context.SaveChangesAsync();
            }
        }
    }


}
