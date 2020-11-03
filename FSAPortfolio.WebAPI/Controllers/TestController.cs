using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.WebAPI.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {

        public async Task<HttpResponseMessage> Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
