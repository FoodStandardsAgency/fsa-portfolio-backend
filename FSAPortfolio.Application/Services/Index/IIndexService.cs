using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Index
{
    public interface IIndexService
    {
        Task IndexProjectAsync(string projectId);
        Task DeleteProjectAsync(string projectId);
        Task ReindexProjectAsync(string projectId);
    }
}