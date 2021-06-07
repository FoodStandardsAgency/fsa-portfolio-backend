using FSAPortfolio.Application.Models;
using FSAPortfolio.Application.Services.Projects;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class DataDumpController : ApiController
    {
        private readonly IPortfolioService portfolioService;
        private readonly IProjectDataService projectDataService;

        public DataDumpController(IPortfolioService provider, IProjectDataService projectDataService)
        {
            this.portfolioService = provider;
            this.projectDataService = projectDataService;
        }


        [HttpGet, Route("api/DataDump/updates")]
        public async Task DumpProjectUpdates([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            var updates = await projectDataService.GetProjectUpdateDataAsync(portfolio, id);
        }

        [HttpGet, Route("api/DataDump/changes")]
        public async Task DumpProjectChanges([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            var changes = await projectDataService.GetProjectChangeDataAsync(portfolio, id);
        }

    }

}
