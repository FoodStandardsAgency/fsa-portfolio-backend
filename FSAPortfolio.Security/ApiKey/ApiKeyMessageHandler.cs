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

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validKey = false;
            IEnumerable<string> apiRequestHeaders, powerBiRequestHeaders;
            var checkApiKeyExists = httpRequestMessage.Headers.TryGetValues("APIKey", out apiRequestHeaders);
            var checkPowerBIApiKey = httpRequestMessage.Headers.TryGetValues("PowerBIAPIKey", out powerBiRequestHeaders);
            if (checkApiKeyExists)
            {
                if (apiRequestHeaders.FirstOrDefault().Equals(APIKeyToCheck))
                    validKey = true;
            }
            if (checkPowerBIApiKey)
            {
                if (powerBiRequestHeaders.FirstOrDefault().Equals(PowerBIAPIKeyToCheck))
                {
                    validKey = true;

                    // Add the Power BI roles
                    if (PowerBIRoles != null)
                    {
                        var principal = Thread.CurrentPrincipal;
                        var identity = principal.Identity as ClaimsIdentity;
                        foreach (var role in PowerBIRoles.Split(',', ';', '|'))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    }
                }
            }

            if (!validKey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden);
            }

            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
    }
}
