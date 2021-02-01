using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests
{
    internal static class TestSettings
    {
        internal static string APIKey = ConfigurationManager.AppSettings["APIKey"];
        internal static string TestUser = ConfigurationManager.AppSettings["TestUser"];
        internal static string TestUserPassword = ConfigurationManager.AppSettings["TestPassword"];
        internal static string TestPortfolio = ConfigurationManager.AppSettings["TestPortfolioViewKey"];
        internal static string TestStartPhases = ConfigurationManager.AppSettings["TestPortfolioPhases"];
        internal static string TestChanged_ReducedNumberPhases_Fail = ConfigurationManager.AppSettings["TestPortfolioPhases_RemoveFail"];
        
        internal static string AssignedPhase = ConfigurationManager.AppSettings["AssignedPhase"];
        


        internal static string TestProjectId = ConfigurationManager.AppSettings["TestProjectId"];

    }
}
