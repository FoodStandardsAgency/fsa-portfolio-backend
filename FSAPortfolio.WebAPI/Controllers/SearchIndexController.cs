using FSAPortfolio.Application.Services.Index;
using FSAPortfolio.Application.Services.Index.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class SearchIndexController : ApiController
    {
        private readonly IIndexManagerService indexManagerService;
        private readonly IIndexService indexService;
        private readonly ISearchService searchService;
        public SearchIndexController(IIndexManagerService indexManagerService, IIndexService indexService, ISearchService searchService)
        {
            this.indexManagerService = indexManagerService;
            this.indexService = indexService;
            this.searchService = searchService;
        }

        [HttpGet, Route("api/SearchIndex/{projectId}/index")]
        public async Task IndexProjectAsync(string projectId)
        {
            await indexService.IndexProjectAsync(projectId);
        }

        [HttpGet, Route("api/SearchIndex/create")]
        public async Task<IndexOperationResult> CreateProjectIndexAsync()
        {
            return await indexManagerService.CreateIndexAsync();
        }

        [HttpGet, Route("api/SearchIndex/rebuild")]
        public async Task<IndexOperationResult> RebuildProjectIndexAsync()
        {
            return await indexManagerService.RebuildIndexAsync();
        }

        [HttpPost, Route("api/SearchIndex/search")]
        public async Task<IEnumerable<ProjectSearchIndexModel>> SearchProjectIndexAsync(SearchTerm term)
        {
            return await searchService.SearchProjectIndexAsync(term.term);
        }

        [HttpGet, Route("api/SearchIndex/health")]
        public async Task<object> GetIndexHealthAsync()
        {
            return await indexManagerService.GetHealthAsync();
        }
    }

    public class SearchTerm
    {
        public string term { get; set; }
    }
}