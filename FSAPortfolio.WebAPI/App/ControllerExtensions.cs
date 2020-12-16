using FSAPortfolio.WebAPI.App.Identity;
using FSAPortfolio.WebAPI.App.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    public static class ControllerExtensions
    {
        private static string[] fsaClaims = new string[] {
            AccessGroupConstants.FSAViewKey,
            AccessGroupConstants.EditorViewKey,
            AccessGroupConstants.AdminViewKey,
            AccessGroupConstants.SuperuserViewKey
        };

        private static string[] supplierClaims = new string[] {
            AccessGroupConstants.SupplierViewKey
        };

        public static bool UserHasFSAClaim(this ApiController controller) => UserHasClaim(controller, fsaClaims);

        public static bool UserHasSupplierClaim(this ApiController controller) => UserHasClaim(controller, supplierClaims);

        private static bool UserHasClaim(ApiController controller, string[] claims)
        {
            var principal = controller.RequestContext.Principal;
            var identity = principal?.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated && identity.Claims != null)
            {
                return identity.Claims.Any(c => c.Type == ApplicationUser.AccessGroupClaimType && claims.Contains(c.Value));
            }
            return false;
        }

    }
}