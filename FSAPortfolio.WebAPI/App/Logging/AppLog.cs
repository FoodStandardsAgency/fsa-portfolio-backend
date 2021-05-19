using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Logging
{
    public static class AppLog
    {
        public static void TraceInformation(string message)
        {
            System.Diagnostics.Trace.TraceInformation(message);
        }

        public static void Indent() => System.Diagnostics.Trace.Indent();
        public static void Unindent() => System.Diagnostics.Trace.Unindent();
    }
}