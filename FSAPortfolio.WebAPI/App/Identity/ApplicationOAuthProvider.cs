using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Identity
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
                CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager, string authenticationType)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);



        //    // Add custom user claims here
        //    // TODO: add portfolio access claims
        //    string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        //    string UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        //    string UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        //    string SecurityStampClaimType = "AspNet.Identity.SecurityStamp";

        //    ClaimsIdentity id = new ClaimsIdentity(authenticationType, UserNameClaimType, RoleClaimType);
        //    //id.AddClaim(new Claim(UserIdClaimType, ConvertIdToString(user.Id), "http://www.w3.org/2001/XMLSchema#string"));
        //    id.AddClaim(new Claim(UserNameClaimType, user.UserName, "http://www.w3.org/2001/XMLSchema#string"));
        //    id.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"));
        //    if (manager.SupportsUserSecurityStamp)
        //    {
        //        ClaimsIdentity claimsIdentity = id;
        //        string securityStampClaimType = SecurityStampClaimType;
        //        claimsIdentity.AddClaim(new Claim(securityStampClaimType, await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture()));
        //    }
        //    if (manager.SupportsUserRole)
        //    {
        //        foreach (string item in await manager.GetRolesAsync(user.Id).WithCurrentCulture())
        //        {
        //            id.AddClaim(new Claim(RoleClaimType, item, "http://www.w3.org/2001/XMLSchema#string"));
        //        }
        //    }
        //    if (manager.SupportsUserClaim)
        //    {
        //        ClaimsIdentity claimsIdentity = id;
        //        claimsIdentity.AddClaims(await manager.GetClaimsAsync(user.Id).WithCurrentCulture());
        //    }


        //    return userIdentity;
        //}

    }
}