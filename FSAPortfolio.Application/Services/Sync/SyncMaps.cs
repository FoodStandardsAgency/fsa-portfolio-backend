using FSAPortfolio.Common;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public partial class SyncMaps
    {
        public static readonly Dictionary<string, string> emailMap = new Dictionary<string, string>()
        {
            { "amy-kate.wych@food.gov.uk", "amy-kate.lynch@food.gov.uk" }
        };


        public static readonly Dictionary<string, Tuple<string, int>> teamNameMap = new Dictionary<string, Tuple<string, int>>()
        {
            { "Data", new Tuple<string, int>("Data", 1) },
            { "Digital", new Tuple<string, int>("Digital", 2) },
            { "KIM", new Tuple<string, int>("Knowledge and Information Management and Security", 3) },
            { "IT", new Tuple<string, int>("FSA IT", 4) }
        };

        public static readonly Dictionary<string, string[]> userRoleMap = new Dictionary<string, string[]>()
        {
            { "portfolio", new string[]{ "ODD.Admin" } },
            { "odd", new string[]{ "ODD.Editor" } }
        };

        public static readonly Dictionary<string, string> accessGroupKeyMap = new Dictionary<string, string>()
        {
            { "1", AccessGroupConstants.FSAViewKey },
            { "2", AccessGroupConstants.EditorViewKey },
            { "3", AccessGroupConstants.AdminViewKey },
            { "4", AccessGroupConstants.SupplierViewKey },
            { "5", AccessGroupConstants.EditorViewKey },
            { "6", AccessGroupConstants.SuperuserViewKey }
        };

        public static readonly Dictionary<string, string> oddLeadRoleMap = new Dictionary<string, string>()
        {
            { "oth", "Role" },
            { "sup", "Support" },
            { "imp", "Implementation" },
            { "man", "Management" }
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
        public static readonly Dictionary<string, string> sizeMap = new Dictionary<string, string>()
        {
            { ViewKeyPrefix.ProjectSizeNotSetViewKey, ProjectSizeConstants.NotSetName },
            { $"{ViewKeyPrefix.ProjectSize}1", ProjectSizeConstants.SmallName },
            { $"{ViewKeyPrefix.ProjectSize}2", ProjectSizeConstants.MediumName },
            { $"{ViewKeyPrefix.ProjectSize}3", ProjectSizeConstants.LargeName },
            { $"{ViewKeyPrefix.ProjectSize}4", ProjectSizeConstants.ExtraLargeName }
        };

    }
}