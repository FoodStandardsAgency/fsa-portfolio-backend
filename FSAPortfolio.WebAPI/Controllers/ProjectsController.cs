using FSAPortfolio.Entites;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.Entites.Projects;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class ProjectsController : ApiController
    {

        // GET: api/Projects/Current
        public async Task<IEnumerable<ProjectModel>> GetCurrent()
        {
            try
            {
                IEnumerable<ProjectModel> result = null;
                var directorate = DirectorateContext.Current;
                using (var context = new PortfolioContext())
                {

                    var projects = await (from p in context.Projects
                                    .Include(p => p.LatestUpdate.OnHoldStatus)
                                    .Include(p => p.LatestUpdate.RAGStatus)
                                    .Include(p => p.LatestUpdate.Phase)
                                          where p.LatestUpdate.Phase.Id != directorate.CompletedPhase.Id
                                          orderby p.Priority descending, p.Name
                                          select p)
                                    .ToListAsync();

                    result = PortfolioMapper.Mapper.Map<IEnumerable<ProjectModel>>(projects);
                }
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        // GET: api/Projects/Latest
        public async Task<IEnumerable<ProjectModel>> GetLatest()
        {
            IEnumerable<ProjectModel> result = null;
            var directorate = DirectorateContext.Current;
            using (var context = new PortfolioContext())
            {
                // latest_projects_int1: use latest update for max time, calculate min time
                // latest_projects_int3: left join to odd_people and get g6team
                // latest_projects: only takes the one with latest update timestamp 
                var projects = await (from p in ProjectWithIncludes(context)
                                      where p.LatestUpdate.Phase.Id != directorate.CompletedPhase.Id
                                      orderby p.Priority descending, p.Name
                                      select p)
                                .ToListAsync();
                result = PortfolioMapper.Mapper.Map<IEnumerable<ProjectModel>>(projects);
            }
            return result;
        }

        // GET: api/Projects/New
        public async Task<IEnumerable<ProjectModel>> GetNew()
        {
            IEnumerable<ProjectModel> result = null;
            var directorate = DirectorateContext.Current;
            using (var context = new PortfolioContext())
            {
                var newCutoff = DateTime.Now.AddDays(-PortfolioSettings.NewProjectLimitDays);
                var projects = await (from p in ProjectWithIncludes(context)
                                      where p.LatestUpdate.Phase.Id != directorate.CompletedPhase.Id && p.FirstUpdate.Timestamp > newCutoff
                                      orderby p.Priority descending, p.Name
                                      select p)
                                .ToListAsync();
                result = PortfolioMapper.Mapper.Map<IEnumerable<ProjectModel>>(projects);
            }
            return result;
        }

        private static IQueryable<Project> ProjectWithIncludes(PortfolioContext context)
        {
            return context.Projects
                .Include(p => p.FirstUpdate.OnHoldStatus)
                .Include(p => p.FirstUpdate.RAGStatus)
                .Include(p => p.FirstUpdate.Phase)
                .Include(p => p.LatestUpdate.OnHoldStatus)
                .Include(p => p.LatestUpdate.RAGStatus)
                .Include(p => p.LatestUpdate.Phase)
                .Include(p => p.Lead);
        }
    }
}
