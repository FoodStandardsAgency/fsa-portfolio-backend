using FSAPortfolio.Application.Models;
using FSAPortfolio.WebAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Projects
{
    public interface IProjectDataService
    {
        Task<GetProjectExportDTO> GetProjectExportDTOAsync(string viewKey);
        Task<IEnumerable<SelectItemModel>> GetProjectDataAsync(string portfolio);
    }
}