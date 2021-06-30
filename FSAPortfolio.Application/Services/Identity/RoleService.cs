using FSAPortfolio.Application.Services;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public class RoleService : BaseService, IRoleService
    {
        public RoleService(IServiceContext context) : base(context)
        {
        }

        public Task<string[]> GetFilteredRoleListAsync(IEnumerable<Role> userRoleList, bool isSupplier) =>
            GetFilteredRoleListAsync(userRoleList.Select(r => r.ViewKey).ToArray(), isSupplier);

        public async Task<string[]> GetFilteredRoleListAsync(string[] userRoleList, bool isSupplier)
        {
            // Now do the roles, filtering using portfolio required roles...
            var portfolios = await ServiceContext.PortfolioContext.Portfolios.ToListAsync();
            var portfolioRoles = portfolios
                .SelectMany(p => p.RequiredRoles)
                .ToArray();

            // Default roles for anyone except suppliers
            var defaultRoleList = isSupplier ? new string[0] : portfolios.Select(p => $"{p.IDPrefix}.Read").ToArray();

            // Merge and take the intersection with required portfolio roles...
            var roleList = userRoleList.Union(defaultRoleList).Intersect(portfolioRoles).ToArray();

            return roleList;
        }
    }
}