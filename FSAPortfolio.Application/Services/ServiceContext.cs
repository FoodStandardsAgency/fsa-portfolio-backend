using FSAPortfolio.Entities;
using FSAPortfolio.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.Application.Services
{
    public class ServiceContext : IServiceContext
    {
        private Lazy<PortfolioContext> lazyPortfolioContext;
        public PortfolioContext PortfolioContext => lazyPortfolioContext.Value;
        public ServiceContext(Lazy<PortfolioContext> lazyPortfolioContext)
        {
            this.lazyPortfolioContext = lazyPortfolioContext;
#if DEBUG
            AppLog.TraceVerbose($"{nameof(ServiceContext)} created.");
#endif
        }
    }
}
