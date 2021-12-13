using FSAPortfolio.Application.Models;
using FSAPortfolio.Entities.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Microsoft
{
    public interface IMicrosoftGraphUserStoreService
    {
        Task<MicrosoftGraphUserModel> GetUserForAccessToken(string accessToken);
        Task<MicrosoftGraphUserModel> GetUserForPrincipalNameAsync(string term);
        Task<IEnumerable<Role>> GetUserRolesAsync(string userId);
        Task<MicrosoftGraphUserListResponse> GetUsersAsync(string term, int count = 10);
    }
}