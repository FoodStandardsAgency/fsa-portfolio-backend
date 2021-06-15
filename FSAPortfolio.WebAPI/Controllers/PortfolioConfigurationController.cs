using FSAPortfolio.Entities;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FSAPortfolio.WebAPI.App.Mapping;
using Newtonsoft.Json.Linq;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.App;
using System.Data.Entity.Validation;
using System.Text;
using FSAPortfolio.Application.Services.Config;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class PortfolioConfigurationController : ApiController
    {

        IPortfolioConfigurationService portfolioConfigurationService;

        public PortfolioConfigurationController(IPortfolioConfigurationService portfolioConfigurationService)
        {
            this.portfolioConfigurationService = portfolioConfigurationService;
        }

        [HttpPatch]
        public async Task Patch([FromUri(Name = "portfolio")] string viewKey, [FromBody] PortfolioConfigUpdateRequest update)
        {
            await portfolioConfigurationService.UpdateConfigAsync(viewKey, update);
        }

        // GET: api/PortfolioConfiguration
        [HttpGet]
        [OverrideAuthorization]
        public async Task<PortfolioConfigModel> Get([FromUri] string portfolio)
        {
            return await portfolioConfigurationService.GetConfigurationAsync(portfolio);
        }

    }
}
