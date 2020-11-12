using FSAPortfolio.Entities;
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

        internal IQueryable<Project> Query { get; private set; }

        public ProjectSearchFilters(ProjectQueryModel searchTerms, IQueryable<Project> filteredQuery)
        {
            this.searchTerms = searchTerms;
            this.Query = filteredQuery;
        }

        internal void BuildFilters()
        {
            if (!string.IsNullOrWhiteSpace(searchTerms.Name))
            {
                Query = Query.Where(p => p.Name.Contains(searchTerms.Name) || p.Reservation.ProjectId.Contains(searchTerms.Name));
            }
            Query = AddExactMatchFilter(searchTerms.Themes, Query, p => searchTerms.Themes.Contains(p.Theme));
            Query = AddExactMatchFilter(searchTerms.ProjectTypes, Query, p => searchTerms.ProjectTypes.Contains(p.ProjectType));
            Query = AddExactMatchFilter(searchTerms.Categories, Query, p => searchTerms.Categories.Contains(p.Category.ViewKey) || searchTerms.Categories.Intersect(p.Subcategories.Select(sc => sc.ViewKey)).Any());
            Query = AddExactMatchFilter(searchTerms.Directorates, Query, p => searchTerms.Directorates.Contains(p.Directorate.ViewKey));
            Query = AddExactMatchFilter(searchTerms.StrategicObjectives, Query, p => searchTerms.StrategicObjectives.Contains(p.StrategicObjectives));
            Query = AddExactMatchFilter(searchTerms.Programmes, Query, p => searchTerms.Programmes.Contains(p.Programme));
            Query = AddExactMatchFilter(searchTerms.Teams, Query, p => searchTerms.Teams.Contains(p.Lead.Team.ViewKey));

            // Project team
            // TODO: add supplier here also (startswith)
            // TODO: add team filter (multiselect for team filter - g6team
            // TODO: switch priority filter to priority group
            if (!string.IsNullOrWhiteSpace(searchTerms.TeamMemberName))
            {
                searchTerms.TeamMemberName = searchTerms.TeamMemberName.ToLower();
                Query = Query.Where(p =>
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
                    p.People.Any(t =>
                        t.Firstname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                        t.Surname.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                        t.Email.ToLower().StartsWith(searchTerms.TeamMemberName))
                    );
            }

            // Project lead search
            if (!string.IsNullOrWhiteSpace(searchTerms.ProjectLeadName))
            {
                searchTerms.ProjectLeadName = searchTerms.ProjectLeadName.ToLower();
                Query = Query.Where(p =>
                    p.Lead.Firstname.ToLower().StartsWith(searchTerms.ProjectLeadName) ||
                    p.Lead.Surname.ToLower().StartsWith(searchTerms.ProjectLeadName) ||
                    p.Lead.Email.ToLower().StartsWith(searchTerms.ProjectLeadName)
                    );
            }

            // Progress
            Query = AddExactMatchFilter(searchTerms.Phases, Query, p => searchTerms.Phases.Contains(p.LatestUpdate.Phase.ViewKey));
            Query = AddExactMatchFilter(searchTerms.RAGStatuses, Query, p => searchTerms.RAGStatuses.Contains(p.LatestUpdate.RAGStatus.ViewKey));
            Query = AddExactMatchFilter(searchTerms.OnHoldStatuses, Query, p => searchTerms.OnHoldStatuses.Contains(p.LatestUpdate.OnHoldStatus.ViewKey));

            // Prioritisation
            if (searchTerms.Priorities != null && searchTerms.Priorities.Length > 0)
            {
                Query = Query.Where(p => p.Priority.HasValue && searchTerms.Priorities.Contains(p.Priority.Value));
            }

            // Updates
            Query = AddExactMatchFilter(searchTerms.LastUpdateBefore, Query, p => p.LatestUpdate.Timestamp < searchTerms.LastUpdateBefore.Value);
            Query = AddExactMatchFilter(searchTerms.NoUpdates, Query, p => p.Updates.Any(u => u.Text != null && u.Text != string.Empty) != searchTerms.NoUpdates.Value);

            // Key dates
            // TODO: ODD exludes backlog from this filter
            Query = AddExactMatchFilter(searchTerms.PastStartDate, Query,
                p => searchTerms.PastStartDate.Value ==
                (
                    p.StartDate != null &&
                    ((p.StartDate < DateTime.Today && p.ActualStartDate == null) || p.ActualStartDate > p.StartDate)
                ));

            // TODO: ODD also excludes LIVE phase in this filter
            Query = AddExactMatchFilter(searchTerms.MissedEndDate, Query,
                p => searchTerms.MissedEndDate.Value ==
                (p.LatestUpdate.Phase.Id != p.Reservation.Portfolio.Configuration.CompletedPhase.Id && 
                    (
                        ((!p.ActualEndDate.HasValue) && p.ExpectedEndDate < DateTime.Today)
                        ||
                        (p.ActualEndDate > p.ExpectedEndDate)
                        ||
                        (!p.ActualEndDate.HasValue && p.HardEndDate < DateTime.Today)
                        ||
                        (p.ActualEndDate > p.HardEndDate)
                    )
                ));
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