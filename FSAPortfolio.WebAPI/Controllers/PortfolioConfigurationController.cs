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

namespace FSAPortfolio.WebAPI.Controllers
{
    public class PortfolioConfigurationController : ApiController
    {
        // POST: api/PortfolioConfiguration/Label
        [HttpPost]
        public async Task PostLabel([FromBody] PortfolioConfigLabelRequest labelRequest)
        {
            using (var context = new PortfolioContext())
            {
                var portfolio = await context.Portfolios.Include(p => p.Configuration.Labels).SingleAsync(p => p.ViewKey == labelRequest.portfolio);
                var label = portfolio.Configuration.Labels.SingleOrDefault(l => l.FieldName == labelRequest.field);
                if(label == null)
                {
                    label = new PortfolioLabelConfig() { FieldName = labelRequest.field };
                    portfolio.Configuration.Labels.Add(label);
                }
                label = PortfolioMapper.Mapper.Map(labelRequest, label);

                // Save
                await context.SaveChangesAsync();
            }
        }

        // GET: api/PortfolioConfiguration
        [HttpGet]
        public async Task<PortfolioConfigModel> Get([FromUri] string portfolio)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var pfolio = await context.Portfolios.Include(p => p.Configuration.Labels).SingleAsync(p => p.ViewKey == portfolio);
                    var model = PortfolioMapper.Mapper.Map<PortfolioConfigModel>(pfolio.Configuration);
                    return model;
                } 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public async Task<string> GetMaxId([FromUri] string portfolio)
        {
            using (var context = new PortfolioContext())
            {
                return await context.Projects.Where(p => p.OwningPortfolio.ViewKey == portfolio).MaxAsync(p => p.ProjectId);
            }

        }
    }
}
