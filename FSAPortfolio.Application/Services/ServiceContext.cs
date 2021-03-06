﻿using FSAPortfolio.Entities;
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

        public bool HasPermission(Portfolio portfolio) => portfolio.RequiredRoles.Any(r => User.IsInRole(r));
        public bool HasPermission(Portfolio portfolio, params string[] roles)
        {
            var roleViewKeys = roles.Select(r => $"{portfolio.IDPrefix}.{r}").ToArray();
            return roleViewKeys.Any(k => User.IsInRole(k) && portfolio.RequiredRoles.Contains(k));
        }
        public void AssertPermission(Portfolio portfolio)
        {
            if (!HasPermission(portfolio)) throw new HttpResponseException(HttpStatusCode.Forbidden);
        }
        public void AssertPermission(Portfolio portfolio, params string[] roles)
        {
            if (!HasPermission(portfolio, roles)) throw new HttpResponseException(HttpStatusCode.Forbidden);
        }
        public void AssertAdmin(Portfolio portfolio)
        {
            if (!HasPermission(portfolio, "Admin", "Superuser"))
                throw new HttpResponseException(HttpStatusCode.Forbidden);
        }
        public void AssertEditor(Portfolio portfolio)
        {
            if (!HasPermission(portfolio, "Admin", "Superuser", "Editor")) throw new HttpResponseException(HttpStatusCode.Forbidden);
        }

        public bool UserHasFSAClaim() => UserHasClaim(fsaClaims);

        public bool UserHasSupplierClaim() => UserHasClaim(supplierClaims);

        public bool UserHasClaim(string[] claims)
        {
            var identity = User?.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated && identity.Claims != null)
            {
                return identity.Claims.Any(c => c.Type == ApplicationUser.AccessGroupClaimType && claims.Contains(c.Value));
            }
            return false;
        }
    }
}
