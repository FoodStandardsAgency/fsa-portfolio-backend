using FSAPortfolio.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
