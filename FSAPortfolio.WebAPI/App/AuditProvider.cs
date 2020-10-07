using FSAPortfolio.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                    foreach (string pname in originalValues.PropertyNames)
                    {
                        var originalValue = originalValues[pname];
                        var currentValue = currentValues[pname];
                        if (!Equals(originalValue, currentValue))
                        {
                            logText.Add($"{pname}: [{originalValue}] to [{currentValue}]");
                        }
                    }
                }
                string text = string.Join("; ", logText);
                log = logFactory(timestamp, text);
            }

            return log;
        }

    }
}