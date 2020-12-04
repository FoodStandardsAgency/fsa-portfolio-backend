using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public partial class SyncMaps
    {
        public static readonly Dictionary<string, Dictionary<string, string>> budgetTypeKeyMap = new Dictionary<string, Dictionary<string, string>>()
        {
            { "odd", odd_budgetTypeKeyMap },
            { "abc", odd_budgetTypeKeyMap },
            { "fhp", odd_budgetTypeKeyMap },
            { "otp", odd_budgetTypeKeyMap },
            { "test", odd_budgetTypeKeyMap },
            { "serd", serd_budgetTypeKeyMap }
        };

        public static Dictionary<string, string> odd_budgetTypeKeyMap => new Dictionary<string, string>()
        {
            { "none", $"{ViewKeyPrefix.BudgetType}0" },
            { "admin", $"{ViewKeyPrefix.BudgetType}1" },
            { "progr", $"{ViewKeyPrefix.BudgetType}2" },
            { "capit", $"{ViewKeyPrefix.BudgetType}3"}
        };
        public static Dictionary<string, string> serd_budgetTypeKeyMap => new Dictionary<string, string>()
        {
            { "none", $"{ViewKeyPrefix.BudgetType}0" },
            { "admin", $"{ViewKeyPrefix.BudgetType}1" },
            { "sef", $"{ViewKeyPrefix.BudgetType}2" },
            { "poth", $"{ViewKeyPrefix.BudgetType}3"},
            { "oth", $"{ViewKeyPrefix.BudgetType}4"}
        };


        // TODO: look at categories - need to do same here - with lookup on portfolio
        public static readonly Dictionary<Tuple<string, string>, string> budgetTypeMap = new Dictionary<Tuple<string, string>, string>()
        {
            { new Tuple<string, string>("odd", ViewKeyPrefix.BudgetTypeNotSetViewKey), BudgetTypeConstants.NotSetName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.BudgetType}1"), BudgetTypeConstants.AdminName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.BudgetType}2"), BudgetTypeConstants.ProgrammeName },
            { new Tuple<string, string>("odd", $"{ViewKeyPrefix.BudgetType}3"), BudgetTypeConstants.CapitalName },

            { new Tuple<string, string>("serd", ViewKeyPrefix.BudgetTypeNotSetViewKey), BudgetTypeConstants.NotSetName },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.BudgetType}1"), BudgetTypeConstants.AdminName },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.BudgetType}2"), "Programme - SEF" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.BudgetType}3"), "Programme - other" },
            { new Tuple<string, string>("serd", $"{ViewKeyPrefix.BudgetType}4"), "Other / TBC" }
        };

    }
}