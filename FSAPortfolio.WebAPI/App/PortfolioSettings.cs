using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public static class PortfolioSettings
    {
        public static int NewProjectLimitDays = GetIntSetting("Projects.NewLimitDays", 14);

        private static int GetIntSetting(string key, int? defaultValue)
        {
            int result;
            var setting = ConfigurationManager.AppSettings[key];
            if(setting == null || !int.TryParse(setting, out result)) result = 0;
            return result;
        }
    }
}