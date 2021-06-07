using FSAPortfolio.Application.Models;
using FSAPortfolio.Entities.Organisation;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Config
{
    public interface IPortfolioConfigurationService
    {
        Task<PortfolioConfigModel> GetConfigurationAsync(string portfolio);
        Task UpdateConfigAsync(string viewKey, PortfolioConfigUpdateRequest update);
    }
}