using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public static class ProjectQueryExtensions
    {
        public static IQueryable<ProjectReservation> ProjectIncludes(this IQueryable<ProjectReservation> query)
        {
            return query
                .Include(r => r.Project.ProjectData)
                .Include(r => r.Project.FirstUpdate.OnHoldStatus)
                .Include(r => r.Project.FirstUpdate.RAGStatus)
                .Include(r => r.Project.FirstUpdate.Phase)
                .Include(r => r.Project.LatestUpdate.OnHoldStatus)
                .Include(r => r.Project.LatestUpdate.RAGStatus)
                .Include(r => r.Project.LatestUpdate.Phase)
                .Include(r => r.Project.Category)
                .Include(r => r.Project.Subcategories)
                .Include(r => r.Project.Size)
                .Include(r => r.Project.BudgetType)
                .Include(r => r.Project.RelatedProjects)
                .Include(r => r.Project.DependantProjects)
                .Include(r => r.Project.Documents)
                .Include(r => r.Project.Lead)
                ;
        }
        public static IQueryable<ProjectReservation> ProjectUpdateIncludes(this IQueryable<ProjectReservation> query)
        {
            return query
                .Include(r => r.Project.AuditLogs)
                ;
        }
        public static IQueryable<ProjectReservation> ConfigIncludes(this IQueryable<ProjectReservation> query)
        {
            return query
                .Include(r => r.Portfolio.Configuration.Phases)
                .Include(r => r.Portfolio.Configuration.RAGStatuses)
                .Include(r => r.Portfolio.Configuration.OnHoldStatuses)
                .Include(r => r.Portfolio.Configuration.Categories)
                .Include(r => r.Portfolio.Configuration.ProjectSizes)
                .Include(r => r.Portfolio.Configuration.BudgetTypes)
                .Include(r => r.Portfolio.Configuration.Labels)
                .Include(r => r.Portfolio.Configuration.LabelGroups);
        }

        public static IQueryable<Project> IncludeProject(this IQueryable<Project> query)
        {
            return query
                .Include(p => p.Reservation.Portfolio.Configuration.CompletedPhase)
                .Include(p => p.ProjectData)
                .Include(p => p.FirstUpdate.OnHoldStatus)
                .Include(p => p.FirstUpdate.RAGStatus)
                .Include(p => p.FirstUpdate.Phase)
                .Include(p => p.LatestUpdate.OnHoldStatus)
                .Include(p => p.LatestUpdate.RAGStatus)
                .Include(p => p.LatestUpdate.Phase)
                .Include(p => p.Category)
                .Include(p => p.Subcategories)
                .Include(p => p.Size)
                .Include(p => p.BudgetType)
                .Include(p => p.RelatedProjects)
                .Include(p => p.DependantProjects)
                .Include(p => p.Documents)
                .Include(p => p.Lead);
        }
        public static IQueryable<Project> IncludeUpdates(this IQueryable<Project> query)
        {
            return query
                .Include(p => p.Updates.Select(u => u.OnHoldStatus))
                .Include(p => p.Updates.Select(u => u.RAGStatus))
                .Include(p => p.Updates.Select(u => u.Phase))
                ;
        }

        public static IQueryable<Project> IncludeLabelConfigs(this IQueryable<Project> query)
        {
            return query
                .Include(p => p.Reservation.Portfolio.Configuration.Labels)
                .Include(p => p.Reservation.Portfolio.Configuration.LabelGroups)
                ;
        }
        public static IQueryable<Project> FullConfigIncludes(this IQueryable<Project> query)
        {
            return query
                .Include(p => p.Reservation.Portfolio.Configuration.Labels)
                .Include(p => p.Reservation.Portfolio.Configuration.LabelGroups)
                .Include(p => p.Reservation.Portfolio.Configuration.BudgetTypes)
                .Include(p => p.Reservation.Portfolio.Configuration.Portfolio)
                .Include(p => p.Reservation.Portfolio.Configuration.Phases)
                .Include(p => p.Reservation.Portfolio.Configuration.RAGStatuses)
                .Include(p => p.Reservation.Portfolio.Configuration.OnHoldStatuses)
                .Include(p => p.Reservation.Portfolio.Configuration.Categories)
                .Include(p => p.Reservation.Portfolio.Configuration.ProjectSizes)
                .Include(p => p.Reservation.Portfolio.Configuration.BudgetTypes)
                ;
        }

    }
}