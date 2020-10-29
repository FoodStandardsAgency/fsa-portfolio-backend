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
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Config;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class PortfolioConfigurationController : ApiController
    {

        [HttpPatch]
        public async Task Patch([FromUri(Name = "id")] string viewKey, [FromBody] PortfolioConfigUpdateRequest update)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(viewKey)) viewKey = update.ViewKey;
                using (var context = new PortfolioContext())
                {
                    // Map the updates to labels
                    var labelUpdates = PortfolioMapper.ConfigMapper.Map<IEnumerable<PortfolioLabelConfig>>(update.Labels);

                    // Get the config with labels
                    var config = await context.PortfolioConfigurations
                        .IncludeFullConfiguration()
                        .Where(p => p.Portfolio.ViewKey == viewKey)
                        .SingleAsync();

                    // Update the labels
                    foreach(var labelUpdate in labelUpdates)
                    {
                        var label = config.Labels.Single(l => l.FieldName == labelUpdate.FieldName);
                        PortfolioMapper.UpdateMapper.Map(labelUpdate, label);
                    }

                    // Record changes
                    AuditProvider.LogChanges(
                        context, 
                        (ts, txt) => auditLogFactory(config, nameof(PortfolioLabelConfig), ts, txt), 
                        context.PortfolioConfigAuditLogs, 
                        DateTime.Now);

                    // Map the collections here: don't do this in mapping because can't use async in resolvers
                    var configProvider = new ConfigurationProvider(context);
                    await configProvider.UpdateCollections(config);

                    await context.SaveChangesAsync();


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET: api/PortfolioConfiguration
        [HttpGet]
        public async Task<PortfolioConfigModel> Get([FromUri(Name ="id")] string portfolio)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var pfolio = await context.Portfolios.IncludeConfig()
                        .SingleAsync(p => p.ViewKey == portfolio);
                    var model = PortfolioMapper.ConfigMapper.Map<PortfolioConfigModel>(pfolio.Configuration);
                    model.Labels = model.Labels.OrderBy(l => l.FieldGroup).ThenBy(l => l.FieldOrder).ToList();

                    return model;
                } 
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private PortfolioConfigAuditLog auditLogFactory(PortfolioConfiguration con, string type, DateTime timestamp, string text)
        {
            return new PortfolioConfigAuditLog()
            {
                Timestamp = timestamp,
                PortfolioConfiguration_Id = con.Portfolio_Id,
                AuditType = type,
                Text = text
            };
        }
    }
}
