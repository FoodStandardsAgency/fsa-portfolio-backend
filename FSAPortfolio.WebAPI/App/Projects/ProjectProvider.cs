using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Projects
{
    public class ProjectProvider : IDisposable
    {
        private PortfolioContext context;
        private string portfolioViewKey;
        public ProjectProvider(string portfolioViewKey)
        {
            this.context = new PortfolioContext();
            this.portfolioViewKey = portfolioViewKey;
        }

        public async Task<PortfolioConfiguration> GetConfigAsync()
        {
            return await (from c in context.PortfolioConfigurations.ConfigIncludes()
                            where c.Portfolio.ViewKey == portfolioViewKey
                          select c).SingleAsync();
        }

        public async Task<ProjectReservation> GetProjectReservationAsync(PortfolioConfiguration config)
        {
            var timestamp = DateTime.Now;
            int year = timestamp.Year;
            int month = timestamp.Month;

            var maxIndexQuery = context.ProjectReservations
                .Where(r => r.Portfolio_Id == config.Portfolio_Id && r.Year == year && r.Month == month)
                .Select(r => (int?)r.Index);
            int maxIndex = (await maxIndexQuery.MaxAsync()) ?? 0;

            var reservation = new ProjectReservation()
            {
                Year = year,
                Month = month,
                Index = maxIndex + 1,
                ReservedAt = timestamp,
                Portfolio_Id = config.Portfolio_Id
            };

            context.ProjectReservations.Add(reservation);
            // TODO: save!!!

            return reservation;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}