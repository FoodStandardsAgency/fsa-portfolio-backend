using FSAPortfolio.Application.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    internal static class ConfigurationTestUtils
    {
        internal static async Task TestCollectionLabel(string fieldName, string testValue, bool addToCurrent = true, Func<Task> midpointTest = null)
        {
            // 1. GET CONFIG the original 
            var initialState = await ConfigTestData.LoadAsync(TestSettings.TestPortfolio);
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

            var afterUpdateState = await ConfigTestData.LoadAsync(TestSettings.TestPortfolio);
            var updated_Config_Actual = afterUpdateState.ConfigJSON;
            var updated_LabelValue_Actual = afterUpdateState.GetLabel(fieldName).InputValue;

            // 3. Do midpoint testing
            if (midpointTest != null)
            {
                await midpointTest();
            }

            // 4. UPDATE CONFIG to set it back to original
            var resetState = afterUpdateState.Clone();
            resetState.GetLabel(fieldName).InputValue = original_LabelValue_Expected;
            await resetState.UpdateAsync();

            var afterResetState = await ConfigTestData.LoadAsync(TestSettings.TestPortfolio);
            var original_Config_Actual = afterResetState.ConfigJSON;
            var original_LabelValue_Actual = afterResetState.GetLabel(fieldName).InputValue;

            // Verify update
            Assert.AreEqual(updated_LabelValue_Expected, updated_LabelValue_Actual);
            VerifyConfigs(updated_Config_Expected, updated_Config_Actual);

            // Verify reset to original
            Assert.AreEqual(original_LabelValue_Expected, original_LabelValue_Actual);
            VerifyConfigs(original_Config_Expected, original_Config_Actual);
        }

        private static void VerifyConfigs(string expectedJson, string actualJson)
        {
            var expectedConfig = JsonConvert.DeserializeObject<PortfolioConfigModel>(expectedJson);
            var actualConfig = JsonConvert.DeserializeObject<PortfolioConfigModel>(actualJson);
            foreach (var expectedLabel in expectedConfig.Labels)
            {
                var actualLabel = actualConfig.Labels.Single(l => l.FieldName == expectedLabel.FieldName && !string.IsNullOrWhiteSpace(expectedLabel.FieldName));
                CompareProperties(expectedLabel, actualLabel);
            }
            Assert.AreEqual(expectedJson, actualJson);
        }

        private static void CompareProperties(PortfolioLabelModel expected, PortfolioLabelModel actual)
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
