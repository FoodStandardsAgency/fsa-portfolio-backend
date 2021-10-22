﻿using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSAPortfolio.Common;

namespace FSAPortfolio.WebAPI.App.Mapping.Organisation.Resolvers.Summaries
{
    public class PortfolioPersonResolver : IValueResolver<Portfolio, PortfolioSummaryModel, string>
    {
        public const string PersonKey = nameof(PortfolioSummaryModel.Person);
        public string Resolve(Portfolio source, PortfolioSummaryModel destination, string destMember, ResolutionContext context)
        {
            string result = null;
            if (context.Items.ContainsKey(PersonKey))
            {
                string userName = (string)context.Items[PersonKey];
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
                    var person = portfolioContext.People.SingleOrDefault(p => p.ActiveDirectoryPrincipalName == userName);
                    if (person != null) result = person.DisplayName;
                }
            }
            return result;   
        }
    }

    public class ProjectCountByPhaseResolver : IValueResolver<ProjectPhase, PhaseSummaryModel, int>
    {
        public int Resolve(ProjectPhase source, PhaseSummaryModel destination, int destMember, ResolutionContext context)
        {
            var summaryType = context.Items[PortfolioSummaryResolver.SummaryTypeKey] as string;
            switch (summaryType)
            {
                case PortfolioSummaryModel.NewProjectsByTeam:
                    var newCutoff = DateTime.Now.AddDays(-PortfolioSettings.NewProjectLimitDays);
                    return source.Configuration.Portfolio.Projects.Count(p => p.LatestUpdate?.Phase?.Id == source.Id && p.FirstUpdate.Timestamp > newCutoff);
                default:
                    return source.Configuration.Portfolio.Projects.Count(p => p.LatestUpdate?.Phase?.Id == source.Id);
            }
        }
    }

    public class PortfolioSummaryResolver : IValueResolver<Portfolio, PortfolioSummaryModel, IEnumerable<ProjectSummaryModel>>
    {
        public static string SummaryTypeKey = $"{nameof(PortfolioSummaryResolver)}.SummaryTypeKey";
        public IEnumerable<ProjectSummaryModel> Resolve(Portfolio source, PortfolioSummaryModel destination, IEnumerable<ProjectSummaryModel> destMember, ResolutionContext context)
        {
            IEnumerable<ProjectSummaryModel> result = null;
            if (context.Items.ContainsKey(SummaryTypeKey))
            {
                var summaryType = context.Items[SummaryTypeKey] as string;
                switch (summaryType)
                {
                    case PortfolioSummaryModel.ByCategory:
                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.Categories.OrderBy(c => c.Name));
                        break;
                    case PortfolioSummaryModel.ByPriorityGroup:
                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.PriorityGroups.OrderBy(c => c.Order));
                        break;
                    case PortfolioSummaryModel.ByRagStatus:
                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.RAGStatuses.OrderBy(c => c.Order));
                        break;
                    case PortfolioSummaryModel.ByPhase:
                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.Phases.Where(p => p.Id != source.Configuration.CompletedPhase.Id).OrderBy(c => c.Order));
                        break;
                    case PortfolioSummaryModel.ByLead:

                        var people = source.Configuration.Portfolio.Projects
                            .Where(p => p.Lead_Id != null)
                            .Select(p => p.Lead)
                            .Distinct()
                            .OrderBy(p => p.Firstname)
                            .ThenBy(p => p.Surname)
                            .ToList();
                        people.Insert(0, new Person() { Id = 0, ActiveDirectoryDisplayName = ProjectTeamConstants.NotSetName });

                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(people);

                        result = RemoveEmptySummaries(result);

                        break;
                    case PortfolioSummaryModel.ByTeam:
                    case PortfolioSummaryModel.NewProjectsByTeam:

                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(
                            source.Teams.OrderBy(t => t.Order)
                            .Union(new Team[] { new Team() { Name = ProjectTeamConstants.NotSetName, Id = 0 } }));

                        result = RemoveEmptySummaries(result);
                        
                        break;
                    case PortfolioSummaryModel.ByUser:

                        result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(ProjectUserCategory.All(source.Configuration.Labels));

                        result = RemoveEmptySummaries(result); 

                        break;
                    default:
                        throw new ArgumentException($"Unrecognised summary type: {summaryType}");
                }
            }
            return result;
        }

        private IEnumerable<ProjectSummaryModel> RemoveEmptySummaries(IEnumerable<ProjectSummaryModel> summaries)
        {
            return summaries.Where(u => u.PhaseProjects.Any(pp => pp.Projects.Count() > 0));
        }
    }

    internal static class SummaryLinqQuery
    {
        public static IEnumerable<PhaseProjectsModel> GetQuery(PortfolioConfiguration config, Func<Project, bool> projectPredicate, ResolutionContext context)
        {
            var q = from ph in config.Phases.Where(p => p.Id != config.CompletedPhase.Id) // Non-completed phases...
                    join pr in config.Portfolio.Projects
                        .Where(p => p.LatestUpdate?.Phase != null)
                        .Where(projectPredicate)
                        on ph.Id equals pr.LatestUpdate.Phase.Id into projects // ... get projects joined to each phase ...
                    from pr in projects.DefaultIfEmpty() // ... need to get all phases ...
                    group pr by ph into phaseGroup // ... group projects by phase ...
                    select new PhaseProjectsModel()
                    {
                        ViewKey = phaseGroup.Key.ViewKey,
                        Order = phaseGroup.Key.Order,
                        Projects = context.Mapper.Map<IEnumerable<ProjectIndexModel>>(phaseGroup.Where(p => p != null).OrderByDescending(p => p.Priority))
                    };
            return q.OrderBy(p => p.Order);
        }
    }

    public class PhaseProjectsByPriorityGroupResolver : IValueResolver<PriorityGroup, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(PriorityGroup source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.PriorityGroup.ViewKey == source.ViewKey, context);
        }
    }

    public class PhaseProjectsByCategoryResolver : IValueResolver<ProjectCategory, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectCategory source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.ProjectCategory_Id == source.Id, context);
        }
    }

    public class PhaseProjectsByUserCategoryResolver : IValueResolver<ProjectUserCategory, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectUserCategory source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            var config = context.Items[nameof(PortfolioConfiguration)] as PortfolioConfiguration;
            var user = context.Items[PortfolioPersonResolver.PersonKey] as string;
            return SummaryLinqQuery.GetQuery(config, p => p.GetUserCategory(user) == source.CategoryType, context);
        }
    }

    public class PhaseProjectsByRAGResolver : IValueResolver<ProjectRAGStatus, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectRAGStatus source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.LatestUpdate.RAGStatus != null && p.LatestUpdate.RAGStatus.Id == source.Id, context);
        }
    }

    public class PhaseProjectsByPhaseResolver : IValueResolver<ProjectPhase, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectPhase source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.LatestUpdate.Phase.Id == source.Id, context);
        }
    }

    public class PhaseProjectsByTeamResolver : IValueResolver<Team, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(Team source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            var portfolioConfiguration = context.Items[nameof(PortfolioConfiguration)] as PortfolioConfiguration;
            var summaryType = context.Items[PortfolioSummaryResolver.SummaryTypeKey] as string;
            IEnumerable<PhaseProjectsModel> result;

            switch(summaryType)
            {
                case PortfolioSummaryModel.NewProjectsByTeam:
                    var newCutoff = DateTime.Now.AddDays(-PortfolioSettings.NewProjectLimitDays);
                    result = SummaryLinqQuery.GetQuery(portfolioConfiguration, p => 
                        (
                            (p.Lead?.Team != null && p.Lead.Team.ViewKey == source.ViewKey) ||
                            (p.Lead?.Team == null && source.Id == 0)) // No team set
                        && p.FirstUpdate.Timestamp > newCutoff, context);
                    break;
                case PortfolioSummaryModel.ByTeam:
                default:
                    result = SummaryLinqQuery.GetQuery(portfolioConfiguration, p => 
                        (p.Lead?.Team != null && p.Lead.Team.ViewKey == source.ViewKey) ||
                        (p.Lead?.Team == null && source.Id == 0) // No team set
                        , context);
                    break;
            }

            return result;
        }
    }

    public class PhaseProjectsByTeamLeadResolver : IValueResolver<Person, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(Person source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            var config = context.Items[nameof(PortfolioConfiguration)] as PortfolioConfiguration;
            return SummaryLinqQuery.GetQuery(config, p => source.Id == 0 ? !p.Lead_Id.HasValue : p.Lead_Id == source.Id, context);
        }
    }

    public class ProjectSummaryLabelResolver : IValueResolver<Portfolio, PortfolioSummaryModel, IEnumerable<PortfolioLabelModel>>
    {
        private static string[] summaryFields = new string[] {
                ProjectPropertyConstants.category,
                ProjectPropertyConstants.pgroup,
                ProjectPropertyConstants.g6team,
                ProjectPropertyConstants.ProjectLead,
                ProjectPropertyConstants.rag,
                ProjectPropertyConstants.phase,
                ProjectPropertyConstants.project_type
            };
        public IEnumerable<PortfolioLabelModel> Resolve(Portfolio source, PortfolioSummaryModel destination, IEnumerable<PortfolioLabelModel> destMember, ResolutionContext context)
        {
            var summaryLabels = source.Configuration.Labels.Where(l => summaryFields.Contains(l.FieldName));
            return context.Mapper.Map<IEnumerable<PortfolioLabelModel>>(summaryLabels);
        }
    }

    public class ProjectSummaryProjectTypeResolver : IValueResolver<Portfolio, PortfolioSummaryModel, List<DropDownItemModel>>
    {
        public List<DropDownItemModel> Resolve(Portfolio source, PortfolioSummaryModel destination, List<DropDownItemModel> destMember, ResolutionContext context)
        {
            List<DropDownItemModel> options = null;
            var label = source.Configuration.Labels.SingleOrDefault(l => l.FieldName == ProjectPropertyConstants.project_type);
            if (label != null)
            {
                options = (label.FieldOptions ?? string.Empty).Split(',').Select((o, i) =>
                {
                    var ot = o.Trim();
                    return new DropDownItemModel()
                    {
                        Order = i + 1,
                        Display = ot,
                        Value = ot
                    };
                }).ToList();
                options.Insert(0, new DropDownItemModel() { Order = 0, Display = "All projects" });
            }
            return options;
        }
    }

    public class ProjectIndexDateResolver : IValueResolver<Project, ProjectIndexModel, ProjectDateIndexModel>
    {
        public const string OptionKey = nameof(ProjectIndexDateResolver);
        public ProjectDateIndexModel Resolve(Project source, ProjectIndexModel destination, ProjectDateIndexModel destMember, ResolutionContext context)
        {
            ProjectDateIndexModel result = null;

            if (context.Items.ContainsKey(OptionKey) && (bool)context.Items[OptionKey])
            {
                result = new ProjectDateIndexModel();
                if (source.LatestUpdate.Phase.ViewKey == PhaseConstants.BacklogViewKey)
                {
                    // Backlog: use intended start date
                    result.Label = "Start date";
                    result.Value = context.Mapper.Map<ProjectDateViewModel>(source.StartDate);
                }
                else if (source.LatestUpdate.Phase.ViewKey == PhaseConstants.ArchiveViewKey || source.LatestUpdate.Phase.ViewKey == PhaseConstants.CompletedViewKey)
                {
                    // Archive/Completed: use actual end date with custom label
                    result.Label = "Completed";
                    result.Value = context.Mapper.Map<ProjectDateViewModel>(source.ActualEndDate);
                }
                else
                {
                    // Use current phase expected end
                    result.Label = "Deadline";
                    result.Value = context.Mapper.Map<ProjectDateViewModel>(source.LatestUpdate.ExpectedCurrentPhaseEnd);
                }
            }
            return result;
        }
    }

    public class ProjectIndexPriorityResolver : IValueResolver<Project, ProjectIndexModel, ProjectPriorityIndexModel>
    {
        public const string OptionKey = nameof(ProjectIndexPriorityResolver);

        public ProjectPriorityIndexModel Resolve(Project source, ProjectIndexModel destination, ProjectPriorityIndexModel destMember, ResolutionContext context)
        {
            ProjectPriorityIndexModel result = null;
            if (context.Items.ContainsKey(OptionKey) && (bool)context.Items[OptionKey])
            {
                result = new ProjectPriorityIndexModel()
                {
                    Value = source.PriorityGroup.Name,
                    Label = "Priority"
                };
            }
            return result;
        }
    }

    public class ProjectActionsResolver : IValueResolver<Project, object, ProjectActionsModel>
    {
        private static Dictionary<ProjectActionItemModel.ActionItemType, string> actionTypeMap 
            = new Dictionary<ProjectActionItemModel.ActionItemType, string>() 
            {
                { ProjectActionItemModel.ActionItemType.Update, "Updates" },
                { ProjectActionItemModel.ActionItemType.Date, "Dates" }
            };

        public ProjectActionsModel Resolve(Project source, object destination, ProjectActionsModel destMember, ResolutionContext context)
        {
            ProjectActionsModel result = null;
            List<ProjectActionItemModel> actions = new List<ProjectActionItemModel>();

            Func<string[], bool> isInPhases = (a) => { return a.Contains(source.LatestUpdate.Phase.ViewKey); };

            if(isInPhases(PortfolioSettings.ProjectActivePhaseViewKeys))
            {
                // Check if updates are overdue
                if ((DateTime.Now - source.LatestUpdate.Timestamp).TotalDays > PortfolioSettings.ProjectUpdateOverdueDays)
                {
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Update,
                        Action = "An update is overdue"
                    });
                }

                // Check if phase end date is in past (ignore if not set)
                var phaseEnd = source.LatestUpdate.ExpectedCurrentPhaseEnd.Date;
                if (phaseEnd.HasValue && phaseEnd.Value < DateTime.Now)
                {
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = "The current phase end date has expired"
                    });
                }

                // Check if end dates are empty or have expired 
                var endDate = source.ExpectedEndDate.Date;
                if (!endDate.HasValue)
                {
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = "The project end date is missing"
                    });
                }
                else if (endDate.Value < DateTime.Now)
                {
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = "The project completion date has expired"
                    });
                }

                // Check if hard deadline expired 
                var deadline = source.HardEndDate.Date;
                if (deadline.HasValue && deadline.Value < DateTime.Now)
                {
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = "The project hard deadline has expired"
                    });
                }
            }

            // Check if start dates have expired
            if (isInPhases(PortfolioSettings.ProjectBacklogPhaseViewKeys))
            {
                // Get the latest out of the two start dates
                var date = source.StartDate.Date.HasValue ?
                        (source.ActualStartDate.Date.HasValue ?
                            (source.StartDate.Date.Value > source.ActualStartDate.Date.Value ?
                                source.StartDate.Date
                                : source.ActualStartDate.Date)
                            : source.StartDate.Date)
                    : source.ActualStartDate.Date;

                if(date.HasValue && date.Value < DateTime.Now)
                {
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = "The project start date has expired"
                    });
                }
                else
                {
                    // No start date provided
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = "The project has no start date"
                    });
                }
            }

            // Build the model
            if(actions.Count > 0)
            {
                result = new ProjectActionsModel() { ActionItems = actions };
                var types = actions.Select(a => a.ActionType).Distinct().OrderBy(t => t).Select(t => actionTypeMap[t]).ToArray();
                switch(types.Length)
                {
                    case 1:
                        result.Summary = types[0];
                        break;
                    default:
                        result.Summary = string.Join(", ", types, 0, types.Length - 1);
                        result.Summary += " and " + types.Last();
                        break;
                }

            }
            return result;
        }
    }
}