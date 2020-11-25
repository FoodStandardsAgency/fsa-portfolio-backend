using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public class ApplicationUser : IUser, IUser<string>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string AccessGroupViewKey { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            // Add custom user claims here
            // TODO: add portfolio access claims


            return userIdentity;
        }

    }
}