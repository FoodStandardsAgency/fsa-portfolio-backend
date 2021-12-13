using FSAPortfolio.Common.Logging;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Application.Services.Identity;
using FSAPortfolio.Application.Services.Users;
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
    public abstract class BaseService : IBaseService
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

        public void TraceVerbose(string message)
        {
            if (serviceContext.CurrentUserName != null)
                AppLog.TraceVerbose($"{serviceContext.CurrentUserName}: {message}");
            else
                AppLog.TraceVerbose(message);

        }
    }
}
