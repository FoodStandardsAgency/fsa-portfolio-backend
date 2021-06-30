using FSAPortfolio.Entities;
using FSAPortfolio.PostgreSQL;
using FSAPortfolio.WebAPI.App.Identity;
using FSAPortfolio.WebAPI.App.Microsoft;
using FSAPortfolio.WebAPI.App.Users;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IPersonService personService;
        private readonly IUserService userService;

        public UsersController(IPersonService personService, IUserService userService)
        {
            this.personService = personService;
            this.userService = userService;
        }

        // Get: api/Users/search
        [AcceptVerbs("GET")]
        [Authorize]
        public async Task<UserSearchResponseModel> SearchUsers([FromUri] string portfolio, [FromUri] string term, [FromUri(Name = "addnone")] bool includeNone = false)
        {
            return await userService.SearchUsersAsync(portfolio, term, includeNone);
        }

        // Get: api/Users/suppliers
        [AcceptVerbs("GET")]
        [Authorize]
        public async Task<SupplierResponseModel> GetSuppliers()
        {
            return await userService.GetSuppliersAsync();
        }

        // POST: api/Users/addsupplier
        [AcceptVerbs("POST")]
        [Authorize]
        public async Task<AddSupplierResponseModel> AddSupplier([FromBody] AddSupplierModel model)
        {
            return await personService.AddSupplierAsync(model.Portfolio, model.UserName, model.PasswordHash);
        }

        // POST: api/Users/LegacyADUsers
        [AcceptVerbs("POST")]
        [Authorize]
        public async Task<UserModel> GetADUser(UserRequestModel userRequest)
        {
            return await userService.GetADUserAsync(userRequest.UserName);
        }

        // POST: api/Users/legacy
        [AcceptVerbs("POST")]
        [Authorize]
        public async Task<UserModel> GetUser(UserRequestModel userRequest)
        {
            return await userService.GetUserAsync(userRequest.UserName, userRequest.PasswordHash);
        }

        [AcceptVerbs("GET")]
        public IdentityResponseModel GetIdentity()
        {
            IdentityResponseModel result = null;

            var principal = RequestContext.Principal;
            var identity = principal?.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                result = new IdentityResponseModel()
                {
                    UserId = principal.Identity.Name,
                    Roles = identity.Claims.Where(c => c.Type == identity.RoleClaimType).Select(c => c.Value.ToLower()).ToArray(),
                    AccessGroup = identity.Claims.SingleOrDefault(c => c.Type == ApplicationUser.AccessGroupClaimType)?.Value?.ToLower()
                };
            }
            return result;
        }

        [AcceptVerbs("POST")]
        public async Task CreateUser([FromBody] AddUserModel model)
        {
            var sha256 = SHA256.Create();
            var hash = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(model.Password))).Replace("-", "");
            await userService.CreateUser(model.UserName, hash, model.AccessGroup);
        }

        [HttpGet, Route("api/Users/ResetAD")]
        public async Task<HttpResponseMessage> ResetADReferences()
        {
            await personService.ResetADReferencesAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("AD user references successfully reset.", Encoding.Unicode);
            return response;
        }

        [HttpGet, Route("api/Users/RemoveDuplicates")]
        public async Task<HttpResponseMessage> RemoveDuplicates()
        {
            await personService.RemoveDuplicatesAsync();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("User list cleared of duplicates.", Encoding.Unicode);
            return response;
        }
    }
}
