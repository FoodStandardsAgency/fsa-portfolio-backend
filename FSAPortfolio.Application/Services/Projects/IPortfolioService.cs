using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FSAPortfolio.Application.Services.Projects
{
    public interface IPortfolioService
    {
        Task<string[]> ArchiveProjectsAsync(string portfolioViewKey);
        Task<PortfolioConfiguration> GetConfigAsync(string portfolioViewKey, bool includedOnly = false);
        PortfolioLabelConfig[] GetCustomFilterLabels(PortfolioConfiguration config);
        Task<ProjectEditOptionsModel> GetNewProjectOptionsAsync(PortfolioConfiguration config, ProjectEditViewModel projectModel = null);
        Task<IEnumerable<PortfolioModel>> GetPortfoliosAsync();
        Task<ProjectReservation> GetProjectReservationAsync(PortfolioConfiguration config);
        Task<PortfolioSummaryModel> GetSummaryAsync(string viewKey, string summaryType, string user, string projectType);
    }
}