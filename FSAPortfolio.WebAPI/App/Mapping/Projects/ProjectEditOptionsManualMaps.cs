using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects
{
    public class ProjectEditOptionsManualMaps
    {
        internal static async Task MapAsync(PortfolioContext context, PortfolioConfiguration config, IEnumerable<Project> projects, ProjectEditOptionsModel options)
        {
            var directorates = await context.Directorates.OrderBy(d => d.Order).ToListAsync();
            var directorateItems = PortfolioMapper.ProjectMapper.Map<List<DropDownItemModel>>(directorates);
            directorateItems.Insert(0, new DropDownItemModel() { Display = "None", Value = "", Order = 0 });
            options.Directorates = directorateItems;

            var teams = PortfolioMapper.ProjectMapper.Map<List<DropDownItemModel>>(config.Portfolio.Teams.OrderBy(d => d.Order));
            options.G6Team = teams;
        }
    }
}