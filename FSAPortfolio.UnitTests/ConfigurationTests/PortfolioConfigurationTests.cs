using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSAPortfolio.UnitTests.APIClients;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    [TestClass]
    public class PortfolioConfigurationTests
    {
        private const string ProjectSize_TestValue = "EXTRA SIZE";

        [TestMethod]
        public async Task UpdateWithNoChange()
        {
            var before = await ConfigTestData.LoadAsync(TestSettings.TestPortfolio);
            var update = before.Clone();
            await update.UpdateAsync();
            var after = await ConfigTestData.LoadAsync(TestSettings.TestPortfolio);

            Assert.AreEqual(before.ConfigJSON, after.ConfigJSON);
        }


        [TestMethod]
        public async Task UpdateProjectOptions()
        {
            await ConfigurationTestUtils.TestCollectionLabel(ProjectPropertyConstants.project_size, ProjectSize_TestValue);
            await ConfigurationTestUtils.TestCollectionLabel(ProjectPropertyConstants.budgettype, "EXTRA BUDGET TYPE");
            await ConfigurationTestUtils.TestCollectionLabel(ProjectPropertyConstants.onhold, "EXTRA STATUS");
            await ConfigurationTestUtils.TestCollectionLabel(ProjectPropertyConstants.risk_rating, "EXTRA RISK");
            await ConfigurationTestUtils.TestCollectionLabel(ProjectPropertyConstants.phase, "Backlog, Phase1, Phase2, Phase3, Phase4, Phase5", addToCurrent: false);

            // TODO: add rest of collections
        }


    }
}
