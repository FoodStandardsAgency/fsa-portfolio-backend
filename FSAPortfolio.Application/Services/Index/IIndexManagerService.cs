using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Index
{
    public interface IIndexManagerService
    {
        Task<IndexOperationResult> CreateIndexAsync();
        Task<IndexOperationResult> RebuildIndexAsync();
        Task<object> GetHealthAsync();
    }
}