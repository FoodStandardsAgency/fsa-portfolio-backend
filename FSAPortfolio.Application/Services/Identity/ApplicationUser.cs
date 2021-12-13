using FSAPortfolio.Entities.Users;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Identity
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

        /// <summary>
        /// These roles are only transient and used by non-AD users (i.e. stored in the database user table) to populate the claims.
        /// For AD users, this collection is not populated.
        /// To get the actual roles for any user identity, use the role claims in the identity.
        /// </summary>
        public List<Role> UserStoreRoleList { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            if (AccessGroupViewKey != null)
            {
                // TODO: NEXT RELEASE!!!
                // --- need to throw exception if accessgroupviewkey is not set! Should now always be set before getting here
                userIdentity.AddClaim(new Claim(AccessGroupClaimType, AccessGroupViewKey));
            }

            if (ActiveDirectoryUserId != null)
            {
                userIdentity.AddClaim(new Claim(ActiveDirectoryClaimType, ActiveDirectoryUserId));
            }

            return userIdentity;
        }



    }
}