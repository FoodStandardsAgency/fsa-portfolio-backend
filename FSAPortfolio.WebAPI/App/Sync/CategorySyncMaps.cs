using FSAPortfolio.Entities.Projects;
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
            { "abc", odd_categoryKeyMap },
            { "fhp", odd_categoryKeyMap },
            { "otp", odd_categoryKeyMap },
            { "test", odd_categoryKeyMap },
            { "serd", serd_categoryKeyMap }
        };

        public static Dictionary<string, string> odd_categoryKeyMap => new Dictionary<string, string>()
        {
            { "cap", $"{ViewKeyPrefix.Category}0" },
            { "data", $"{ViewKeyPrefix.Category}1" },
            { "sm", $"{ViewKeyPrefix.Category}2" },
            { "ser", $"{ViewKeyPrefix.Category}3" },
            { "it", $"{ViewKeyPrefix.Category}4" },
            { "res", $"{ViewKeyPrefix.Category}5" },
        };

        public static Dictionary<string, string> serd_categoryKeyMap => new Dictionary<string, string>()
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

        public static readonly Dictionary<Tuple<string, string>, string> categoryMap = new Dictionary<Tuple<string, string>, string>()
        {
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}0"), CategoryConstants.CapabilityName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}1"), CategoryConstants.DataName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}2"), CategoryConstants.ServiceMgmtName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}3"), CategoryConstants.SupportName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}4"), CategoryConstants.ITName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.Category}5"), CategoryConstants.ResilienceName },

            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}0"), "Food hypersensitivity"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}1"), "Chemical contaminants"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}2"), "Foodborne disease"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}3"), "Antimicrobial resistance"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}4"), "Nutrition and health"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}5"), "Consumer and business behaviour"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}6"), "Food crime"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}7"), "Novel food and processes"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}8"), "Data and digital"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}9"), "Best regulator"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}10"), "Emerging risks and opportunities"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}11"), "Not a current ARI"},
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.Category}12"), "Unknown"},

            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}0"), "Category / Swimlane 1"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}1"), "Category / Swimlane 2"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}2"), "Category / Swimlane 3"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}3"), "Category / Swimlane 4"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}4"), "Category / Swimlane 5"},
            { new Tuple<string, string>("abc",$"{ViewKeyPrefix.Category}5"), "Category / Swimlane 6"}

        };

    }
}