using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Projects;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.Mapping;
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
                    .IncludeConfig()
                    .IncludeProjects()
                    .SingleOrDefaultAsync(p => p.ViewKey == viewKey);

                if (portfolio == null) throw new HttpResponseException(HttpStatusCode.NotFound);

                result = PortfolioMapper.ConfigMapper.Map<PortfolioSummaryModel>(
                    portfolio, 
                    opt => { 
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
                    Config = PortfolioMapper.GetProjectLabelConfigModel(config, PortfolioFieldFlags.FilterProject|PortfolioFieldFlags.FilterRequired, customFields),
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
                if (!string.IsNullOrWhiteSpace(searchTerms.Name))
                {
                    filteredQuery = filteredQuery.Where(p => p.Name.Contains(searchTerms.Name) || p.Reservation.ProjectId.Contains(searchTerms.Name));
                }
                filteredQuery = AddExactMatchFilter(searchTerms.Themes, filteredQuery, p => searchTerms.Themes.Contains(p.Theme));
                filteredQuery = AddExactMatchFilter(searchTerms.ProjectTypes, filteredQuery, p => searchTerms.ProjectTypes.Contains(p.ProjectType));
                filteredQuery = AddExactMatchFilter(searchTerms.Categories, filteredQuery, p => searchTerms.Categories.Contains(p.Category.ViewKey) || searchTerms.Categories.Intersect(p.Subcategories.Select(sc => sc.ViewKey)).Any());
                filteredQuery = AddExactMatchFilter(searchTerms.Directorates, filteredQuery, p => searchTerms.Directorates.Contains(p.Directorate));
                filteredQuery = AddExactMatchFilter(searchTerms.StrategicObjectives, filteredQuery, p => searchTerms.StrategicObjectives.Contains(p.StrategicObjectives));
                filteredQuery = AddExactMatchFilter(searchTerms.Programmes, filteredQuery, p => searchTerms.Programmes.Contains(p.Programme));

                // Project team
                if (!string.IsNullOrWhiteSpace(searchTerms.TeamMemberName))
                {
                    filteredQuery = filteredQuery.Where(p =>
                        p.Lead.Firstname.StartsWith(searchTerms.TeamMemberName) ||
                        p.Lead.Surname.StartsWith(searchTerms.TeamMemberName) ||
                        p.Lead.Email.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact1.Firstname.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact1.Surname.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact1.Email.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact2.Firstname.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact2.Surname.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact2.Email.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact3.Firstname.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact3.Surname.StartsWith(searchTerms.TeamMemberName) ||
                        p.KeyContact3.Email.StartsWith(searchTerms.TeamMemberName) ||
                        p.Team.Any(t =>
                            t.Firstname.StartsWith(searchTerms.TeamMemberName) ||
                            t.Surname.StartsWith(searchTerms.TeamMemberName) ||
                            t.Email.StartsWith(searchTerms.TeamMemberName))
                        );
                }
                // Project lead search
                if (!string.IsNullOrWhiteSpace(searchTerms.ProjectLeadName))
                {
                    filteredQuery = filteredQuery.Where(p =>
                        p.Lead.Firstname.StartsWith(searchTerms.ProjectLeadName) ||
                        p.Lead.Surname.StartsWith(searchTerms.ProjectLeadName) ||
                        p.Lead.Email.StartsWith(searchTerms.ProjectLeadName)
                        );
                }

                // Progress
                filteredQuery = AddExactMatchFilter(searchTerms.Phases, filteredQuery, p => searchTerms.Phases.Contains(p.LatestUpdate.Phase.ViewKey));
                filteredQuery = AddExactMatchFilter(searchTerms.RAGStatuses, filteredQuery, p => searchTerms.RAGStatuses.Contains(p.LatestUpdate.RAGStatus.ViewKey));
                filteredQuery = AddExactMatchFilter(searchTerms.OnHoldStatuses, filteredQuery, p => searchTerms.OnHoldStatuses.Contains(p.LatestUpdate.OnHoldStatus.ViewKey));

                // Prioritisation
                if (searchTerms.Priorities != null && searchTerms.Priorities.Length > 0)
                {
                    filteredQuery = filteredQuery.Where(p => p.Priority.HasValue && searchTerms.Priorities.Contains(p.Priority.Value));
                }

                result = PortfolioMapper.ProjectMapper.Map<ProjectQueryResultModel>(await filteredQuery.OrderByDescending(p => p.Priority).ToArrayAsync());
                return result;
            }
        }

        private IQueryable<Project> AddExactMatchFilter(string[] terms, IQueryable<Project> query, Expression<Func<Project, bool>> filter)
        {
            return (terms != null && terms.Length > 0) ? query.Where(filter) : query;
        }

    }
}
