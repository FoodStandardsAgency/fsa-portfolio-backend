using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Application
{
    /// <summary>
    /// An exception indicating that the error is due to user action.
    /// The message is expected to be output to the user on the error page.
    /// </summary>
    public class PortfolioUserException : Exception
    {
        public PortfolioUserException(string message) : base(message)
        {
        }
    }
}
