using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Includes for loading the full config for add/edit/view of a project
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<PortfolioConfiguration> ConfigIncludes(this IQueryable<PortfolioConfiguration> query)
        {
            return query
                .Include(c => c.Portfolio)
                .Include(c => c.Phases)
                .Include(c => c.RAGStatuses)
                .Include(c => c.OnHoldStatuses)
                .Include(c => c.Categories)
                .Include(c => c.ProjectSizes)
                .Include(c => c.BudgetTypes)
                .Include(c => c.Labels)
                .Include(c => c.LabelGroups)
                ;
        }

    }
}