using FSAPortfolio.Common.Logging;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.App.Identity;
using FSAPortfolio.WebAPI.App.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace FSAPortfolio.Application.Services
{
    public abstract class BaseService
    {
        private readonly IServiceContext serviceContext;
        public IServiceContext ServiceContext => serviceContext;
        protected BaseService(IServiceContext context)
        {
#if DEBUG
            AppLog.TraceVerbose($"{this.GetType().Name} created.");
#endif

            this.serviceContext = context;
        }





    }
}
