using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public class SyncMaps
    {
        public static readonly Dictionary<string, string> phaseMap = new Dictionary<string, string>()
        {
            { "backlog", PhaseConstants.BacklogName },
            { "discovery", PhaseConstants.DiscoveryName },
            { "alpha", PhaseConstants.AlphaName },
            { "beta", PhaseConstants.BetaName },
            { "live", PhaseConstants.LiveName },
            { "completed", PhaseConstants.CompletedName }
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