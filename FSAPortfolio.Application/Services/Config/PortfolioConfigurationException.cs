using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Services.Config
{
    public class PortfolioConfigurationException : Exception
    {
        public PortfolioConfigurationException(string message) : base(message)
        {
        }
        public PortfolioConfigurationException(string message, Exception e) : base(message, e)
        {
        }
    }
}