using FSAPortfolio.Entities.Projects;
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

        public static readonly Dictionary<Tuple<string, string>, string> phaseMap = new Dictionary<Tuple<string, string>, string>()
        {
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}0"), PhaseConstants.BacklogName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}2"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}3"), PhaseConstants.BetaName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}4"), PhaseConstants.LiveName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}5"), PhaseConstants.CompletedName },

            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}0"), "In development" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}1"), "Awaiting decision" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}2"), "Waiting to start" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}3"), "Underway" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}4"), "Complete" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}5"), "Archive" },

            { new Tuple<string, string>("abc", $"{ViewKeyPrefix.Phase}1"), "Feasibility" },
            { new Tuple<string, string>("abc", $"{ViewKeyPrefix.Phase}2"), "Appraise & select" },
            { new Tuple<string, string>("abc", $"{ViewKeyPrefix.Phase}3"), "Define" },
            { new Tuple<string, string>("abc", $"{ViewKeyPrefix.Phase}4"), "Deliver" },
            { new Tuple<string, string>("abc", $"{ViewKeyPrefix.Phase}5"), "Embed/close" },

            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}0"), PhaseConstants.BacklogName },
            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}2"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}3"), PhaseConstants.BetaName },
            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}4"), PhaseConstants.LiveName },
            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}5"), PhaseConstants.CompletedName },

            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}0"), PhaseConstants.BacklogName },
            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}2"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}3"), PhaseConstants.BetaName },
            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}4"), PhaseConstants.LiveName },
            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}5"), PhaseConstants.CompletedName },

            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}0"), PhaseConstants.BacklogName },
            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}2"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}3"), PhaseConstants.BetaName },
            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}4"), PhaseConstants.LiveName },
            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}5"), PhaseConstants.CompletedName }

        };


    }
}