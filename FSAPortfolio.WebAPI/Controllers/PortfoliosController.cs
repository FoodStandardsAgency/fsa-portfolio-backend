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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FSAPortfolio.WebAPI.App.Sync;
using System.IO;
using FSAPortfolio.Application.Services.Projects;
using FSAPortfolio.Common.Logging;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Services;
using FSAPortfolio.Application.Services.Index;
using FSAPortfolio.Application.Services.Index.Models;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class PortfoliosController : ApiController
    {
        private readonly IPortfolioService portfolioService;
        private readonly IProjectDataService projectDataService;
        private readonly ISyncService syncService;
        private readonly ISearchService searchService;
        private readonly ServiceContext serviceContext;

        public PortfoliosController(ServiceContext serviceContext, IPortfolioService portfolioService, IProjectDataService projectDataService, ISyncService syncService, ISearchService searchService)
        {
            this.portfolioService = portfolioService;
            this.projectDataService = projectDataService;
            this.syncService = syncService;
            this.searchService = searchService;
            this.serviceContext = serviceContext;

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
        public async Task<PortfolioSummaryModel> SummaryLabels([FromUri(Name = "portfolio")] string viewKey)
        {
            return await portfolioService.GetSummaryLabelsAsync(viewKey);
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
            ProjectQueryResultModel result = null;
            IEnumerable<ProjectQueryResultProjectModel> textSearchResults = null;
            IEnumerable<ProjectQueryResultProjectModel> queryResults = null;

            bool hasFilterTerms = checkForFilterTerms(searchTerms);
            bool hasTextSearchTerm = !string.IsNullOrWhiteSpace(searchTerms.TextSearch);

            if (hasTextSearchTerm)
            {
                var indexResults = await searchService.SearchProjectIndexAsync(searchTerms.TextSearch);
                if(indexResults.Count() > 0)
                {
                    textSearchResults = PortfolioMapper.ProjectMapper.Map<IEnumerable<ProjectQueryResultProjectModel>>(indexResults);

                }
            }


            if (hasFilterTerms || !hasTextSearchTerm)
            {
                using (var context = new PortfolioContext())
                {
                    var filteredQuery = from p in context.Projects.IncludeQueryResult() where p.Reservation.Portfolio.ViewKey == searchTerms.PortfolioViewKey select p;
                    var config = await context.PortfolioConfigurations.SingleAsync(p => p.Portfolio.ViewKey == searchTerms.PortfolioViewKey);
                    var filterBuilder = new ProjectSearchFilters(searchTerms, filteredQuery, config);
                    filterBuilder.BuildFilters();
                    filteredQuery = filterBuilder.Query;
                    queryResults = PortfolioMapper.ProjectMapper.Map<IEnumerable<ProjectQueryResultProjectModel>>(await filteredQuery.ToArrayAsync());
                }
            }
            if (queryResults != null && textSearchResults != null) queryResults = queryResults.Union(textSearchResults);
            else if (queryResults == null && textSearchResults != null) queryResults = textSearchResults;

            if (queryResults != null)
            {
                result = PortfolioMapper.ProjectMapper.Map<ProjectQueryResultModel>(queryResults.OrderByDescending(p => p.Priority));
            }
            return result;
        }

        private bool checkForFilterTerms(ProjectQueryModel searchTerms)
        {
            var t = searchTerms.GetType();
            var properties = t.GetProperties();
            foreach(var property in properties.Where(p => p.Name != nameof(ProjectQueryModel.TextSearch) && p.Name != nameof(ProjectQueryModel.PortfolioViewKey)))
            {
                var objValue = property.GetValue(searchTerms);

                // This is a filter term if the property has a value and is not an empty string
                if (objValue != null && !(property.PropertyType == typeof(string) && string.IsNullOrWhiteSpace((string)objValue)))
                    return true;
            }
            return false;
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

                portfolio.Configuration.CompletedPhase = portfolio.Configuration.Phases.Single(p => p.ViewKey == PhaseConstants.CompletedViewKey);
                portfolio.Configuration.ArchivePhase = portfolio.Configuration.Phases.Single(p => p.ViewKey == PhaseConstants.ArchiveViewKey);
                await context.SaveChangesAsync();
            }
        }

        [AcceptVerbs("POST"), Authorize(Roles = "superuser")]
        public async Task AddPermissionAsync([FromUri(Name = "portfolio")] string viewKey, [FromBody] PortfolioPermissionModel model)
        {
            await portfolioService.AddPermissionAsync(viewKey, model);
        }

        [AcceptVerbs("GET"), Route("api/Portfolios/cleanreservations")]
        [OverrideAuthorization]
        public async Task CleanReservationsAsync()
        {
            serviceContext.AssertSuperuser();
            await portfolioService.CleanReservationsAsync();
        }

    }


}
