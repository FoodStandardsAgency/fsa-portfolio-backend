using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Projects
{
    public class ProjectIdReservationProvider : IDisposable
    {
        private PortfolioContext context;
        private string portfolioViewKey;
        public ProjectIdReservationProvider(string portfolioViewKey)
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

        public async Task<ProjectReservation> GetProjectReservationAsync(Portfolio portfolio)
        {
            var timestamp = DateTime.Now;
            int year = timestamp.Year;
            int month = timestamp.Month;
            int maxIndex = await context.ProjectReservations
                .Where(r => r.Portfolio.Id == portfolio.Id && r.Year == year && r.Month == month)
                .Select(r => r.Index)
                .DefaultIfEmpty()
                .MaxAsync();

            var reservation = new ProjectReservation()
            {
                Year = year,
                Month = month,
                Index = maxIndex + 1,
                ReservedAt = timestamp,
                Portfolio_Id = portfolio.Id
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