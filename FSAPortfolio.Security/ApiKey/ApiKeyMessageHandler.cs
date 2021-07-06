using FSAPortfolio.Entities.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FSAPortfolio.Security.ApiKey
{
    public class ApiKeyMessageHandler : DelegatingHandler
    {
        private static string APIKeyToCheck = ConfigurationManager.AppSettings["APIKey"];
        private static string PowerBIAPIKeyToCheck = ConfigurationManager.AppSettings["PowerBIAPIKey"];
        private static string PowerBIRoles = ConfigurationManager.AppSettings["PowerBIRoles"];
        private static string AdminAPIKeyToCheck = ConfigurationManager.AppSettings["AdminAPIKey"];
        private static string AdminRoles = ConfigurationManager.AppSettings["AdminRoles"];
        private static string TestAPIKeyToCheck = ConfigurationManager.AppSettings["TestAPIKey"];
        private static string TestRoles = ConfigurationManager.AppSettings["TestRoles"];

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validKey = checkAPIKey(httpRequestMessage, "APIKey", APIKeyToCheck);
            validKey = validKey || checkAPIKey(httpRequestMessage, "PowerBIAPIKey", PowerBIAPIKeyToCheck, PowerBIRoles);
            validKey = validKey || checkAPIKey(httpRequestMessage, "AdminAPIKey", AdminAPIKeyToCheck, AdminRoles);
            validKey = validKey || checkAPIKey(httpRequestMessage, "TestAPIKey", TestAPIKeyToCheck, TestRoles);

            if (!validKey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden);
            }
            else
            {
                return await base.SendAsync(httpRequestMessage, cancellationToken);
            }
        }

        private bool checkAPIKey(HttpRequestMessage httpRequestMessage, string keyName, string keyValue, string roles = null)
        {
            bool validKey = false;
            IEnumerable<string> apiRequestHeaders = null;
            var checkApiKeyExists = !string.IsNullOrWhiteSpace(keyValue) && httpRequestMessage.Headers.TryGetValues(keyName, out apiRequestHeaders);
            if (checkApiKeyExists)
            {
                if (apiRequestHeaders.FirstOrDefault().Equals(keyValue))
                { 
                    validKey = true;

                    // Add the roles
                    if (roles != null)
                    {
                        var principal = Thread.CurrentPrincipal;
                        var identity = principal.Identity as ClaimsIdentity;
                        var rolesToAdd = roles.Split(',', ';', '|').Select(r => new Role(r));
                        foreach (var role in rolesToAdd)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, role.ViewKey));
                        }
                    }
                }
            }
            return validKey;

        }
    }
}
