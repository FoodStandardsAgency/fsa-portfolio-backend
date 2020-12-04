using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public partial class SyncMaps
    {
        public static readonly Dictionary<string, Dictionary<string, string>> categoryKeyMap = new Dictionary<string, Dictionary<string, string>>()
        {
            { "odd", odd_categoryKeyMap },
            { "serd", serd_categoryKeyMap }
        };

        public static readonly Dictionary<string, string> odd_categoryKeyMap = new Dictionary<string, string>()
        {
            { "cap", $"{ViewKeyPrefix.Category}0" },
            { "data", $"{ViewKeyPrefix.Category}1" },
            { "sm", $"{ViewKeyPrefix.Category}2" },
            { "ser", $"{ViewKeyPrefix.Category}3" },
            { "it", $"{ViewKeyPrefix.Category}4" },
            { "res", $"{ViewKeyPrefix.Category}5" },
        };

        public static readonly Dictionary<string, string> serd_categoryKeyMap = new Dictionary<string, string>()
        {
            { "02", $"{ViewKeyPrefix.Category}0" },
            { "04", $"{ViewKeyPrefix.Category}1" },
            { "03", $"{ViewKeyPrefix.Category}2" },
            { "07", $"{ViewKeyPrefix.Category}3" },
            { "05", $"{ViewKeyPrefix.Category}4" },
            { "09", $"{ViewKeyPrefix.Category}5" },
            { "11", $"{ViewKeyPrefix.Category}6" },
            { "06", $"{ViewKeyPrefix.Category}7" },
            { "08", $"{ViewKeyPrefix.Category}8" },
            { "01", $"{ViewKeyPrefix.Category}9" },
            { "10", $"{ViewKeyPrefix.Category}10" },
            { "12", $"{ViewKeyPrefix.Category}11" },
            { "13", $"{ViewKeyPrefix.Category}12" }
        };
    }
}