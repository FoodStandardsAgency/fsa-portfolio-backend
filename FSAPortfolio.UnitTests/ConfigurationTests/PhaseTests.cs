using FSAPortfolio.UnitTests.APIClients;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    [TestClass]
    public class PhaseTests
    {
        private const string PhaseBackup = "Backlog, Discovery, Implementation, Testing, Live, Completed";
        private const string TestStartPhases = "Backlog, Discovery, Implementation, Testing, Live, Completed";
        private const string TestChangedPhases = "backlog, discovery, New1, New2, New3, New4";
        private const string TestChanged_ReducedNumberPhases = "backlog, discovery, New1, New2, New3";

        [ClassCleanup]
        public static async Task CleanupTest()
        {
            await ProjectClient.DeleteTestProjectsAsync();
            await PortfolioConfigClient.UpdatePhasesAsync("dev", PhaseBackup);
        }

        [TestMethod]
        public async Task ReconfigurePhases_SameNumber_Test()
        {
            await TestPhaseChange(TestStartPhases, TestChangedPhases);
        }

        [TestMethod]
        public async Task ReconfigurePhases_ReduceNumber_Test()
        {
            await TestPhaseChange(TestStartPhases, TestChanged_ReducedNumberPhases, new string[] { "Live" });
        }

        [TestMethod]
        public async Task ReconfigurePhases_ReduceNumber_FailureMode_Test()
        {
            try
            {
                await TestPhaseChange(TestStartPhases, TestChanged_ReducedNumberPhases);
            }
            catch(Exception e)
            {
                Assert.AreEqual("Phase [Live] can't be removed because it has projects assigned to it. This is likely occurring because you are trying to reduce the number of phases but there are projects assigned to the phase to be removed.", e.Message);
            }
        }

        private static async Task TestPhaseChange(string startPhases, string changedPhases, string[] ignorePhases = null)
        {
            // Ensure the projects are there
            await ProjectClient.EnsureTestProjects();

            // DATA SET UP -------------------------------
            // Reconfigure the categories
            var categoriesBackup = await PortfolioConfigClient.UpdatePhasesAsync("dev", startPhases);
            Assert.AreEqual(PhaseBackup, categoriesBackup);

            GetProjectQueryDTO options = await PortfolioConfigClient.GetFilterOptionsAsync("dev");
            var phaseOptions = options.Options.PhaseItems;
            if(ignorePhases != null) phaseOptions.RemoveAll(p => ignorePhases.Contains(p.Display));

            List<ProjectUpdateModel> updates = new List<ProjectUpdateModel>();
            Action<ProjectUpdateModel, string> ensurePhase = (p, ph) =>
            {
                if (p.phase != ph)
                {
                    p.phase = ph;
                    updates.Add(p);
                }
            };

            var testProjects = await ProjectClient.GetTestProjectsAsync();

            int ci = 0;
            for (int i = 0; i < ProjectClient.TestProjectCount; i++)
            {
                ensurePhase(testProjects[i], phaseOptions[ci++].Value);
                if (ci >= phaseOptions.Count) ci = 0;
            }

            await ProjectClient.UpdateProjectsAsync(updates);
            updates.Clear();

            // CHANGE CATEGORIES  -------------------------------
            categoriesBackup = await PortfolioConfigClient.UpdatePhasesAsync("dev", changedPhases);
            Assert.AreEqual(startPhases, categoriesBackup);

            // Restore the categories
            categoriesBackup = await PortfolioConfigClient.UpdatePhasesAsync("dev", categoriesBackup);
            Assert.AreEqual(changedPhases, categoriesBackup);
        }



    }
}
