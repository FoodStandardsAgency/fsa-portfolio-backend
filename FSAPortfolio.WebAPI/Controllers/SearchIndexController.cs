using FSAPortfolio.Application.Services.Index;
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
        private IIndexService indexService;
        private ISearchService searchService;
        public SearchIndexController(IIndexService indexService, ISearchService searchService)
        {
            this.indexService = indexService;
            this.searchService = searchService;
        }

        [HttpGet, Route("api/SearchIndex/{projectId}/index")]
        public async Task IndexProjectAsync(string projectId)
        {
            await indexService.IndexProjectAsync(projectId);
        }

        [HttpGet, Route("api/SearchIndex/create")]
        public async Task CreateProjectIndexAsync()
        {
            await indexService.CreateIndexAsync();
        }

        [HttpGet, Route("api/SearchIndex/rebuild")]
        public async Task RebuildProjectIndexAsync()
        {
            await indexService.RebuildIndexAsync();
        }

        [HttpPost, Route("api/SearchIndex/search")]
        public async Task SearchProjectIndexAsync()
        {
            await searchService.SearchProjectIndexAsync();
        }

    }
}