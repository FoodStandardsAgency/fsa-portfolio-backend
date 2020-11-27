using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Microsoft;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public override bool SupportsUserRole => true;
        public override bool SupportsUserClaim => true;
        public override bool SupportsUserPassword => true;
        private const string _accessTokenRegexPattern = "AccessToken (?<accessToken>.*)";
        private MicrosoftGraphUserStore graph;
        private string accessToken;
        public ApplicationUserManager(IUserStore<ApplicationUser> store, string accessToken) : base(store)
        {
            if (accessToken != null)
            {
                this.accessToken = accessToken;
                graph = new MicrosoftGraphUserStore();
            }
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            string activeDirectoryAccessToken = null;

            // If we have an access token in the header, use active directory to authenticate the user.
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authheader = context.Request.Headers["Authorization"];
                var match = Regex.Match(authheader, _accessTokenRegexPattern);
                if (match.Success)
                {
                    activeDirectoryAccessToken = match.Groups["accessToken"].Value;
                }
            }
            var manager = new ApplicationUserManager(new UserStore(context.Get<PortfolioContext>()), activeDirectoryAccessToken);

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            ApplicationUser user;
            if (accessToken != null)
            {
                var aduser = await graph.GetUserForAccessToken(accessToken);
                user = new ApplicationUser()
                {
                    Id = aduser.id,
                    ActiveDirectoryUserId = aduser.id,
                    UserName = aduser.userPrincipalName
                };
            }
            else
            {
                user = await base.FindAsync(userName, password);
            }
            return user;
        }


        /// <summary>
        /// E.g. ODD.Admin, ODD.Editor 
        /// </summary>
        /// <param name="userId">Either the active directory id or the local user id. Check the access token to distinguish which id it is.</param>
        /// <returns></returns>
        public override async Task<IList<string>> GetRolesAsync(string userId)
        {
            List<Role> roles;
            if (accessToken != null)
            {
                roles = (await graph.GetUserRolesAsync(userId)).ToList();
            }
            else
            {
                roles = (await Store.FindByIdAsync(userId)).Roles;
            }
            return roles.Select(r => r.ViewKey).ToList();
        }

        // E.g. AD user, Supplier
        public override async Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            // TODO: stub
            return new List<Claim>();
        }

        public override async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return user.PasswordHash == password;
        }

    }
}