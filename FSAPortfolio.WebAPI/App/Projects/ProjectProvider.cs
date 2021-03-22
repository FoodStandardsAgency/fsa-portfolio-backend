using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.App.Projects
{
    public class ProjectProvider
    {
        internal PortfolioContext context;
        public ProjectProvider(PortfolioContext context)
        {
            this.context = context;
        }

        public async Task<ProjectReservation> GetProjectReservationAsync(string projectId)
        {
            return await context.ProjectReservations
                .ProjectIncludes()
                .ProjectUpdateIncludes()
                .ConfigIncludes()
                .Include(r => r.Portfolio.Teams)
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

        internal async Task UpdateProject(ProjectUpdateModel update, PersonProvider userProvider, ProjectReservation reservation = null, Action<Portfolio> permissionCallback = null)
        {
            try
            {
                if (reservation == null) reservation = await GetProjectReservationAsync(update.project_id);
                if (reservation == null) throw new HttpResponseException(HttpStatusCode.NotFound);
                else
                {
                    if(permissionCallback != null) permissionCallback(reservation.Portfolio);
                    var project = reservation?.Project;
                    if (project == null)
                    {
                        project = CreateNewProject(reservation);
                    }

                    // Map the model to the project - map the leads manually because they can require an async AD lookup
                    PortfolioMapper.ProjectMapper.Map(update, project, opt =>
                    {
                        opt.Items[nameof(PortfolioContext)] = context;
                    });
                    await userProvider.MapPeopleAsync(update, project);

                    // Audit and save
                    if (project.AuditLogs != null) LogAuditChanges(project);
                    await context.SaveChangesAsync();

                    // Get the last update and create a new one if necessary
                    ProjectUpdateItem lastUpdate = project.LatestUpdate;
                    ProjectUpdateItem projectUpdate = lastUpdate;
                    if (projectUpdate == null || projectUpdate.Timestamp.Date != DateTime.Today)
                    {
                        // Create a new update
                        projectUpdate = new ProjectUpdateItem() { Project = project };
                        if (project.FirstUpdate_Id == null)
                        {
                            project.FirstUpdate = projectUpdate;
                        }
                    }

                    // Map the data to the update and add if not a duplicate
                    PortfolioMapper.ProjectMapper.Map(update, projectUpdate, opt => opt.Items[nameof(PortfolioContext)] = context);
                    if (!projectUpdate.IsDuplicate(lastUpdate))
                    {
                        project.Updates.Add(projectUpdate);
                        project.LatestUpdate = projectUpdate;
                        project.LatestUpdate.Timestamp = DateTime.Now;
                    }

                    // Save
                    await context.SaveChangesAsync();
                }
            }
            catch (AutoMapperMappingException ame)
            {
                if (ame.InnerException is Config.PortfolioConfigurationException)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = ame.InnerException.Message
                    };
                    throw new HttpResponseException(resp);
                }
                else throw ame;
            }
            catch (DbEntityValidationException e)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                if (e.EntityValidationErrors != null && e.EntityValidationErrors.Count() > 0)
                {
                    resp.ReasonPhrase = string.Join(", ", e.EntityValidationErrors.SelectMany(err => err.ValidationErrors).Select(ve => ve.ErrorMessage));
                }
                else
                {
                    resp.ReasonPhrase = e.Message;
                }

                throw new HttpResponseException(resp);
            }
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