﻿using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public class SyncMaps
    {
        public static readonly Dictionary<string, Tuple<string, int>> teamNameMap = new Dictionary<string, Tuple<string, int>>()
        {
            { "Data", new Tuple<string, int>("Data", 1) },
            { "Digital", new Tuple<string, int>("Digital", 2) },
            { "KIM", new Tuple<string, int>("Knowledge and Information Management and Security", 3) },
            { "IT", new Tuple<string, int>("FSA IT", 4) }
        };


        public static readonly Dictionary<string, string> accessGroupKeyMap = new Dictionary<string, string>()
        {
            { "1", AccessGroupConstants.FSAViewKey },
            { "2", AccessGroupConstants.EditorViewKey },
            { "3", AccessGroupConstants.AdminViewKey },
            { "4", AccessGroupConstants.SupplierViewKey },
            { "5", AccessGroupConstants.TeamLeaderViewKey }
        };

        public static readonly Dictionary<string, string> directorateKeyMap = new Dictionary<string, string>()
        {
            { "IR", "inc" },
            { "FO", "field" },
            { "FP", "finance" },
            { "FSP", "policy" },
            { "PEOP", "people" },
            { "RC", "comp" },
            { "SERD", "science" },
            { "SLG", "strategy" },
            { "WAL", "wales" }
        };

        public static readonly Dictionary<string, string> budgetTypeKeyMap = new Dictionary<string, string>()
        {
            { "none", $"{ViewKeyPrefix.BudgetType}0" },
            { "admin", $"{ViewKeyPrefix.BudgetType}1" },
            { "progr", $"{ViewKeyPrefix.BudgetType}2" },
            { "capit", $"{ViewKeyPrefix.BudgetType}3"}
        };
        public static readonly Dictionary<string, string> sizeKeyMap = new Dictionary<string, string>()
        {
            { string.Empty, $"{ViewKeyPrefix.ProjectSize}0" },
            { "o", $"{ViewKeyPrefix.ProjectSize}0" },
            { "s", $"{ViewKeyPrefix.ProjectSize}1" },
            { "m", $"{ViewKeyPrefix.ProjectSize}2" },
            { "l", $"{ViewKeyPrefix.ProjectSize}3" },
            { "x", $"{ViewKeyPrefix.ProjectSize}4" }
        };
        public static readonly Dictionary<string, string> onholdKeyMap = new Dictionary<string, string>()
        {
            { "n", $"{ViewKeyPrefix.Status}0" },
            { "y", $"{ViewKeyPrefix.Status}1" },
            { "b", $"{ViewKeyPrefix.Status}2" },
            { "c", $"{ViewKeyPrefix.Status}3" }
        };
        public static readonly Dictionary<string, string> categoryKeyMap = new Dictionary<string, string>()
        {
            { "cap", $"{ViewKeyPrefix.Category}0" },
            { "data", $"{ViewKeyPrefix.Category}1" },
            { "sm", $"{ViewKeyPrefix.Category}2" },
            { "ser", $"{ViewKeyPrefix.Category}3" },
            { "it", $"{ViewKeyPrefix.Category}4" },
            { "res", $"{ViewKeyPrefix.Category}5" },
        };
        public static readonly Dictionary<string, string> phaseKeyMap = new Dictionary<string, string>()
        {
            { "backlog", $"{ViewKeyPrefix.Phase}0" },
            { "discovery", $"{ViewKeyPrefix.Phase}1" },
            { "alpha", $"{ViewKeyPrefix.Phase}2" },
            { "beta", $"{ViewKeyPrefix.Phase}3" },
            { "live", $"{ViewKeyPrefix.Phase}4" },
            { "completed", $"{ViewKeyPrefix.Phase}5" },
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
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Phase}5"), "Complete" },

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
        public static readonly Dictionary<string, string> onholdMap = new Dictionary<string, string>()
        {
            { $"{ViewKeyPrefix.Status}0", OnHoldConstants.NoName },
            { $"{ViewKeyPrefix.Status}1", OnHoldConstants.OnHoldName },
            { $"{ViewKeyPrefix.Status}2", OnHoldConstants.BlockedName },
            { $"{ViewKeyPrefix.Status}3", OnHoldConstants.CovidName }
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
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}0"), CategoryConstants.CapabilityName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}1"), CategoryConstants.DataName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}2"), CategoryConstants.ServiceMgmtName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}3"), CategoryConstants.SupportName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}4"), CategoryConstants.ITName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}5"), CategoryConstants.ResilienceName },

            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}0"), "Best regulator"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}1"), "Food hypersensitivity"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}2"), "Foodborne disease"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}3"), "Chemical contaminants"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}4"), "Novel food and processes"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}5"), "Antimicrobial resistance"},

            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}0"), "Category / Swimlane 1"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}1"), "Category / Swimlane 2"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}2"), "Category / Swimlane 3"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}3"), "Category / Swimlane 4"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}4"), "Category / Swimlane 5"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}5"), "Category / Swimlane 6"}

        };
        public static readonly Dictionary<string, string> sizeMap = new Dictionary<string, string>()
        {
            { $"{ViewKeyPrefix.ProjectSize}0", ProjectSizeConstants.NotSetName },
            { $"{ViewKeyPrefix.ProjectSize}1", ProjectSizeConstants.SmallName },
            { $"{ViewKeyPrefix.ProjectSize}2", ProjectSizeConstants.MediumName },
            { $"{ViewKeyPrefix.ProjectSize}3", ProjectSizeConstants.LargeName },
            { $"{ViewKeyPrefix.ProjectSize}4", ProjectSizeConstants.ExtraLargeName }
        };

        public static readonly Dictionary<string, string> budgetTypeMap = new Dictionary<string, string>()
        {
            { $"{ViewKeyPrefix.BudgetType}0", BudgetTypeConstants.NotSetName },
            { $"{ViewKeyPrefix.BudgetType}1", BudgetTypeConstants.AdminName },
            { $"{ViewKeyPrefix.BudgetType}2", BudgetTypeConstants.ProgrammeName },
            { $"{ViewKeyPrefix.BudgetType}3", BudgetTypeConstants.CapitalName }
        };
    }
}