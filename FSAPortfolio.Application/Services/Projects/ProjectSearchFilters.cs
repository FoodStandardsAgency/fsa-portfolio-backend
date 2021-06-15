using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LinqKit;

namespace FSAPortfolio.WebAPI.App.Projects
{
    public class ProjectSearchFilters
    {
        private ProjectQueryModel searchTerms;

        public IQueryable<Project> Query { get; private set; }

        private PortfolioConfiguration config;

        public ProjectSearchFilters(ProjectQueryModel searchTerms, IQueryable<Project> filteredQuery, PortfolioConfiguration config)
        {
            this.searchTerms = searchTerms;
            this.Query = filteredQuery;
            this.config = config;
        }

        public void BuildFilters()
        {
            // Project title: project_name, project_id
            if (!string.IsNullOrWhiteSpace(searchTerms.Name))
            {
                Query = Query.Where(p => p.Name.Contains(searchTerms.Name) || p.Reservation.ProjectId.Contains(searchTerms.Name));
            }

            Query = AddExactMatchFilter(searchTerms.RiskRatings, Query, p => searchTerms.RiskRatings.Contains(p.RiskRating));
            Query = AddExactMatchFilter(searchTerms.Themes, Query, p => searchTerms.Themes.Contains(p.Theme));
            Query = AddExactMatchFilter(searchTerms.ProjectTypes, Query, p => searchTerms.ProjectTypes.Contains(p.ProjectType));
            Query = AddExactMatchFilter(searchTerms.ProjectSizes, Query, p => searchTerms.ProjectSizes.Contains(p.Size.ViewKey));
            Query = AddExactMatchFilter(searchTerms.Categories, Query, p => searchTerms.Categories.Contains(p.Category.ViewKey) || searchTerms.Categories.Intersect(p.Subcategories.Select(sc => sc.ViewKey)).Any());
            Query = AddExactMatchFilter(searchTerms.Directorates, Query, p => searchTerms.Directorates.Contains(p.Directorate.ViewKey));
            Query = AddExactMatchFilter(searchTerms.StrategicObjectives, Query, p => searchTerms.StrategicObjectives.Contains(p.StrategicObjectives));
            Query = AddExactMatchFilter(searchTerms.Programmes, Query, p => searchTerms.Programmes.Contains(p.Programme));

            // Budget search terms
            Query = AddExactMatchFilter(searchTerms.BudgetTypes, Query, p => searchTerms.BudgetTypes.Contains(p.BudgetType.ViewKey));

            if (searchTerms.BudgetOptions1 != null && searchTerms.BudgetOptions1.Length > 0)
            {
                var predicate = PredicateBuilder.New<Project>();
                foreach (var option in searchTerms.BudgetOptions1)
                {
                    predicate = predicate.Or(p => p.BudgetSettings.Option1 == option);
                }
                Query = Query.Where(predicate);
            }

            if (searchTerms.BudgetOptions2 != null && searchTerms.BudgetOptions2.Length > 0)
            {
                var predicate = PredicateBuilder.New<Project>();
                foreach (var option in searchTerms.BudgetOptions2)
                {
                    predicate = predicate.Or(p => p.BudgetSettings.Option2 == option);
                }
                Query = Query.Where(predicate);
            }

            // FSA Process
            if (searchTerms.ProcessOptions1 != null && searchTerms.ProcessOptions1.Length > 0)
            {
                var predicate = PredicateBuilder.New<Project>();
                foreach (var option in searchTerms.ProcessOptions1)
                {
                    predicate = predicate.Or(p => p.ProcessSettings.Option1 == option);
                }
                Query = Query.Where(predicate);
            }

            if (searchTerms.ProcessOptions2 != null && searchTerms.ProcessOptions2.Length > 0)
            {
                var predicate = PredicateBuilder.New<Project>();
                foreach (var option in searchTerms.ProcessOptions2)
                {
                    predicate = predicate.Or(p => p.ProcessSettings.Option2 == option);
                }
                Query = Query.Where(predicate);
            }


            // Project team
            Query = AddExactMatchFilter(searchTerms.Teams, Query, p => searchTerms.Teams.Contains(p.Lead.Team.ViewKey));

            if (!string.IsNullOrWhiteSpace(searchTerms.TeamMemberName))
            {
                searchTerms.TeamMemberName = searchTerms.TeamMemberName.ToLower();
                Query = Query.Where(p =>
                    (p.Lead.Firstname + " " + p.Lead.Surname).ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.Lead.Email.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    (p.KeyContact1.Firstname + " " + p.KeyContact1.Surname).ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact1.Email.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    (p.KeyContact2.Firstname + " " + p.KeyContact2.Surname).ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact2.Email.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    (p.KeyContact3.Firstname + " " + p.KeyContact3.Surname).ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.KeyContact3.Email.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.Supplier.ToLower().StartsWith(searchTerms.TeamMemberName) ||
                    p.People.Any(t =>
                        (t.Firstname + " " + t.Surname).ToLower().StartsWith(searchTerms.TeamMemberName) ||
                        t.Email.ToLower().StartsWith(searchTerms.TeamMemberName))
                    );
            }

            // Project lead search
            // TODO: split search term by spaces? Currently, searching for "surname forename" won't match anything.
            if (!string.IsNullOrWhiteSpace(searchTerms.ProjectLeadName))
            {
                searchTerms.ProjectLeadName = searchTerms.ProjectLeadName.ToLower();
                Query = Query.Where(p =>
                    (p.Lead.Firstname + " " + p.Lead.Surname).ToLower().StartsWith(searchTerms.ProjectLeadName) ||
                    p.Lead.Email.ToLower().StartsWith(searchTerms.ProjectLeadName)
                    );
            }

            // Progress
            Query = AddExactMatchFilter(searchTerms.Phases, Query, p => searchTerms.Phases.Contains(p.LatestUpdate.Phase.ViewKey));
            Query = AddExactMatchFilter(searchTerms.RAGStatuses, Query, p => searchTerms.RAGStatuses.Contains(p.LatestUpdate.RAGStatus.ViewKey));
            Query = AddExactMatchFilter(searchTerms.OnHoldStatuses, Query, p => searchTerms.OnHoldStatuses.Contains(p.LatestUpdate.OnHoldStatus.ViewKey));

            // Prioritisation
            if (searchTerms.PriorityGroups != null && searchTerms.PriorityGroups.Length > 0)
            {
                Expression<Func<Project, bool>> priExp = null;
                foreach (var priorityGroupViewKey in searchTerms.PriorityGroups)
                {
                    var priorityGroup = config.PriorityGroups.SingleOrDefault(pg => pg.ViewKey == priorityGroupViewKey);
                    if(priorityGroup != null)
                    {

                        // Check if is a ranged priority group or "none set"
                        Expression<Func<Project, bool>> clause;
                        if(priorityGroup.ViewKey == PriorityGroupConstants.NotSetViewKey)
                            clause = p => p.Priority == null;
                        else
                            clause = p => p.Priority >= priorityGroup.LowLimit && p.Priority <= priorityGroup.HighLimit;

                        // Combine the clause
                        if (priExp == null)
                            priExp = clause;
                        else
                            priExp = priExp.Or(clause);

                    }
                }
                if (priExp != null) Query = Query.Where(priExp);
            }

            // Updates
            Query = AddExactMatchFilter(searchTerms.LastUpdateBefore, Query, 
                p =>
                    p.LatestUpdate.Timestamp < searchTerms.LastUpdateBefore.Value
                );

            Query = AddExactMatchFilter(searchTerms.NoUpdates, Query, p => p.Updates.Any(u => u.Text != null && u.Text != string.Empty) != searchTerms.NoUpdates.Value);

            // Key dates
            Query = AddExactMatchFilter(searchTerms.PastStartDate, Query,
                p => searchTerms.PastStartDate.Value ==
                (
                    p.StartDate.Date != null &&
                    ((p.StartDate.Date < DateTime.Today && p.ActualStartDate.Date == null) || p.ActualStartDate.Date > p.StartDate.Date)
                ));

            Query = AddExactMatchFilter(searchTerms.MissedEndDate, Query,
                p => searchTerms.MissedEndDate.Value ==
                    (
                        ((!p.ActualEndDate.Date.HasValue) && p.ExpectedEndDate.Date < DateTime.Today)
                        ||
                        (p.ActualEndDate.Date > p.ExpectedEndDate.Date)
                        ||
                        ((!p.ActualEndDate.Date.HasValue) && p.HardEndDate.Date < DateTime.Today)
                        ||
                        (p.ActualEndDate.Date > p.HardEndDate.Date)
                    )
                );
        }

        private IQueryable<Project> AddTextFilter(string term, IQueryable<Project> query, Expression<Func<Project, bool>> filter)
        {
            return (!string.IsNullOrWhiteSpace(term)) ? query.Where(filter) : query;
        }

        private IQueryable<Project> AddExactMatchFilter(string[] terms, IQueryable<Project> query, Expression<Func<Project, bool>> filter)
        {
            return (terms != null && terms.Length > 0) ? query.Where(filter) : query;
        }
        private IQueryable<Project> AddExactMatchFilter(string term, IQueryable<Project> query, Expression<Func<Project, bool>> filter)
        {
            return (!string.IsNullOrWhiteSpace(term)) ? query.Where(filter) : query;
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