using FSAPortfolio.Entities;
using FSAPortfolio.UnitTests.APIClients;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSAPortfolio.Entities.Organisation;
using System.Data.Entity;

namespace FSAPortfolio.UnitTests.PortfolioTests
{
    [TestClass]
    public class ArchiveTests
    {
        const int testProjectCount = 10;
        const string prefix = "Archive_UT_";

        [ClassCleanup]
        public static async Task CleanupTest()
        {
            await ProjectClient.DeleteTestProjectsAsync(prefix);
        }


        /// <summary>
        /// Note that this test may fail on the first run if there are non-test projects waiting to be archived.
        /// In that case, the resulting archived projects have additional project ids that are not in the test set up data.
        /// The test should succeed on a subsequent test run on the same day.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ArchiveTest()
        {
            // Test data setup - use a separate prefix so no risk of interfering with other tests
            await ProjectClient.EnsureTestProjects(testProjectCount, prefix);
            var testProjects = await ProjectClient.GetTestProjectsAsync(prefix);
            var expectedArchivedProjects = new List<string>();

            using(var context = new PortfolioContext())
            {
                // Get the config
                var portfolioConfig = await context.PortfolioConfigurations
                    .Include(c => c.CompletedPhase)
                    .Include(c => c.ArchivePhase)
                    .SingleAsync(p => p.Portfolio.ViewKey == TestSettings.TestPortfolio);

                // Set the timestamp to (half the test project count) days before the age cutoff: this is so half the test projects will get archived.
                var cutoff = DateTime.Today.AddDays(-portfolioConfig.ArchiveAgeDays);
                var latestUpdateTimestamp = cutoff.AddDays(-(testProjectCount / 2));

                // Set the phases and timestamps for the latest updates
                foreach (var project in testProjects.OrderBy(p => p.project_id))
                {
                    var entity = await context.Projects.Include(p => p.LatestUpdate).SingleAsync(p => p.Reservation.ProjectId == project.project_id);
                    entity.LatestUpdate.Phase = portfolioConfig.ArchivePhase;
                    entity.LatestUpdate.Timestamp = latestUpdateTimestamp;

                    if (latestUpdateTimestamp < cutoff) expectedArchivedProjects.Add(project.project_id);

                    latestUpdateTimestamp = latestUpdateTimestamp.AddDays(1);
                }
                await context.SaveChangesAsync();
            }

            // Do the archival and check respoonse
            var response = await PortfolioClient.ArchiveProjectsAsync(TestSettings.TestPortfolio);
            Assert.AreEqual(expectedArchivedProjects.Count, response.ArchivedProjectIds.Length);
            for(int i = 0; i < expectedArchivedProjects.Count; i++)
            {
                Assert.AreEqual(expectedArchivedProjects[i], response.ArchivedProjectIds[i]);
            }
        }
    }
}
