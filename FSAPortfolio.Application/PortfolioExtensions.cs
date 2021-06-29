using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public static async Task LoadProjectsIntoPortfolioAsync(this PortfolioContext context, Portfolio portfolio, Expression<Func<Project, bool>> filter = null)
        {
            var query = context.Entry(portfolio).Collection(p => p.Projects)
                .Query()
                .Include(p => p.Reservation)
                .Include(p => p.Lead.Team)
                .Include(p => p.People)
                .Include(p => p.Category)
                .Include(p => p.LatestUpdate.Phase)
                .Include(p => p.FirstUpdate);

            if (filter != null) query = query.Where(filter);
            await query.LoadAsync();
            portfolio.Projects = context.Projects.Local;
        }

    }
}