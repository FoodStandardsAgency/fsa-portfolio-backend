using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Config
{
    public class ConfigurationProvider
    {
        private PortfolioContext context;

        public ConfigurationProvider(PortfolioContext context)
        {
            this.context = context;
        }

        // TODO: add audit log for this - especially if going from 5 to 3
        public async Task UpdateRAGStatusOptions(PortfolioConfiguration config)
        {
            var labelConfig = config.Labels.Single(l => l.FieldName == nameof(ProjectModel.rag));
            int options = int.Parse(labelConfig.FieldOptions);
            if (config.RAGStatuses.Count != options)
            {
                ProjectRAGStatus redAmber, amberGreen;
                switch (options)
                {
                    case 5:
                        // Add Red/Amber and Amber/Green and set the order
                        var redAmberMap = SyncMaps.ragMap[RagConstants.RedAmberViewKey];
                        var amberGreenMap = SyncMaps.ragMap[RagConstants.AmberGreenViewKey];
                        redAmber = new ProjectRAGStatus() { ViewKey = RagConstants.RedAmberViewKey, Name = redAmberMap.Item1, Order = redAmberMap.Item2 };
                        amberGreen = new ProjectRAGStatus() { ViewKey = RagConstants.AmberGreenViewKey, Name = amberGreenMap.Item1, Order = amberGreenMap.Item2 };
                        config.RAGStatuses.Add(redAmber);
                        config.RAGStatuses.Add(amberGreen);
                        break;
                    case 3:
                        // Remove Red/Amber and Amber/Green, map any projects with those RAGs
                        var red = config.RAGStatuses.Single(rag => rag.ViewKey == RagConstants.RedViewKey);
                        var amber = config.RAGStatuses.Single(rag => rag.ViewKey == RagConstants.AmberViewKey);
                        redAmber = config.RAGStatuses.Single(rag => rag.ViewKey == RagConstants.RedAmberViewKey);
                        amberGreen = config.RAGStatuses.Single(rag => rag.ViewKey == RagConstants.AmberGreenViewKey);

                        var redAmberUpdates = await context.ProjectUpdates.Where(u => u.RAGStatus.Id == redAmber.Id).ToListAsync();
                        foreach (var update in redAmberUpdates) update.RAGStatus = red;

                        var amberGreenUpdates = await context.ProjectUpdates.Where(u => u.RAGStatus.Id == amberGreen.Id).ToListAsync();
                        foreach (var update in amberGreenUpdates) update.RAGStatus = amber;

                        context.ProjectRAGStatuses.Remove(redAmber);
                        context.ProjectRAGStatuses.Remove(amberGreen);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"The label configuration for fieldname=[{nameof(ProjectModel.rag)}] has an unrecognised value. Should be 3 or 5.");
                }

                await context.SaveChangesAsync();
            }

        }
    }
}