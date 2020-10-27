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

            // TODO: change the view keys for other portfolios to match labels (can't at moment as it affects syncing)
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
        public static readonly Dictionary<string, Tuple<string, int>> ragMap = new Dictionary<string, Tuple<string, int>>()
        {
            { RagConstants.NoneViewKey, new Tuple<string, int>(RagConstants.NoneName, 0) },
            { RagConstants.RedViewKey, new Tuple<string, int>(RagConstants.RedName, 1) },
            { RagConstants.RedAmberViewKey, new Tuple<string, int>(RagConstants.RedAmberName, 2) },
            { RagConstants.AmberViewKey, new Tuple<string, int>(RagConstants.AmberName, 3) },
            { RagConstants.AmberGreenViewKey, new Tuple<string, int>(RagConstants.AmberGreenName, 4) },
            { RagConstants.GreenViewKey, new Tuple<string, int>(RagConstants.GreenName, 5) },
        };
        public static readonly Dictionary<Tuple<string, string>, string> categoryMap = new Dictionary<Tuple<string, string>, string>()
        {
            { new Tuple<string, string>("odd", "data"), CategoryConstants.DataName },
            { new Tuple<string, string>("odd", "cap"), CategoryConstants.CapabilityName },
            { new Tuple<string, string>("odd", "ser"), CategoryConstants.SupportName },
            { new Tuple<string, string>("odd", "it"), CategoryConstants.ITName },
            { new Tuple<string, string>("odd", "res"), CategoryConstants.ResilienceName },
            { new Tuple<string, string>("odd", "sm"), CategoryConstants.ServiceMgmtName },

            { new Tuple<string, string>("serd", "cap"), "Best regulator"},
            { new Tuple<string, string>("serd", "data"), "Food hypersensitivity"},
            { new Tuple<string, string>("serd", "sm"), "Foodborne disease"},
            { new Tuple<string, string>("serd", "ser"), "Chemical contaminants"},
            { new Tuple<string, string>("serd", "it"), "Novel food and processes"},
            { new Tuple<string, string>("serd", "res"), "Antimicrobial resistance"},

            { new Tuple<string, string>("abc","cap"), "Category / Swimlane 1"},
            { new Tuple<string, string>("abc","data"), "Category / Swimlane 2"},
            { new Tuple<string, string>("abc","sm"), "Category / Swimlane 3"},
            { new Tuple<string, string>("abc","ser"), "Category / Swimlane 4"},
            { new Tuple<string, string>("abc","it"), "Category / Swimlane 5"},
            { new Tuple<string, string>("abc","res"), "Category / Swimlane 6"}

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
            { "none", BudgetTypeConstants.NotSetName },
            { "admin", BudgetTypeConstants.AdminName },
            { "progr", BudgetTypeConstants.ProgrammeName },
            { "capit", BudgetTypeConstants.CapitalName }
        };
    }
}