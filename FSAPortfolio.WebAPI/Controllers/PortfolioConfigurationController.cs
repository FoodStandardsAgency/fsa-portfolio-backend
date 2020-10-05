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
                    var labelUpdates = PortfolioMapper.Mapper.Map<IEnumerable<PortfolioLabelConfig>>(update.Labels);

                    // Get the config with labels
                    var config = await context.PortfolioConfigurations
                        .Include(pc => pc.Labels)
                        .Where(p => p.Portfolio.ViewKey == viewKey)
                        .SingleAsync();

                    // Update the labels
                    foreach(var labelUpdate in labelUpdates)
                    {
                        var label = config.Labels.Single(l => l.FieldName == labelUpdate.FieldName);
                        PortfolioMapper.UpdateMapper.Map(labelUpdate, label);
                    }

                    // Record changes
                    LogChanges(context, config, nameof(PortfolioLabelConfig), DateTime.Now);

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
                    var pfolio = await context.Portfolios.Include(p => p.Configuration.Labels)
                        .SingleAsync(p => p.ViewKey == portfolio);
                    var model = PortfolioMapper.Mapper.Map<PortfolioConfigModel>(pfolio.Configuration);
                    model.Labels = model.Labels.OrderBy(l => l.FieldGroup).ThenBy(l => l.FieldOrder).ToList();
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

        private static void LogChanges(PortfolioContext context, PortfolioConfiguration con, string type, DateTime timestamp)
        {
            var changes = context.ChangeTracker.Entries().Where(c => c.State == EntityState.Modified);
            if (changes.Count() > 0)
            {
                var log = new PortfolioConfigAuditLog() { 
                    Timestamp = timestamp,
                    PortfolioConfiguration_Id = con.Id,
                    AuditType = type
                };
                var logText = new List<string>();
                foreach (var change in changes)
                {
                    var originalValues = change.OriginalValues;
                    var currentValues = change.CurrentValues;
                    foreach (string pname in originalValues.PropertyNames)
                    {
                        var originalValue = originalValues[pname];
                        var currentValue = currentValues[pname];
                        if (!Equals(originalValue, currentValue))
                        {
                            logText.Add($"{pname}: [{originalValue}] to [{currentValue}]");
                        }
                    }
                }
                log.Text = string.Join("; ", logText);
                context.PortfolioConfigAuditLogs.Add(log);
            }
        }

    }
}
