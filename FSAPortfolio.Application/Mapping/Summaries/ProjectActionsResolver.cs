using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FSAPortfolio.WebAPI.App.Mapping.Organisation.Resolvers.Summaries
{
    public class ProjectActionsResolver : IValueResolver<Project, object, ProjectActionsModel>
    {
        public const string UpdateOverdueAction = "An update is overdue";
        public const string ProjectEndDateMissingAction = "The project end date is missing";
        public const string StartDateExpiredAction = "The project start date has expired";
        public const string NoStartDateAction = "The project has no start date";
        public const string PhaseCompletionExpiredAction = "The current phase end date has expired";
        public const string ProjectCompletionExpiredAction = "The project completion date has expired";
        public const string ProjectHardDeadlineExpiredAction = "The project hard deadline has expired";

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
                        Action = UpdateOverdueAction
                    });
                }

                // Check if phase end date is in past (ignore if not set)
                var phaseEnd = source.LatestUpdate?.ExpectedCurrentPhaseEnd?.Date;
                if (phaseEnd.HasValue)
                {
                    if (phaseEnd.Value < DateTime.Today)
                    {
                        actions.Add(new ProjectActionItemModel()
                        {
                            ActionType = ProjectActionItemModel.ActionItemType.Date,
                            Action = PhaseCompletionExpiredAction
                        });
                    }
                }

                // Check if end dates are empty or have expired 
                var endDate = source.ActualEndDate?.Date ?? source.ExpectedEndDate?.Date;
                if (!endDate.HasValue)
                {
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = ProjectEndDateMissingAction
                    });
                }
                else if (endDate.Value < DateTime.Today)
                {
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = ProjectCompletionExpiredAction
                    });
                }

                // Check if hard deadline expired 
                var deadline = source.HardEndDate?.Date;
                if (deadline.HasValue)
                {
                    if (deadline.Value < DateTime.Today)
                    {
                        actions.Add(new ProjectActionItemModel()
                        {
                            ActionType = ProjectActionItemModel.ActionItemType.Date,
                            Action = ProjectHardDeadlineExpiredAction
                        });
                    }
                }
            }

            // Check if start dates have expired
            if (isInPhases(PortfolioSettings.ProjectBacklogPhaseViewKeys))
            {
                // Get the latest out of the two start dates
                Func<ProjectDate, bool> dateHasValue = (d) => d?.Date != null;
                var date = dateHasValue(source.StartDate) ?
                        (dateHasValue(source.ActualStartDate) ?
                            (source.StartDate.Date.Value > source.ActualStartDate.Date.Value ?
                                source.StartDate.Date
                                : source.ActualStartDate.Date)
                            : source.StartDate?.Date)
                    : source.ActualStartDate?.Date;

                if (date.HasValue)
                {
                    if (date.Value < DateTime.Today)
                    {
                        actions.Add(new ProjectActionItemModel()
                        {
                            ActionType = ProjectActionItemModel.ActionItemType.Date,
                            Action = StartDateExpiredAction
                        });
                    }
                }
                else
                {
                    // No start date provided
                    actions.Add(new ProjectActionItemModel()
                    {
                        ActionType = ProjectActionItemModel.ActionItemType.Date,
                        Action = NoStartDateAction
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