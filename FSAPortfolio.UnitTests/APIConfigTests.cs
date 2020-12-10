using System;
using System.Linq;
using System.Threading.Tasks;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FSAPortfolio.UnitTests
{
    [TestClass]
    public class APIConfigTests
    {
        private const string ProjectSize_TestValue = "Size1";

        [TestMethod]
        public async Task UpdateWithNoChange()
        {
            var config = await BackendAPIClient.GetPortfolioConfigurationAsync("odd");
            var beforeJson = JsonConvert.SerializeObject(config);

            var update = new PortfolioConfigUpdateRequest() { ViewKey = "odd", Labels = config.Labels };
            await BackendAPIClient.UpdatePortfolioConfigurationAsync(update);

            config = await BackendAPIClient.GetPortfolioConfigurationAsync("odd");
            var afterJson = JsonConvert.SerializeObject(config);

            Assert.AreEqual(beforeJson, afterJson);
        }

        [TestMethod]
        public async Task UpdateProjectSize()
        {
            await TestLabel(ProjectPropertyConstants.project_size, ProjectSize_TestValue);
        }

        private async Task TestLabel(string fieldName, string extraTestValues)
        {
            var config = await BackendAPIClient.GetPortfolioConfigurationAsync("odd");
            var original_Config_Expected = JsonConvert.SerializeObject(config);

            var label = config.Labels.Single(l => l.FieldName == fieldName);
            var original_LabelValue_Expected = label.InputValue;

            // Add the additional values to the current label value (as the existing ones may be in use)...
            var updated_LabelValue_Expected = $"{original_LabelValue_Expected}, {extraTestValues}";
            label.InputValue = updated_LabelValue_Expected;
            var updated_Config_Expected = JsonConvert.SerializeObject(config);

            // Update the label and check values
            var update = new PortfolioConfigUpdateRequest() { ViewKey = "odd", Labels = config.Labels };
            await BackendAPIClient.UpdatePortfolioConfigurationAsync(update);

            config = await BackendAPIClient.GetPortfolioConfigurationAsync("odd");
            var updated_Config_Actual = JsonConvert.SerializeObject(config);
            label = config.Labels.Single(l => l.FieldName == fieldName);
            var updated_LabelValue_Actual = label.InputValue;

            // Now set it back
            label.InputValue = original_LabelValue_Expected;
            update = new PortfolioConfigUpdateRequest() { ViewKey = "odd", Labels = config.Labels };
            await BackendAPIClient.UpdatePortfolioConfigurationAsync(update);

            config = await BackendAPIClient.GetPortfolioConfigurationAsync("odd");
            var original_Config_Actual = JsonConvert.SerializeObject(config);
            label = config.Labels.Single(l => l.FieldName == fieldName);
            var original_LabelValue_Actual = label.InputValue;

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
                //Assert.AreEqual(expectedLabel.AdminOnly, actualLabel.AdminOnly);
                //Assert.AreEqual(expectedLabel.AdminOnlyLock, actualLabel.AdminOnlyLock);
                //Assert.AreEqual(expectedLabel.FieldGroup, actualLabel.FieldGroup);
                //Assert.AreEqual(expectedLabel.FieldName, actualLabel.FieldName);
                //Assert.AreEqual(expectedLabel.FieldOrder, actualLabel.FieldOrder);
                //Assert.AreEqual(expectedLabel.FieldTitle, actualLabel.FieldTitle);
                //Assert.AreEqual(expectedLabel.FieldType, actualLabel.FieldType);
                //Assert.AreEqual(expectedLabel.FieldTypeDescription, actualLabel.FieldTypeDescription);
                //Assert.AreEqual(expectedLabel.FieldTypeLocked, actualLabel.FieldTypeLocked);
                //Assert.AreEqual(expectedLabel.Filterable, actualLabel.Filterable);
                //Assert.AreEqual(expectedLabel.FilterProject, actualLabel.FilterProject);
                //Assert.AreEqual(expectedLabel.FSAOnly, actualLabel.FSAOnly);
                //Assert.AreEqual(expectedLabel.GroupOrder, actualLabel.GroupOrder);
                //Assert.AreEqual(expectedLabel.Included, actualLabel.Included);
                //Assert.AreEqual(expectedLabel.IncludedLock, actualLabel.IncludedLock);
                //Assert.AreEqual(expectedLabel.InputValue, actualLabel.InputValue);
                //Assert.AreEqual(expectedLabel.Label, actualLabel.Label);
                //Assert.AreEqual(expectedLabel.MasterField, actualLabel.MasterField);
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
