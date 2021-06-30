using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Common
{
    public static class ViewKeyPrefix
    {
        public const string Phase = "phase";
        public const string Status = "status";
        public const string Category = "category";
        public const string BudgetType = "budgettype";
        public const string ProjectSize = "size";

        public static string ProjectSizeNotSetViewKey = $"{ProjectSize}0";
        public static string BudgetTypeNotSetViewKey = $"{BudgetType}0";

    }
}