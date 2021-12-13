using FSAPortfolio.UnitTests.ConfigurationTests;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSAPortfolio.Common;

namespace FSAPortfolio.UnitTests.APIClients
{
    public static class PortfolioConfigClient
    {
        internal static async Task<PortfolioConfigModel> GetPortfolioConfigurationAsync(string portfolio)
        {
            return await TestBackendAPIClient.GetAsync<PortfolioConfigModel>($"api/PortfolioConfiguration?portfolio={portfolio}");
        }

        internal static async Task UpdatePortfolioConfigurationAsync(PortfolioConfigUpdateRequest update)
        {
            await TestBackendAPIClient.PatchAsync($"api/PortfolioConfiguration?portfolio={update.ViewKey}", update);
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
        internal static async Task<string> UpdatePhasesAsync(string portfolio, string phases)
        {
            var config = await ConfigTestData.LoadAsync(portfolio);
            var phasesLabel = config.GetLabel(ProjectPropertyConstants.phase);
            var original_phases = phasesLabel.InputValue;
            phasesLabel.InputValue = phases;
            await config.UpdateAsync();
            return original_phases;
        }

        internal static async Task<GetProjectQueryDTO> GetFilterOptionsAsync(string portfolio)
        {
            return await TestBackendAPIClient.GetAsync<GetProjectQueryDTO>($"api/Portfolios/{portfolio}/filteroptions");
        }
    }
}
