using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public class ApplicationUser : IUser
    {
        public const string AccessGroupClaimType = "AccessGroup";
        public const string ActiveDirectoryClaimType = "ActiveDirectory";
        public string Id { get; set; }

        public string UserName { get; set; }
        public string ActiveDirectoryUserId { get; set; }
        public string AccessGroupViewKey { get; set; }
        public string PasswordHash { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            if(AccessGroupViewKey != null)
                userIdentity.AddClaim(new Claim(AccessGroupClaimType, AccessGroupViewKey));
            
            if(ActiveDirectoryUserId != null)
                userIdentity.AddClaim(new Claim(ActiveDirectoryClaimType, ActiveDirectoryUserId));

            return userIdentity;
        }

    }
}