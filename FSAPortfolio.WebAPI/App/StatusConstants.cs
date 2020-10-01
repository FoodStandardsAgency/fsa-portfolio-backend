using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public static class PhaseConstants
    {
        public static string BacklogName = ConfigurationManager.AppSettings["Phase.Backlog.Name"] ?? "Backlog";
        public static string DiscoveryName = ConfigurationManager.AppSettings["Phase.Discovery.Name"] ?? "Discovery";
        public static string AlphaName = ConfigurationManager.AppSettings["Phase.Alpha.Name"] ?? "Alpha";
        public static string BetaName = ConfigurationManager.AppSettings["Phase.Beta.Name"] ?? "Beta";
        public static string LiveName = ConfigurationManager.AppSettings["Phase.Live.Name"] ?? "Live";
        public static string CompletedName = ConfigurationManager.AppSettings["Phase.Completed.Name"] ?? "Completed";
    }

    public static class OnHoldConstants
    {
        public static string NoName = ConfigurationManager.AppSettings["OnHold.No.Name"] ?? "No";
        public static string OnHoldName = ConfigurationManager.AppSettings["OnHold.OnHold.Name"] ?? "On hold";
        public static string BlockedName = ConfigurationManager.AppSettings["OnHold.Blocked.Name"] ?? "Blocked";
        public static string CovidName = ConfigurationManager.AppSettings["OnHold.Covid.Name"] ?? "Covid-19 on hold";
    }

    public static class RagConstants
    {
        public static string RedName = ConfigurationManager.AppSettings["Rag.Red.Name"] ?? "Red";
        public static string AmberName = ConfigurationManager.AppSettings["Rag.Amber.Name"] ?? "Amber";
        public static string GreenName = ConfigurationManager.AppSettings["Rag.Green.Name"] ?? "Green";
        public static string NoneName = ConfigurationManager.AppSettings["Rag.None.Name"] ?? "Undecided";
    }
}