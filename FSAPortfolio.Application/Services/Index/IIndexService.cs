using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Index
{
    public interface IIndexService
    {
        Task IndexProjectAsync(string projectId);
        Task CreateIndexAsync();
        Task RebuildIndexAsync();
    }
}