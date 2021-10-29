using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.Application.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSAPortfolio.WebAPI.App.Mapping.Organisation.Resolvers.Summaries;
using FSAPortfolio.Entities;

namespace FSAPortfolio.UnitTests.SummaryTests
{
    [TestClass]
    public class ProjectFlaggingTests
    {
        [TestMethod]
        public void ProjectFlaggingMappingTest()
        {
            ProjectIndexModel result;
            var project = new Project()
            {
                LatestUpdate = new ProjectUpdateItem()
                {
                    Phase = new ProjectPhase()
                    {
                        ViewKey = "phase1"
                    }
                }
            };
            PortfolioMapper.Configure();

            // The mapping function
            Func<Project, ProjectIndexModel> map = (p) =>
            {
                return PortfolioMapper.ConfigMapper.Map<ProjectIndexModel>(p, opt => {
                    opt.Items[PortfolioSummaryResolver.SummaryTypeKey] = PortfolioSummaryModel.ByUser;
                });
            };

            // Check actions 
            Func<ProjectIndexModel, ProjectActionItemModel.ActionItemType, string, ProjectActionItemModel> getAction = (p, t, a) => {
                return p.Actions?.ActionItems?.SingleOrDefault(i => i.ActionType == t && i.Action == a);
            };
            Action<string, ProjectActionItemModel.ActionItemType, string> assertAction = (phase, t, a) => {
                project.LatestUpdate.Phase.ViewKey = phase;
                result = map(project);
                Assert.IsNotNull(getAction(result, t, a));
            };
            Action<string, ProjectActionItemModel.ActionItemType, string> assertNoAction = (phase, t, a) => {
                project.LatestUpdate.Phase.ViewKey = phase;
                result = map(project);
                Assert.IsNull(getAction(result, t, a));
            };


            // No dates
            result = map(project);
            Assert.IsNotNull(getAction(result, ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction));
            Assert.IsNotNull(getAction(result, ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectEndDateMissingAction));

            // A.	Project is in ‘discovery, alpha or beta’ and latest update is older than 14 days
            project.LatestUpdate.Timestamp = DateTime.Today.Subtract(TimeSpan.FromDays(13));
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertNoAction("phase2", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertNoAction("phase3", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertNoAction("phase4", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertNoAction("phase5", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);

            project.LatestUpdate.Timestamp = DateTime.Today.Subtract(TimeSpan.FromDays(14));
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertAction("phase1", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertAction("phase2", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertAction("phase3", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertNoAction("phase4", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);
            assertNoAction("phase5", ProjectActionItemModel.ActionItemType.Update, ProjectActionsResolver.UpdateOverdueAction);

            // B.	Project is in ‘backlog’ and the greater of intended start date or start date is in the past, or intended start date or start date are not provided
            project.ActualStartDate = null;
            project.StartDate = new ProjectDate() { Date = DateTime.Today };
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);
            project.StartDate.Date = DateTime.Today.Subtract(TimeSpan.FromDays(1));
            assertAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);

            project.StartDate.Date = null;
            project.ActualStartDate = new ProjectDate() { Date = DateTime.Today };
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);
            project.ActualStartDate.Date = DateTime.Today.Subtract(TimeSpan.FromDays(1));
            assertAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);

            // Both dates set - current
            project.StartDate = new ProjectDate() { Date = DateTime.Today }; 
            project.ActualStartDate = new ProjectDate() { Date = DateTime.Today };
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);

            // Both dates set - one in past
            project.ActualStartDate.Date = DateTime.Today.Subtract(TimeSpan.FromDays(1));
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);

            // Both dates set - both in past
            project.StartDate.Date = DateTime.Today.Subtract(TimeSpan.FromDays(2));
            assertAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.StartDateExpiredAction);

            // Dates not provided
            project.ActualStartDate.Date = null;
            project.StartDate.Date = null;
            assertAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.NoStartDateAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.NoStartDateAction);

            // C.	Project is in ‘discovery, alpha or beta’ and phase completion is in the past, or phase completion date in not provided
            // Current
            project.LatestUpdate.ExpectedCurrentPhaseEnd = new ProjectDate() { Date = DateTime.Today };
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertNoAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertNoAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertNoAction("phase4", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertNoAction("phase5", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);

            // In past
            project.LatestUpdate.ExpectedCurrentPhaseEnd.Date = DateTime.Today.Subtract(TimeSpan.FromDays(1));
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertNoAction("phase4", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);
            assertNoAction("phase5", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.PhaseCompletionExpiredAction);

            // Not provided - this is being cut from spec

            // D.	Project is in ‘discovery, alpha or beta’ and project completion or hard deadline is in the past, or end date is not provided (hard deadline is not needed for all projects).
            // Current
            project.ActualEndDate = new ProjectDate() { Date = DateTime.Today };
            project.ExpectedEndDate = new ProjectDate() { Date = DateTime.Today };
            project.HardEndDate = new ProjectDate() { Date = DateTime.Today };
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertNoAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertNoAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertNoAction("phase4", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertNoAction("phase5", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);

            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase4", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase5", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);

            // In past
            project.ActualEndDate.Date = DateTime.Today.Subtract(TimeSpan.FromDays(1));
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertNoAction("phase4", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);
            assertNoAction("phase5", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectCompletionExpiredAction);

            project.HardEndDate.Date = DateTime.Today.Subtract(TimeSpan.FromDays(1));
            assertNoAction("phase0", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase4", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase5", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);

            // Not provided
            project.ActualEndDate = new ProjectDate() { Date = DateTime.Today };
            project.ExpectedEndDate = new ProjectDate() { Date = DateTime.Today };
            project.HardEndDate = new ProjectDate() { Date = null };
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectEndDateMissingAction);
            assertNoAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectEndDateMissingAction);
            assertNoAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectEndDateMissingAction);
            assertNoAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);
            assertNoAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectHardDeadlineExpiredAction);

            project.ActualEndDate = new ProjectDate() { Date = null };
            project.ExpectedEndDate = new ProjectDate() { Date = null };
            assertAction("phase1", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectEndDateMissingAction);
            assertAction("phase2", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectEndDateMissingAction);
            assertAction("phase3", ProjectActionItemModel.ActionItemType.Date, ProjectActionsResolver.ProjectEndDateMissingAction);


        }

    }
}
