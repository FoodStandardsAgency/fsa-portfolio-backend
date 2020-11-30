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
        public static bool UserHasFSAClaim(this ApiController controller)
        {
            var principal = controller.RequestContext.Principal;
            var identity = principal?.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated && identity.Claims != null)
            {
                return identity.Claims.Any(c => c.Type == ApplicationUser.AccessGroupClaimType && fsaClaims.Contains(c.Value));
            }
            return false;
        }
    }
}