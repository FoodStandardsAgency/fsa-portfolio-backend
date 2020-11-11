using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Projects
{
    public class ProjectSearchFilters
    {
        private ProjectQueryModel searchTerms;
        private IQueryable<Project> filteredQuery;

        internal IQueryable<Project> Query => filteredQuery;

        public ProjectSearchFilters(ProjectQueryModel searchTerms, IQueryable<Project> filteredQuery)
        {
            this.searchTerms = searchTerms;
            this.filteredQuery = filteredQuery;
        }

        internal void BuildFilters()
        {
            if (!string.IsNullOrWhiteSpace(searchTerms.Name))
            {
                filteredQuery = filteredQuery.Where(p => p.Name.Contains(searchTerms.Name) || p.Reservation.ProjectId.Contains(searchTerms.Name));
            }
            filteredQuery = AddExactMatchFilter(searchTerms.Themes, filteredQuery, p => searchTerms.Themes.Contains(p.Theme));
            filteredQuery = AddExactMatchFilter(searchTerms.ProjectTypes, filteredQuery, p => searchTerms.ProjectTypes.Contains(p.ProjectType));
            filteredQuery = AddExactMatchFilter(searchTerms.Categories, filteredQuery, p => searchTerms.Categories.Contains(p.Category.ViewKey) || searchTerms.Categories.Intersect(p.Subcategories.Select(sc => sc.ViewKey)).Any());
            filteredQuery = AddExactMatchFilter(searchTerms.Directorates, filteredQuery, p => searchTerms.Directorates.Contains(p.Directorate));
            filteredQuery = AddExactMatchFilter(searchTerms.StrategicObjectives, filteredQuery, p => searchTerms.StrategicObjectives.Contains(p.StrategicObjectives));
            filteredQuery = AddExactMatchFilter(searchTerms.Programmes, filteredQuery, p => searchTerms.Programmes.Contains(p.Programme));

            // Project team
            // TODO: add supplier here also (startswith)
            // TODO: add team filter (multiselect for team filter - g6team
            // TODO: switch priority filter to priority group
            if (!string.IsNullOrWhiteSpace(searchTerms.TeamMemberName))
            {
                searchTerms.TeamMemberName = searchTerms.TeamMemberName.ToLower();
                filteredQuery = filteredQuery.Where(p =>
                    p.Lead.Firstname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.Lead.Surname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.Lead.Email.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact1.Firstname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact1.Surname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact1.Email.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact2.Firstname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact2.Surname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact2.Email.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact3.Firstname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact3.Surname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact3.Email.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.Team.Any(t =>
                        t.Firstname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                        t.Surname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                        t.Email.ToLower().StartsWith(searchTerms.TeamMemberName))
                    );
            }
            // Project lead search
            if (!string.IsNullOrWhiteSpace(searchTerms.ProjectLeadName))
            {
                searchTerms.ProjectLeadName = searchTerms.ProjectLeadName.ToLower();
                filteredQuery = filteredQuery.Where(p =>
                    p.Lead.Firstname.ToLower().StartsWith(searchTerms.ProjectLeadName) ||
                    p.Lead.Surname.ToLower().StartsWith(searchTerms.ProjectLeadName) ||
                    p.Lead.Email.ToLower().StartsWith(searchTerms.ProjectLeadName)
                    );
            }

            // Progress
            filteredQuery = AddExactMatchFilter(searchTerms.Phases, filteredQuery, p => searchTerms.Phases.Contains(p.LatestUpdate.Phase.ViewKey));
            filteredQuery = AddExactMatchFilter(searchTerms.RAGStatuses, filteredQuery, p => searchTerms.RAGStatuses.Contains(p.LatestUpdate.RAGStatus.ViewKey));
            filteredQuery = AddExactMatchFilter(searchTerms.OnHoldStatuses, filteredQuery, p => searchTerms.OnHoldStatuses.Contains(p.LatestUpdate.OnHoldStatus.ViewKey));

            // Prioritisation
            if (searchTerms.Priorities != null && searchTerms.Priorities.Length > 0)
            {
                filteredQuery = filteredQuery.Where(p => p.Priority.HasValue && searchTerms.Priorities.Contains(p.Priority.Value));
            }

            // Updates
            filteredQuery = AddExactMatchFilter(searchTerms.LastUpdateBefore, filteredQuery, p => p.LatestUpdate.Timestamp < searchTerms.LastUpdateBefore.Value);
            filteredQuery = AddExactMatchFilter(searchTerms.NoUpdates, filteredQuery, p => p.Updates.Any(u => u.Text != null && u.Text != string.Empty) != searchTerms.NoUpdates.Value);

            // Key dates
            filteredQuery = AddExactMatchFilter(searchTerms.PastStartDate, filteredQuery,
                p => searchTerms.PastStartDate.Value ==
                ((p.StartDate > DateTime.Today && p.ActualStartDate == null) || p.ActualStartDate > p.StartDate));

            filteredQuery = AddExactMatchFilter(searchTerms.MissedEndDate, filteredQuery,
                p => searchTerms.MissedEndDate.Value ==
                (((!p.ActualEndDate.HasValue) && p.ExpectedEndDate < DateTime.Today)
                ||
                (p.ActualEndDate > p.ExpectedEndDate)
                ||
                (!p.ActualEndDate.HasValue && p.HardEndDate < DateTime.Today)
                ||
                (p.ActualEndDate > p.HardEndDate)
                ||
                (p.LatestUpdate.ExpectedCurrentPhaseEnd.HasValue && p.LatestUpdate.ExpectedCurrentPhaseEnd < DateTime.Today))
                );
        }
        private IQueryable<Project> AddExactMatchFilter(string[] terms, IQueryable<Project> query, Expression<Func<Project, bool>> filter)
        {
            return (terms != null && terms.Length > 0) ? query.Where(filter) : query;
        }
        private IQueryable<Project> AddExactMatchFilter(DateTime? term, IQueryable<Project> query, Expression<Func<Project, bool>> filter)
        {
            return term.HasValue ? query.Where(filter) : query;
        }
        private IQueryable<Project> AddExactMatchFilter(bool? term, IQueryable<Project> query, Expression<Func<Project, bool>> filter)
        {
            return term.HasValue ? query.Where(filter) : query;
        }

    }
}