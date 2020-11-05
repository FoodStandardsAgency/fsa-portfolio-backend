using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public static class PortfolioExtensions
    {
        public static IQueryable<Portfolio> IncludeConfig(this IQueryable<Portfolio> query)
        {
            return query
                .Include(p => p.Configuration.Phases)
                .Include(p => p.Configuration.RAGStatuses)
                .Include(p => p.Configuration.OnHoldStatuses)
                .Include(p => p.Configuration.Categories)
                .Include(p => p.Configuration.ProjectSizes)
                .Include(p => p.Configuration.BudgetTypes)
                .Include(p => p.Configuration.Labels)
                .Include(p => p.Configuration.LabelGroups)
                ;
        }
        public static IQueryable<Portfolio> IncludeProjects(this IQueryable<Portfolio> query)
        {
            return query
                .Include(p => p.Projects.Select(pr => pr.Reservation))
                .Include(p => p.Projects.Select(pr => pr.Lead))
                .Include(p => p.Projects.Select(pr => pr.Category))
                .Include(p => p.Projects.Select(pr => pr.LatestUpdate.Phase))
                ;
        }

    }
}