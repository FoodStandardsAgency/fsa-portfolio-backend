using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
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
using FSAPortfolio.WebAPI.App.Sync;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;
using FSAPortfolio.WebAPI.App.Users;
using AutoMapper;

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
                var portfolios = await context.Portfolios.ToListAsync();
                List<Portfolio> validPortfolios = null;
                validPortfolios = new List<Portfolio>();
                foreach(var portfolio in portfolios)
                {
                    if(this.HasPermission(portfolio))
                    {
                        validPortfolios.Add(portfolio);
                    }
                }
                result = PortfolioMapper.ConfigMapper.Map<IEnumerable<PortfolioModel>>(validPortfolios);
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

                if (this.HasPermission(portfolio))
                {
                    result = PortfolioMapper.ConfigMapper.Map<PortfolioSummaryModel>(
                    portfolio,
                    opt =>
                    {
                        opt.Items[nameof(PortfolioConfiguration)] = portfolio.Configuration;
                        opt.Items[nameof(PortfolioSummaryModel)] = summaryType;
                    });
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
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
                    where p.Reservation.Portfolio.ViewKey == viewKey
                    select p.ProjectReservation_Id
                    ).ToListAsync();

                var projectQuery = from p in context.Projects.IncludeProject()
                                   where reservationIds.Contains(p.ProjectReservation_Id)
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

        [HttpPost]
        public async Task<HttpResponseMessage> ImportAsync([FromUri(Name = "portfolio")] string viewKey)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                // Get the files
                var provider = new MultipartFormDataStreamProvider(Path.GetTempPath());
                var files = await Request.Content.ReadAsMultipartAsync(provider);

                using (var context = new PortfolioContext())
                {
                    // Get the config and options
                    var portfolioProvider = new PortfolioProvider(context, viewKey);
                    var config = await portfolioProvider.GetConfigAsync();
                    this.AssertPermission(config.Portfolio);
                    var options = await portfolioProvider.GetNewProjectOptionsAsync(config);

                    // Import the projects
                    var importer = new PropertyImporter();
                    var projects = await importer.ImportProjectsAsync(files, config, options);

                    // Update/create the projects
                    var userProvider = new PersonProvider(context);
                    var projectprovider = new ProjectProvider(context);
                    foreach (var project in projects)
                    {
                        if (string.IsNullOrWhiteSpace(project.project_id))
                        {
                            // Create a reservation
                            var reservation = await portfolioProvider.GetProjectReservationAsync(config);
                            project.project_id = reservation.ProjectId;
                            await projectprovider.UpdateProject(project, userProvider, reservation);
                        }
                        else
                        {
                            await projectprovider.UpdateProject(project, userProvider);
                        }
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                throw e;
            }
        }


        [HttpGet, OverrideAuthorization]
        public async Task<ArchiveResponse> ArchiveProjectsAsync([FromUri(Name = "portfolio")] string viewKey)
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(viewKey)) throw new HttpResponseException(HttpStatusCode.BadRequest);

            ArchiveResponse response = new ArchiveResponse();

            using (var context = new PortfolioContext())
            {
                var provider = new PortfolioProvider(context, viewKey);
                response.ArchivedProjectIds = await provider.ArchiveProjectsAsync();
            }

            return response;
        }

        [HttpPost]
        public async Task CreateAsync([FromBody] NewPortfolioModel model)
        {
            using (var context = new PortfolioContext())
            {
                var log = new List<string>();
                var syncProvider = new SyncProvider(log);
                var portfolio = syncProvider.AddPortfolio(context, model.Name, model.ShortName, model.ViewKey);
                var labels = new DefaultFieldLabels(portfolio.Configuration);
                portfolio.Configuration.Labels = labels.GetDefaultLabels();
                portfolio.IDPrefix = portfolio.ViewKey.ToUpper();
                context.Portfolios.Add(portfolio);
                await context.SaveChangesAsync();

                portfolio.Configuration.CompletedPhase = portfolio.Configuration.Phases.Single(p => p.ViewKey == $"{ViewKeyPrefix.Phase}5");
                portfolio.Configuration.ArchivePhase = portfolio.Configuration.Phases.Single(p => p.ViewKey == $"{ViewKeyPrefix.Phase}4");
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
