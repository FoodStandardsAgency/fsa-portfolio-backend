using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Index
{
    public interface IIndexService
    {
        Task<object> IndexProjectAsync(string projectId);
        Task<object> DeleteProjectAsync(string projectId);
        Task<object> ReindexProjectAsync(string projectId);
    }
}