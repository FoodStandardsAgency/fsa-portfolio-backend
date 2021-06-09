using FSAPortfolio.Application.Models;
using FSAPortfolio.Application.Services.Config;
using FSAPortfolio.Application.Services.Projects;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class DataDumpController : ApiController
    {
        private readonly IDataDumpService dataDumpService;

        public DataDumpController(IDataDumpService dataDumpService)
        {
            this.dataDumpService = dataDumpService;
        }

        [HttpGet, Route("api/DataDump/config")]
        public async Task DumpPortfolioConfiguration([FromUri] string portfolio)
        {
            await dataDumpService.DumpPortfolioConfig(portfolio);
        }

        [HttpGet, Route("api/DataDump/projects")]
        public async Task DumpProjects([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            await dataDumpService.DumpPortfolioProjects(portfolio, id);
        }

        [HttpGet, Route("api/DataDump/updates")]
        public async Task DumpProjectUpdates([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            await dataDumpService.DumpProjectUpdates(portfolio, id);
        }

        [HttpGet, Route("api/DataDump/changes")]
        public async Task DumpProjectChanges([FromUri] string portfolio, [FromUri] string[] id = null)
        {
            await dataDumpService.DumpProjectChanges(portfolio, id);
        }

    }

}
