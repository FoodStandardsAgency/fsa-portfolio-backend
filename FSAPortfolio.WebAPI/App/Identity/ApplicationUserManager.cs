using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Microsoft;
using FSAPortfolio.WebAPI.App.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        private PortfolioContext portfolioContext;
        private PortfolioRoleManager roleManager;
        private MicrosoftGraphUserStore graph;
        private string accessToken;
        public ApplicationUserManager(PortfolioContext portfolioContext, IUserStore<ApplicationUser> store, PortfolioRoleManager roleManager, string accessToken) : base(store)
        {
            this.portfolioContext = portfolioContext;
            this.roleManager = roleManager;
            if (accessToken != null)
            {
                this.accessToken = accessToken;
                graph = new MicrosoftGraphUserStore(roleManager);
            }
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            string activeDirectoryAccessToken = null;
            var portfolioContext = context.Get<PortfolioContext>();
            var roleManager = new PortfolioRoleManager(portfolioContext);

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
            var manager = new ApplicationUserManager(portfolioContext, new UserStore(portfolioContext, roleManager), roleManager, activeDirectoryAccessToken);

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override async Task<ApplicationUser> FindAsync(string userName, string password)
        {
            ApplicationUser user = null;
            if (accessToken != null)
            {
                var aduser = await graph.GetUserForAccessToken(accessToken);
                if (aduser.companyName == "Food Standards Agency")
                {
                    user = new ApplicationUser()
                    {
                        Id = aduser.id,
                        AccessGroupViewKey = aduser.companyName == "Food Standards Agency" ? AccessGroupConstants.FSAViewKey : null,
                        ActiveDirectoryUserId = aduser.id,
                        UserName = aduser.userPrincipalName
                    };
                }
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
            return roles == null ? new List<string>() : roles.Select(r => r.ViewKey).ToList();
        }

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