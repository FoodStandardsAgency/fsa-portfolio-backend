using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Logging
{
    public static class AppLog
    {
        public static void TraceInformation(string message) => System.Diagnostics.Trace.TraceInformation(message);
        public static void TraceWarning(string message) => System.Diagnostics.Trace.TraceWarning(message);
        public static void TraceError(string message) => System.Diagnostics.Trace.TraceError(message);

        public static void Indent() => System.Diagnostics.Trace.Indent();
        public static void Unindent() => System.Diagnostics.Trace.Unindent();

        public static void Trace(Exception ex) => System.Diagnostics.Trace.TraceError(ex.ToString());
    }
}