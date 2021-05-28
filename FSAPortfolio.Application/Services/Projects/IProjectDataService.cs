using FSAPortfolio.WebAPI.DTO;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Projects
{
    public interface IProjectDataService
    {
        Task<GetProjectExportDTO> GetProjectExportDTOAsync(string viewKey);
    }
}