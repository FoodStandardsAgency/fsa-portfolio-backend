using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Index
{
    public interface IIndexManagerService
    {
        Task CreateIndexAsync();
        Task RebuildIndexAsync();
    }
}