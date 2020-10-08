using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public static class ProjectQueryExtensions
    {
        public static IQueryable<Project> ProjectIncludes(this IQueryable<Project> query)
        {
            return query.Include(p => p.Category)
                .Include(p => p.FirstUpdate.OnHoldStatus)
                .Include(p => p.FirstUpdate.RAGStatus)
                .Include(p => p.FirstUpdate.Phase)
                .Include(p => p.LatestUpdate.OnHoldStatus)
                .Include(p => p.LatestUpdate.RAGStatus)
                .Include(p => p.LatestUpdate.Phase)
                .Include(p => p.Category)
                .Include(p => p.Size)
                .Include(p => p.BudgetType)
                .Include(p => p.RelatedProjects)
                .Include(p => p.DependantProjects)
                .Include(p => p.Lead);
        }
        public static IQueryable<Project> ConfigIncludes(this IQueryable<Project> query)
        {
            return query.Include(p => p.OwningPortfolio.Configuration.Labels)
                .Include(p => p.OwningPortfolio.Configuration.LabelGroups)
                ;
        }
    }
}