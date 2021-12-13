using FSAPortfolio.UnitTests.ConfigurationTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSAPortfolio.Application.Models;


namespace FSAPortfolio.UnitTests.APIClients
{
    public static class PortfolioClient
    {
        internal static async Task<ArchiveResponse> ArchiveProjectsAsync(string portfolio)
        {
            return await TestBackendAPIClient.GetAsync<ArchiveResponse>($"api/Portfolios/{portfolio}/archive");
        }
    }
}
