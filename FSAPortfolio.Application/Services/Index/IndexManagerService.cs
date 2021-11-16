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
    public class IndexManagerService : IIndexManagerService
    {
        private static volatile bool operationInProgress = false;
        private static object indexLock = new object();

        private Lazy<PortfolioContext> lazyPortfolioContext;
        private readonly IPortfolioService portfolioService;
        private ProjectNestClient nestClient = new ProjectNestClient();


        public IndexManagerService(Lazy<PortfolioContext> lazyPortfolioContext, IPortfolioService portfolioService)
        {
            this.lazyPortfolioContext = lazyPortfolioContext;
            this.portfolioService = portfolioService;
        }

        public bool OperationInProgress => operationInProgress;

        public async Task<object> CreateIndexAsync()
        {
            return ExecuteIndexOperation(async (ct) =>
            {
                await nestClient.CreateProjectIndexAsync();
                await IndexAllProjectsImplAsync();
            });
        }

        public async Task<IndexOperationResult> RebuildIndexAsync()
        {
            return ExecuteIndexOperation(async (ct) => {
                await IndexAllProjectsImplAsync();
            });
        }

        private async Task IndexAllProjectsImplAsync()
        {
            await this.portfolioService.CleanReservationsAsync();
            var context = lazyPortfolioContext.Value;
            var reservations = await context.ProjectReservations.ToListAsync();
            foreach(var r in reservations)
            {
                await IndexProjectImplAsync(r.ProjectId);
            }
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


        private IndexOperationResult ExecuteIndexOperation(Func<CancellationToken, Task> operation)
        {
            bool operationQueued = false;
            if (!operationInProgress) 
            {
                lock(indexLock)
                {
                    if (!operationInProgress)
                    {
                        try
                        {
                            operationInProgress = true;
                            operationQueued = true;
                            HostingEnvironment.QueueBackgroundWorkItem((ct) => {
                                try
                                {
                                    operation(ct);
                                }
                                finally
                                {
                                    operationInProgress = false;
                                }
                            });
                        }
                        catch(Exception e)
                        {
                            operationInProgress = false;
                            operationQueued = false;
                            throw e;
                        }
                    }
                }
            }
            return new IndexOperationResult { Message = operationQueued ? "Index Operation Queued" : "Index Busy" };
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

        public Task<object> GetHealthAsync()
        {
            return nestClient.GetStatusAsync();
        }
    }

}
