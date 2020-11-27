using FSAPortfolio.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public override bool SupportsUserRole => true;
        public override bool SupportsUserClaim => true;
        public override bool SupportsUserPassword => true;

        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore(context.Get<PortfolioContext>()));

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public override Task<ApplicationUser> FindAsync(string userName, string password)
        {
            return base.FindAsync(userName, password);
        }


        // E.g. ODD.Admin, ODD.Editor
        public override async Task<IList<string>> GetRolesAsync(string userId)
        {
            // TODO: stub
            return new List<string>() { "ODD.Admin" };
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