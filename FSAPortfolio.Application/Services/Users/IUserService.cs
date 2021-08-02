using FSAPortfolio.Application.Models;
using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.App.Users
{
    public interface IUserService
    {
        Task CreateUser(string userName, string passwordHash, string accessGroupViewKey);
        Task SeedAccessGroups();
        Task<UserSearchResponseModel> SearchUsersAsync(string portfolio, string term, bool includeNone = false);
        Task<SupplierResponseModel> GetSuppliersAsync();
        Task<UserModel> GetADUserAsync(string userName);
        Task<UserModel> GetUserAsync(string userName, string passwordHash);
        IdentityResponseModel GetCurrentIdentity();
    }
}