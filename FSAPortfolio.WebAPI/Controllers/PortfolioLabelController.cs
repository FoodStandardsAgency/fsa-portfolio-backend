using FSAPortfolio.Entities;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FSAPortfolio.WebAPI.Mapping;
using Newtonsoft.Json.Linq;
using FSAPortfolio.Entities.Organisation;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class PortfolioLabelController : ApiController
    {

        // POST: api/PortfolioLabel
        [HttpPost]
        public async Task PostLabel([FromBody] PortfolioConfigAddLabelRequest labelRequest)
        {
            using (var context = new PortfolioContext())
            {
                var portfolio = await context.Portfolios.Include(p => p.Configuration.Labels).SingleAsync(p => p.ViewKey == labelRequest.ViewKey);
                var label = portfolio.Configuration.Labels.SingleOrDefault(l => l.FieldName == labelRequest.FieldName);
                if(label == null)
                {
                    label = new PortfolioLabelConfig() { FieldName = labelRequest.FieldName };
                    portfolio.Configuration.Labels.Add(label);
                }
                label = PortfolioMapper.Mapper.Map(labelRequest, label);

                // Save
                await context.SaveChangesAsync();
            }
        }

        [HttpDelete]
        public async Task DeleteLabel([FromUri] PortfolioConfigDeleteLabelRequest labelRequest)
        {
            using (var context = new PortfolioContext())
            {
                var label = await context.PortfolioConfigurationLabels
                    .SingleOrDefaultAsync(l => l.Configuration.Portfolio.ViewKey == labelRequest.ViewKey && l.FieldName == labelRequest.FieldName);

                if (label != null)
                {
                    context.PortfolioConfigurationLabels.Remove(label);
                    await context.SaveChangesAsync();
                }
                else throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

    }
}
