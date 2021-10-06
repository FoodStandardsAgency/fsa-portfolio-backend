using FSAPortfolio.Application.Models;
using FSAPortfolio.Application.Services.Index.Models;
using FSAPortfolio.Application.Services.Index.Nest;
using FSAPortfolio.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Mapping;
using System.Collections.Generic;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Services.Projects;

namespace FSAPortfolio.Application.Services.Index
{
    public class IndexService : IIndexService
    {
        private Lazy<PortfolioContext> lazyPortfolioContext;
        private ProjectNestClient nestClient = new ProjectNestClient();


        public IndexService(Lazy<PortfolioContext> lazyPortfolioContext)
        {
            this.lazyPortfolioContext = lazyPortfolioContext;
        }

        public async Task DeleteProjectAsync(string projectId)
        {
            await nestClient.DeleteProjectAsync(projectId);
        }
        public async Task ReindexProjectAsync(string projectId)
        {
            await IndexProjectImplAsync(projectId);
        }

        public async Task IndexProjectAsync(string projectId)
        {
            await IndexProjectImplAsync(projectId);
        }

        private async Task<bool> IndexProjectImplAsync(string projectId)
        {
            bool projectExists = false;
            var project = await GetProjectAsync(projectId);
            if (project != null)
            {
                projectExists = true;
                await nestClient.IndexProjectAsync(project);
            }
            else
            {
                await nestClient.DeleteProjectAsync(projectId);
            }
            return projectExists;
        }

        private async Task<ProjectSearchIndexModel> GetProjectAsync(string projectId)
        {
            string portfolio;
            ProjectSearchIndexModel result = null;
            var context = lazyPortfolioContext.Value;
            var reservation = await context.ProjectReservations.SingleOrDefaultAsync(r => r.ProjectId == projectId);

            if (reservation != null)
            {
                var query = (from p in context.Projects
                             .IncludeProject()
                             where p.ProjectReservation_Id == reservation.Id
                             select p);

                var project = await query.SingleOrDefaultAsync();
                if (project != null)
                {

                    portfolio = project.Reservation.Portfolio.ViewKey;

                    // Build the result
                    result = PortfolioMapper.IndexMapper.Map<ProjectSearchIndexModel>(project);

                }
            }

            return result;
        }


    }

}
