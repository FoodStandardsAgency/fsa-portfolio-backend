using System;
using System.Linq;
using System.Threading.Tasks;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    [TestClass]
    public class APIConfigTests
    {
        private const string ProjectSize_TestValue = "EXTRA SIZE";

        [TestMethod]
        public async Task UpdateWithNoChange()
        {
            var before = await ConfigTestData.LoadAsync("odd");
            var update = before.Clone();
            await update.UpdateAsync();
            var after = await ConfigTestData.LoadAsync("odd");

            Assert.AreEqual(before.ConfigJSON, after.ConfigJSON);
        }

        [TestMethod]
        public async Task UpdateCategoryWithHistoryCheck()
        {
            var projectId = "ODD2009001";
            var testCategory = "EXTRA CATEGORY";
            string originalCategoryViewKey = null;
            Func<Task> updateCategory = async () => { 
                originalCategoryViewKey = await UpdateProjectCategory(projectId, testCategory); 
            };

            // Test procedure: 
            // 1. Update config to add the category
            // - at the midpoint, update the project to use the test category
            // - config then resets - but category is in use so is only hidden
            await TestCollectionLabel(ProjectPropertyConstants.category, testCategory, midpointTest: updateCategory);

            // 2. Update the project to not use the label
            var project = await ProjectTestData.LoadAsync(projectId);
            project.ProjectUpdate.category = originalCategoryViewKey;
            await project.UpdateAsync();

            // 3. Check the option has disappeared.
            await project.LoadAsync();
            var categoryItem = project.DTO.Options.CategoryItems.SingleOrDefault(i => i.Display == testCategory);
            Assert.IsNull(categoryItem);

            // 4. Do a no-change update on config (hidden option should be removed - but no way to verify!)
            var config = await ConfigTestData.LoadAsync("odd");
            await config.UpdateAsync();

        }

        [TestMethod]
        public async Task UpdateProjectOptions()
        {
            await TestCollectionLabel(ProjectPropertyConstants.project_size, ProjectSize_TestValue);
            await TestCollectionLabel(ProjectPropertyConstants.budgettype, "EXTRA BUDGET TYPE");
            await TestCollectionLabel(ProjectPropertyConstants.onhold, "EXTRA STATUS");
            await TestCollectionLabel(ProjectPropertyConstants.risk_rating, "EXTRA RISK");
            await TestCollectionLabel(ProjectPropertyConstants.phase, "Backlog, Phase1, Phase2, Phase3, Phase4", addToCurrent: false);

            // TODO: add rest of collections
        }

        private async Task<string> UpdateProjectCategory(string projectId, string categoryName)
        {
            // 1. GET PROJECT
            var initial = await ProjectTestData.LoadAsync(projectId);
            var originalCategoryViewKey = initial.DTO.Project.category;
            var categoryItem = initial.DTO.Options.CategoryItems.Single(i => i.Display == categoryName);

            // 2. UPDATE PROJECT category
            var update = initial.Clone();
            update.ProjectUpdate.category = categoryItem.Value;
            await update.UpdateAsync();

            // 3. Check the option is present
            await update.LoadAsync();
            Assert.AreEqual(categoryItem.Value, update.DTO.Project.category);

            return originalCategoryViewKey;
        }

        private async Task TestCollectionLabel(string fieldName, string testValue, bool addToCurrent = true, Func<Task> midpointTest = null)
        {
            // 1. GET CONFIG the original 
            var initialState = await ConfigTestData.LoadAsync("odd");
            var original_Config_Expected = initialState.ConfigJSON;
            var original_LabelValue_Expected = initialState.GetLabel(fieldName).InputValue;

            // 2. UPDATE CONFIG with new label and check values
            var updatedState = initialState.Clone();
            var updateLabel = updatedState.GetLabel(fieldName);

            // Add the additional values to the current label value (as the existing ones may be in use)...
            var updated_LabelValue_Expected = addToCurrent ? $"{original_LabelValue_Expected}, {testValue}" : testValue;
            updateLabel.InputValue = updated_LabelValue_Expected;
            var updated_Config_Expected = updatedState.ConfigJSON;
            await updatedState.UpdateAsync();

            var afterUpdateState = await ConfigTestData.LoadAsync("odd");
            var updated_Config_Actual = afterUpdateState.ConfigJSON;
            var updated_LabelValue_Actual = afterUpdateState.GetLabel(fieldName).InputValue;

            // 3. Do midpoint testing
            if(midpointTest != null)
            {
                await midpointTest();
            }

            // 4. UPDATE CONFIG to set it back to original
            var resetState = afterUpdateState.Clone();
            resetState.GetLabel(fieldName).InputValue = original_LabelValue_Expected;
            await resetState.UpdateAsync();

            var afterResetState = await ConfigTestData.LoadAsync("odd");
            var original_Config_Actual = afterResetState.ConfigJSON;
            var original_LabelValue_Actual = afterResetState.GetLabel(fieldName).InputValue;

            // Verify update
            Assert.AreEqual(updated_LabelValue_Expected, updated_LabelValue_Actual);
            VerifyConfigs(updated_Config_Expected, updated_Config_Actual);

            // Verify reset to original
            Assert.AreEqual(original_LabelValue_Expected, original_LabelValue_Actual);
            VerifyConfigs(original_Config_Expected, original_Config_Actual);
        }

        private void VerifyConfigs(string expectedJson, string actualJson)
        {
            var expectedConfig = JsonConvert.DeserializeObject<PortfolioConfigModel>(expectedJson);
            var actualConfig = JsonConvert.DeserializeObject<PortfolioConfigModel>(actualJson);
            foreach(var expectedLabel in expectedConfig.Labels)
            {
                var actualLabel = actualConfig.Labels.Single(l => l.FieldName == expectedLabel.FieldName && !string.IsNullOrWhiteSpace(expectedLabel.FieldName));
                CompareProperties(expectedLabel, actualLabel);
            }
            Assert.AreEqual(expectedJson, actualJson);
        }

        private void CompareProperties(PortfolioLabelModel expected, PortfolioLabelModel actual)
        {
            var expectedProperties = expected.GetType().GetProperties();
            var actualProperties = actual.GetType().GetProperties().ToDictionary(p => p.Name);
            foreach (var expectedProperty in expectedProperties)
            {
                var expectedValue = expectedProperty.GetValue(expected);
                var actualValue = actualProperties[expectedProperty.Name].GetValue(actual);
                Assert.AreEqual(expectedValue, actualValue, $"{expected.FieldName} labels don't match: {expectedProperty.Name} is [{actualValue ?? "null"}]; expected [{expectedValue ?? "null"}]");
            }
        }
    }
}
