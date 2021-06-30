using FSAPortfolio.Entities.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public interface IRoleService
    {
        Task<string[]> GetFilteredRoleListAsync(IEnumerable<Role> userRoleList, bool isSupplier);
        Task<string[]> GetFilteredRoleListAsync(string[] userRoleList, bool isSupplier);
    }
}