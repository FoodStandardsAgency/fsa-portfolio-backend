using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public partial class SyncMaps
    {
        public static readonly Dictionary<string, Dictionary<string, string>> phaseKeyMap = new Dictionary<string, Dictionary<string, string>>()
        {
            { "odd", odd_phaseKeyMap },
            { "serd", serd_phaseKeyMap }
        };

        public static Dictionary<string, string> odd_phaseKeyMap => new Dictionary<string, string>()
        {
            { "backlog", $"{ViewKeyPrefix.Phase}0" },
            { "discovery", $"{ViewKeyPrefix.Phase}1" },
            { "alpha", $"{ViewKeyPrefix.Phase}2" },
            { "beta", $"{ViewKeyPrefix.Phase}3" },
            { "live", $"{ViewKeyPrefix.Phase}4" },
            { "completed", $"{ViewKeyPrefix.Phase}5" },
        };
        public static Dictionary<string, string> serd_phaseKeyMap => new Dictionary<string, string>()
        {
            { "dev", $"{ViewKeyPrefix.Phase}0" },
            { "dec", $"{ViewKeyPrefix.Phase}1" },
            { "wai", $"{ViewKeyPrefix.Phase}2" },
            { "und", $"{ViewKeyPrefix.Phase}3" },
            { "com", $"{ViewKeyPrefix.Phase}5" },
        };

    }
}