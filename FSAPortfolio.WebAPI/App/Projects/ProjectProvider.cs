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
    public class ProjectProvider
    {
        internal PortfolioContext context;
        private string projectId;
        public ProjectProvider(PortfolioContext context, string projectId)
        {
            this.context = context;
            this.projectId = projectId;
        }

        public async Task<ProjectReservation> GetProjectReservationAsync()
        {
            return await context.ProjectReservations
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

        internal void LogAuditChanges(Project project)
        {
            // Record changes
            AuditProvider.LogChanges(
                context,
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