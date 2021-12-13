using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Projects
{
    public interface IDataDumpService : IBaseService
    {
        Task DumpPortfolioConfig(string portfolio);
        Task DumpPortfolioProjects(string portfolio, string[] id = null);
        Task DumpProjectUpdates(string portfolio, string[] id = null);
        Task DumpProjectChanges(string portfolio, string[] id = null);
    }
}