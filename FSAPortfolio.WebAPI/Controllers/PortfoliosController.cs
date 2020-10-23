using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                result = PortfolioMapper.ConfigMapper.Map<IEnumerable<PortfolioModel>>(context.Portfolios);
            }
            return result;
        }

        [HttpGet]
        public async Task<PortfolioSummaryModel> Summary([FromUri(Name = "portfolio")] string viewKey)
        {
            PortfolioSummaryModel result = null;
            using (var context = new PortfolioContext())
            {
                var portfolio = await context.Portfolios
                    .IncludeConfig()
                    .IncludeProjects()
                    .SingleOrDefaultAsync(p => p.ViewKey == viewKey);

                if (portfolio == null) throw new HttpResponseException(HttpStatusCode.NotFound);

                result = PortfolioMapper.ConfigMapper.Map<PortfolioSummaryModel>(portfolio);
            }
            return result;
        }
    }
}
