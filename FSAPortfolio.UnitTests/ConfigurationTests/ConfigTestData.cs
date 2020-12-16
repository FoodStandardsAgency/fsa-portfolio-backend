using FSAPortfolio.UnitTests.TestMappings;
using FSAPortfolio.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    public class ConfigTestData
    {
        private string portfolio;
        public PortfolioConfigModel Config { get; set; }
        public string ConfigJSON => JsonConvert.SerializeObject(Config);

        public ConfigTestData(string portfolio)
        {
            this.portfolio = portfolio;
        }
        public async Task LoadAsync()
        {
            // 1. GET CONFIG the original 
            Config = await BackendAPIClient.GetPortfolioConfigurationAsync(portfolio);
        }

        public PortfolioLabelModel GetLabel(string fieldName) => Config.Labels.Single(l => l.FieldName == fieldName);

        public ConfigTestData Clone()
        {
            var clone = new ConfigTestData(portfolio);
            clone.Config = TestMapper.ProjectMapper.Map<PortfolioConfigModel>(Config);
            return clone;
        }

        internal async Task UpdateAsync()
        {
            var request = new PortfolioConfigUpdateRequest() { ViewKey = portfolio, Labels = Config.Labels };
            await BackendAPIClient.UpdatePortfolioConfigurationAsync(request);
        }

        internal static async Task<ConfigTestData> LoadAsync(string portfolio)
        {
            var config = new ConfigTestData(portfolio);
            await config.LoadAsync();
            return config;
        }
    }
}
