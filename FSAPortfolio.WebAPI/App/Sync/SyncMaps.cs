using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public class SyncMaps
    {
        public static readonly Dictionary<Tuple<string, string>, string> phaseMap = new Dictionary<Tuple<string, string>, string>()
        {
            { new Tuple<string, string>("odd", "backlog"), PhaseConstants.BacklogName },
            { new Tuple<string, string>("odd", "discovery"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("odd", "alpha"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("odd", "beta"), PhaseConstants.BetaName },
            { new Tuple<string, string>("odd", "live"), PhaseConstants.LiveName },
            { new Tuple<string, string>("odd", "completed"), PhaseConstants.CompletedName },

            { new Tuple<string, string>("serd", "backlog"), "In development" },
            { new Tuple<string, string>("serd", "discovery"), "Awaiting decision" },
            { new Tuple<string, string>("serd", "alpha"), "Waiting to start" },
            { new Tuple<string, string>("serd", "beta"), "Underway" },
            { new Tuple<string, string>("serd", "completed"), "Complete" },

            { new Tuple<string, string>("abc", "discovery"), "Feasibility" },
            { new Tuple<string, string>("abc", "alpha"), "Appraise & select" },
            { new Tuple<string, string>("abc", "beta"), "Define" },
            { new Tuple<string, string>("abc", "live"), "Deliver" },
            { new Tuple<string, string>("abc", "completed"), "Embed/close" },

            { new Tuple<string, string>("fhp", "backlog"), PhaseConstants.BacklogName },
            { new Tuple<string, string>("fhp", "discovery"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("fhp", "alpha"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("fhp", "beta"), PhaseConstants.BetaName },
            { new Tuple<string, string>("fhp", "live"), PhaseConstants.LiveName },
            { new Tuple<string, string>("fhp", "completed"), PhaseConstants.CompletedName },

            { new Tuple<string, string>("otp", "backlog"), PhaseConstants.BacklogName },
            { new Tuple<string, string>("otp", "discovery"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("otp", "alpha"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("otp", "beta"), PhaseConstants.BetaName },
            { new Tuple<string, string>("otp", "live"), PhaseConstants.LiveName },
            { new Tuple<string, string>("otp", "completed"), PhaseConstants.CompletedName },

            { new Tuple<string, string>("test", "backlog"), PhaseConstants.BacklogName },
            { new Tuple<string, string>("test", "discovery"), PhaseConstants.DiscoveryName },
            { new Tuple<string, string>("test", "alpha"), PhaseConstants.AlphaName },
            { new Tuple<string, string>("test", "beta"), PhaseConstants.BetaName },
            { new Tuple<string, string>("test", "live"), PhaseConstants.LiveName },
            { new Tuple<string, string>("test", "completed"), PhaseConstants.CompletedName }

        };
        public static readonly Dictionary<string, string> onholdMap = new Dictionary<string, string>()
        {
            { "n", OnHoldConstants.NoName },
            { "y", OnHoldConstants.OnHoldName },
            { "b", OnHoldConstants.BlockedName },
            { "c", OnHoldConstants.CovidName }
        };
        public static readonly Dictionary<string, string> ragMap = new Dictionary<string, string>()
        {
            { "red", RagConstants.RedName },
            { "amb", RagConstants.AmberName },
            { "gre", RagConstants.GreenName },
            { "nor", RagConstants.NoneName }
        };
        public static readonly Dictionary<string, string> categoryMap = new Dictionary<string, string>()
        {
            { "cap", CategoryConstants.CapabilityName },
            { "data", CategoryConstants.DataName },
            { "sm", CategoryConstants.ServiceMgmtName },
            { "ser", CategoryConstants.SupportName },
            { "it", CategoryConstants.ITName },
            { "res", CategoryConstants.ResilienceName }
        };
        public static readonly Dictionary<string, string> sizeMap = new Dictionary<string, string>()
        {
            { "s", ProjectSizeConstants.SmallName },
            { "m", ProjectSizeConstants.MediumName },
            { "l", ProjectSizeConstants.LargeName },
            { "x", ProjectSizeConstants.ExtraLargeName }
        };
        public static readonly Dictionary<string, string> budgetTypeMap = new Dictionary<string, string>()
        {
            { "admin", BudgetTypeConstants.AdminName },
            { "progr", BudgetTypeConstants.ProgrammeName },
            { "capit", BudgetTypeConstants.CapitalName }
        };
    }
}