using FSAPortfolio.Entities.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public interface IRoleService
    {
        Task<Role[]> GetFilteredRoleListAsync(IEnumerable<Role> userRoleList, bool isSupplier);
        Task<Role[]> GetFilteredRoleListAsync(Role[] userRoleList, bool isSupplier);
    }
}