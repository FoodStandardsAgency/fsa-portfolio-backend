using FSAPortfolio.Entities.Users;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Identity
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IServiceContext context) : base(context)
        {
        }

        public Task<Role[]> GetFilteredRoleListAsync(IEnumerable<Role> userRoleList, bool isSupplier) =>
            GetFilteredRoleListAsync(userRoleList.ToArray(), isSupplier);

        public async Task<Role[]> GetFilteredRoleListAsync(Role[] userRoleList, bool isSupplier)
        {
            // Now do the roles, filtering using portfolio required roles...
            var portfolios = await ServiceContext.PortfolioContext.Portfolios.ToListAsync();
            var portfolioRoles = portfolios
                .SelectMany(p => p.RequiredRoles)
                .ToArray();

            // Default roles for anyone except suppliers
            if (isSupplier) userRoleList = userRoleList.Where(r => r.IsAllowedForSupplier).ToArray();
            var defaultRoleList = (isSupplier ? new Role[0] : portfolios.Select(p => new Role(p.IDPrefix, "Read"))).ToArray();

            // Merge and take the intersection with required portfolio roles...
            var roleList = userRoleList.Union(defaultRoleList).Intersect(portfolioRoles).ToArray();

            return roleList;
        }
    }
}