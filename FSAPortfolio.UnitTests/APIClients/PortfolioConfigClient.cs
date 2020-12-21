using FSAPortfolio.UnitTests.ConfigurationTests;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.APIClients
{
    public static class PortfolioConfigClient
    {
        internal static async Task<PortfolioConfigModel> GetPortfolioConfigurationAsync(string portfolio)
        {
            return await BackendAPIClient.GetAsync<PortfolioConfigModel>($"api/PortfolioConfiguration/{portfolio}");
        }

        internal static async Task UpdatePortfolioConfigurationAsync(PortfolioConfigUpdateRequest update)
        {
            await BackendAPIClient.PatchAsync($"api/PortfolioConfiguration/{update.ViewKey}", update);
        }
        internal static async Task<string> UpdateCategoriesAsync(string portfolio, string categories)
        {
            var config = await ConfigTestData.LoadAsync(portfolio);
            var categoryLabel = config.GetLabel(ProjectPropertyConstants.category);
            var original_categories = categoryLabel.InputValue;
            categoryLabel.InputValue = categories;
            await config.UpdateAsync();
            return original_categories;
        }

        internal static async Task<GetProjectQueryDTO> GetFilterOptionsAsync(string portfolio)
        {
            return await BackendAPIClient.GetAsync<GetProjectQueryDTO>($"api/Portfolios/{portfolio}/filteroptions");
        }
    }
}
