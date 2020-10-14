﻿using FSAPortfolio.Entities.Organisation;
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
                .Include(p => p.Phases)
                .Include(p => p.RAGStatuses)
                .Include(p => p.OnHoldStatuses)
                .Include(p => p.Categories)
                .Include(p => p.ProjectSizes)
                .Include(p => p.BudgetTypes)
                .Include(p => p.Labels)
                .Include(p => p.LabelGroups)
                ;
        }

    }
}