using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Entities
{
    public static class PortfolioSettings
    {
        public static int NewProjectLimitDays = GetIntSetting("Projects.NewLimitDays", 14);
        public static int DefaultProjectArchiveAgeDays = GetIntSetting("Projects.DefaultArchiveAgeDays", 90);

        private static int GetIntSetting(string key, int? defaultValue)
        {
            int result;
            var setting = ConfigurationManager.AppSettings[key];
            if(setting == null || !int.TryParse(setting, out result)) result = defaultValue ?? 0;
            return result;
        }
    }
}