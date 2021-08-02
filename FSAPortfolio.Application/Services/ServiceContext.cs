using FSAPortfolio.Entities;
using FSAPortfolio.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FSAPortfolio.WebAPI.App.Users;
using System.Security.Principal;
using FSAPortfolio.Entities.Organisation;
using System.Web.Http;
using System.Net;
using System.Security.Claims;
using FSAPortfolio.WebAPI.App.Identity;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.Common;

namespace FSAPortfolio.Application.Services
{
    public class ServiceContext : IServiceContext
    {
        private Lazy<PortfolioContext> lazyPortfolioContext;
        public PortfolioContext PortfolioContext => lazyPortfolioContext.Value;

        public ServiceContext(Lazy<PortfolioContext> lazyPortfolioContext)
        {
            this.lazyPortfolioContext = lazyPortfolioContext;
#if DEBUG
            AppLog.TraceVerbose($"{nameof(ServiceContext)} created.");
#endif
        }

        private static string[] fsaClaims = new string[] {
            AccessGroupConstants.FSAViewKey,
            AccessGroupConstants.EditorViewKey,
            AccessGroupConstants.AdminViewKey,
            AccessGroupConstants.SuperuserViewKey
        };

        private static string[] supplierClaims = new string[] {
            AccessGroupConstants.SupplierViewKey
        };

        private IPrincipal User => HttpContext.Current.User;
        public ClaimsIdentity Identity => HttpContext.Current.User?.Identity as ClaimsIdentity;
        public string CurrentUserName => Identity?.Name;
        public string ActiveDirectoryId => Identity?.Claims?.SingleOrDefault(c => c.Type == ApplicationUser.ActiveDirectoryClaimType)?.Value;


        public bool HasPermission(Portfolio portfolio) => portfolio.RequiredRoles.Any(r => User.IsInRole(r.ViewKey));
        public bool HasPermission(Portfolio portfolio, params string[] roleNames)
        {
            var roles = roleNames.Select(r => new Role(portfolio.IDPrefix, r)).ToArray();
            return HasPermission(portfolio, roles);
        }
        public bool HasPermission(Portfolio portfolio, Role[] roles)
        {
            return roles.Any(k => User.IsInRole(k.ViewKey) && portfolio.RequiredRoles.Contains(k));
        }
        public bool HasPermission(params string[] roles)
        {
            return roles.Any(k => User.IsInRole(k));
        }
        public void AssertPermission(Portfolio portfolio)
        {
            if (!HasPermission(portfolio)) ThrowResponseException(HttpStatusCode.Forbidden);
        }
        public void AssertPermission(Portfolio portfolio, params string[] roles)
        {
            if (!HasPermission(portfolio, roles)) ThrowResponseException(HttpStatusCode.Forbidden);
        }
        public void AssertSuperuser()
        {
            if (!HasPermission(AccessGroupConstants.SuperuserViewKey)) ThrowResponseException(HttpStatusCode.Forbidden);
        }
        public void AssertAdmin(Portfolio portfolio)
        {
            if (!HasPermission(portfolio, AccessGroupConstants.AdminViewKey, AccessGroupConstants.SuperuserViewKey)) ThrowResponseException(HttpStatusCode.Forbidden);
        }
        public void AssertAdmin()
        {
            if (!HasPermission(AccessGroupConstants.AdminViewKey, AccessGroupConstants.SuperuserViewKey)) ThrowResponseException(HttpStatusCode.Forbidden);
        }
        public void AssertEditor(Portfolio portfolio)
        {
            if (!HasPermission(portfolio, AccessGroupConstants.AdminViewKey, AccessGroupConstants.SuperuserViewKey, AccessGroupConstants.EditorViewKey)) ThrowResponseException(HttpStatusCode.Forbidden);
        }

        public bool UserHasFSAClaim() => UserHasClaim(fsaClaims);

        public bool UserHasSupplierClaim() => UserHasClaim(supplierClaims);

        public bool UserHasClaim(string[] claims)
        {
            if (Identity != null && Identity.IsAuthenticated && Identity.Claims != null)
            {
                return Identity.Claims.Any(c => c.Type == ApplicationUser.AccessGroupClaimType && claims.Contains(c.Value));
            }
            return false;
        }

        private void ThrowResponseException(HttpStatusCode code)
        {
            AppLog.TraceWarning($"Returning Http Response Code '{code}'");
            throw new HttpResponseException(code);
        }
    }
}
