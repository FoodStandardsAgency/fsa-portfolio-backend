using FSAPortfolio.UnitTests.ConfigurationTests;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.APIClients
{
    public static class PortfolioClient
    {
        internal static async Task<ArchiveResponse> ArchiveProjectsAsync(string portfolio)
        {
            return await BackendAPIClient.GetAsync<ArchiveResponse>($"api/Portfolios/{portfolio}/archive");
        }
    }
}
