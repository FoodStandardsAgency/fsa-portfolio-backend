using FSAPortfolio.Application.Services.Index.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Index
{
    public interface ISearchService
    {
        Task<IEnumerable<ProjectSearchIndexModel>> SearchProjectIndexAsync(string term);
    }
}