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
    public class ProjectProvider : IDisposable
    {
        internal PortfolioContext Context;
        private string projectId;
        public ProjectProvider(string projectId)
        {
            this.Context = new PortfolioContext();
            this.projectId = projectId;
        }

        public async Task<ProjectReservation> GetProjectReservationAsync()
        {
            return await Context.ProjectReservations
                .ProjectIncludes()
                .ProjectUpdateIncludes()
                .ConfigIncludes()
                .SingleOrDefaultAsync(r => r.ProjectId == projectId);
        }

        internal Project CreateNewProject(ProjectReservation reservation)
        {
            reservation.Project = new Project()
            {
                Reservation = reservation,
                Updates = new List<ProjectUpdateItem>(),
                Portfolios = new List<Portfolio>() { reservation.Portfolio }
            };
            return reservation.Project;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        internal Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        internal void LogAuditChanges(Project project)
        {
            // Record changes
            AuditProvider.LogChanges(
                Context,
                (ts, txt) => auditLogFactory(ts, txt),
                project.AuditLogs,
                DateTime.Now);
        }

        private ProjectAuditLog auditLogFactory(DateTime timestamp, string text)
        {
            return new ProjectAuditLog()
            {
                Timestamp = timestamp,
                Text = text
            };
        }

    }
}