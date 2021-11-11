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

        public static int ProjectDateMinYearOffset = GetIntSetting("ProjectDate.MinOffset", 5);
        public static int ProjectDateMaxYearOffset = GetIntSetting("ProjectDate.MaxOffset", 10);

        public static int ProjectUpdateOverdueDays = GetIntSetting("Projects.UpdateOverdueDays", 14);
        public static string[] ProjectActivePhaseViewKeys = GetStringArraySetting("Projects.ProjectActivePhaseViewKeys", "phase1|phase2|phase3");
        public static string[] ProjectBacklogPhaseViewKeys = GetStringArraySetting("Projects.ProjectStartDatePhaseViewKeys", "phase0");

        private static int GetIntSetting(string key, int? defaultValue)
        {
            int result;
            var setting = ConfigurationManager.AppSettings[key];
            if (setting == null || !int.TryParse(setting, out result)) result = defaultValue ?? 0;
            return result;
        }
        private static string[] GetStringArraySetting(string key, string defaultValue)
        {
            string[] result = null;
            var setting = ConfigurationManager.AppSettings[key] ?? defaultValue;
            if (setting != null) result = setting.Split('|', ';');
            return result;
        }
    }
}