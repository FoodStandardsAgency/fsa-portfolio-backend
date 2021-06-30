using FSAPortfolio.Application.Models;
using FSAPortfolio.Entities.Projects;
using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.App.Users
{
    public interface IPersonService
    {
        Task<AddSupplierResponseModel> AddSupplierAsync(string portfolioViewKey, string userName, string passwordHash);
        Task MapPeopleAsync(ProjectUpdateModel update, Project project);
        Task ResetADReferencesAsync();
        Task RemoveDuplicatesAsync();
    }
}