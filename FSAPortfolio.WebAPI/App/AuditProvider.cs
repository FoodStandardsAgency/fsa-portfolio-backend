using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public class AuditProvider
    {
        public static void LogChanges<T>(PortfolioContext context, Func<DateTime, string, T> logFactory, DbSet<T> dataset, DateTime timestamp)
            where T : class
        {
            T log = null;
            log = buildLog(context, logFactory, timestamp, log);
            if (log != null) dataset.Add(log);
        }
        public static void LogChanges<T>(PortfolioContext context, Func<DateTime, string, T> logFactory, ICollection<T> collection, DateTime timestamp)
            where T : class
        {
            T log = null;
            log = buildLog(context, logFactory, timestamp, log);
            if (log != null) collection.Add(log);
        }

        private static T buildLog<T>(PortfolioContext context, Func<DateTime, string, T> logFactory, DateTime timestamp, T log) where T : class
        {
            var changes = context.ChangeTracker.Entries().Where(c => c.State == EntityState.Modified);
            if (changes.Count() > 0)
            {
                var logText = new List<string>();
                foreach (var change in changes)
                {
                    var originalValues = change.OriginalValues;
                    var currentValues = change.CurrentValues;
                    builLog(logText, originalValues, currentValues);
                }
                string text = string.Join("; ", logText);
                log = logFactory(timestamp, text);
            }

            return log;
        }

        private static void builLog(List<string> logText, DbPropertyValues originalValues, DbPropertyValues currentValues)
        {
            foreach (string pname in originalValues.PropertyNames)
            {
                var originalValue = originalValues[pname];
                var currentValue = currentValues[pname];
                if(!(originalValue == null && currentValue == null))
                {
                    string origString = getStringValue(originalValue);
                    string currentString = getStringValue(currentValue);
                    if (!string.Equals(origString, currentString))
                    {
                        string log = $"{pname}: [{origString}] to [{currentString}]";
                        logText.Add(log);
                    }
                }
            }
        }
        private static string getStringValue(object value)
        {
            if (value == null) return null;
            if (value is DbPropertyValues)
            {
                var pvalues = value as DbPropertyValues;
                var list = new List<string>();
                foreach (var pname in pvalues.PropertyNames)
                {
                    var pvalue = getStringValue(pvalues[pname]);
                    if (!string.IsNullOrWhiteSpace(pvalue))
                    {
                        list.Add($"{pname}: [{pvalue}]");
                    }
                }
                return (list.Count > 0) ? string.Join(", ", list) : null;
            }
            else if (value is ProjectDateFlags)
            {
                return ((int)value) > 0 ? value.ToString() : null;
            }
            else if (value is decimal)
            {
                var dec = (decimal)value;
                return dec > 0m ? dec.ToString() : null;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}