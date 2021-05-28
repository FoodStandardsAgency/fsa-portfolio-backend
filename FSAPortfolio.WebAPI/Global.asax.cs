using FSAPortfolio.Security.ApiKey;
using FSAPortfolio.Common.Logging;
using FSAPortfolio.WebAPI.App.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace FSAPortfolio.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AppLog.TraceInformation("Portfolio application starting...");
            AppLog.Indent();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new ApiKeyMessageHandler());
            PortfolioMapper.Configure();
            AppLog.Unindent();
            AppLog.TraceInformation("Portfolio application started.");
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            AppLog.Trace(ex);
        }
    }
}
