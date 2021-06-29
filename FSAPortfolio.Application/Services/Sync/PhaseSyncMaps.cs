using FSAPortfolio.Common;
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
            { "backlog", PhaseConstants.BacklogViewKey },
            { "discovery", $"{ViewKeyPrefix.Phase}1" },
            { "alpha", $"{ViewKeyPrefix.Phase}2" },
            { "beta", $"{ViewKeyPrefix.Phase}3" },
            { "live", PhaseConstants.ArchiveViewKey },
            { "completed", PhaseConstants.CompletedViewKey },
        };
        public static Dictionary<string, string> serd_phaseKeyMap => new Dictionary<string, string>()
        {
            { "dev", PhaseConstants.BacklogViewKey },
            { "dec", $"{ViewKeyPrefix.Phase}1" },
            { "wai", $"{ViewKeyPrefix.Phase}2" },
            { "und", $"{ViewKeyPrefix.Phase}3" },
            { "com", PhaseConstants.ArchiveViewKey },
        };

        public static readonly Dictionary<Tuple<string, string>, string> phaseMap = new Dictionary<Tuple<string, string>, string>()
        {
            { new Tuple<string, string>("odd", PhaseConstants.BacklogViewKey), PhaseConstants.BacklogName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}2"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Phase}3"), PhaseConstants.BetaName },
            { new Tuple<string, string>("odd", PhaseConstants.ArchiveViewKey), PhaseConstants.LiveName },
            { new Tuple<string, string>("odd", PhaseConstants.CompletedViewKey), PhaseConstants.CompletedName },

            { new Tuple<string, string>("serd", PhaseConstants.BacklogViewKey), "In development" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}1"), "Awaiting decision" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}2"), "Waiting to start" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}3"), "Underway" },
            { new Tuple<string, string>("serd", PhaseConstants.ArchiveViewKey), "Complete" },
            { new Tuple<string, string>("serd", PhaseConstants.CompletedViewKey), "Archive" },

            { new Tuple<string, string>("abc", $"{ViewKeyPrefix.Phase}1"), "Feasibility" },
            { new Tuple<string, string>("abc", $"{ViewKeyPrefix.Phase}2"), "Appraise & select" },
            { new Tuple<string, string>("abc", $"{ViewKeyPrefix.Phase}3"), "Define" },
            { new Tuple<string, string>("abc", PhaseConstants.ArchiveViewKey), "Deliver" },
            { new Tuple<string, string>("abc", PhaseConstants.CompletedViewKey), "Embed/close" },

            { new Tuple<string, string>("fhp", PhaseConstants.BacklogViewKey), PhaseConstants.BacklogName },
            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}2"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("fhp", $"{ViewKeyPrefix.Phase}3"), PhaseConstants.BetaName },
            { new Tuple<string, string>("fhp", PhaseConstants.ArchiveViewKey), PhaseConstants.LiveName },
            { new Tuple<string, string>("fhp", PhaseConstants.CompletedViewKey), PhaseConstants.CompletedName },

            { new Tuple<string, string>("otp", PhaseConstants.BacklogViewKey), PhaseConstants.BacklogName },
            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}2"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("otp", $"{ViewKeyPrefix.Phase}3"), PhaseConstants.BetaName },
            { new Tuple<string, string>("otp", PhaseConstants.ArchiveViewKey), PhaseConstants.LiveName },
            { new Tuple<string, string>("otp", PhaseConstants.CompletedViewKey), PhaseConstants.CompletedName },

            { new Tuple<string, string>("test", PhaseConstants.BacklogViewKey), PhaseConstants.BacklogName },
            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}2"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("test", $"{ViewKeyPrefix.Phase}3"), PhaseConstants.BetaName },
            { new Tuple<string, string>("test", PhaseConstants.ArchiveViewKey), PhaseConstants.LiveName },
            { new Tuple<string, string>("test", PhaseConstants.CompletedViewKey), PhaseConstants.CompletedName },

            { new Tuple<string, string>("dev", PhaseConstants.BacklogViewKey), PhaseConstants.BacklogName },
            { new Tuple<string, string>("dev", $"{ViewKeyPrefix.Phase}1"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("dev", $"{ViewKeyPrefix.Phase}2"), "Implementation" },
            { new Tuple<string, string>("dev", $"{ViewKeyPrefix.Phase}3"), "Testing" },
            { new Tuple<string, string>("dev", PhaseConstants.ArchiveViewKey), PhaseConstants.LiveName },
            { new Tuple<string, string>("dev", PhaseConstants.CompletedViewKey), PhaseConstants.CompletedName }


        };


    }
}