using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Entities.Projects
{
    public static class ProjectOptionConstants
    {
        public const int HideOrderValue = -1;
    }

    public static class PhaseConstants
    {
        public const int MaxCount = 6;
        public static string BacklogName = ConfigurationManager.AppSettings["Phase.Backlog.Name"] ?? "Backlog";
        public static string DiscoveryName = ConfigurationManager.AppSettings["Phase.Discovery.Name"] ?? "Discovery";
        public static string AlphaName = ConfigurationManager.AppSettings["Phase.Alpha.Name"] ?? "Alpha";
        public static string BetaName = ConfigurationManager.AppSettings["Phase.Beta.Name"] ?? "Beta";
        public static string LiveName = ConfigurationManager.AppSettings["Phase.Live.Name"] ?? "Live";
        public static string CompletedName = ConfigurationManager.AppSettings["Phase.Completed.Name"] ?? "Completed";
    }

    public static class OnHoldConstants
    {
        public const int MaxCount = 30;
        public static string NoName = ConfigurationManager.AppSettings["OnHold.No.Name"] ?? "No";
        public static string OnHoldName = ConfigurationManager.AppSettings["OnHold.OnHold.Name"] ?? "On hold";
        public static string BlockedName = ConfigurationManager.AppSettings["OnHold.Blocked.Name"] ?? "Blocked";
        public static string CovidName = ConfigurationManager.AppSettings["OnHold.Covid.Name"] ?? "Covid-19 on hold";
    }

    public static class RagConstants
    {
        public static string RedViewKey = ConfigurationManager.AppSettings["Rag.Red.ViewKey"] ?? "red";
        public static string RedAmberViewKey = ConfigurationManager.AppSettings["Rag.RedAmber.ViewKey"] ?? "redamb";
        public static string AmberViewKey = ConfigurationManager.AppSettings["Rag.Amber.ViewKey"] ?? "amb";
        public static string AmberGreenViewKey = ConfigurationManager.AppSettings["Rag.AmberGreen.ViewKey"] ?? "ambgre";
        public static string GreenViewKey = ConfigurationManager.AppSettings["Rag.Green.ViewKey"] ?? "gre";
        public static string NoneViewKey = ConfigurationManager.AppSettings["Rag.None.ViewKey"] ?? "nor";


        public static string RedName = ConfigurationManager.AppSettings["Rag.Red.Name"] ?? "Red";
        public static string RedAmberName = ConfigurationManager.AppSettings["Rag.RedAmber.Name"] ?? "Red/Amber";
        public static string AmberName = ConfigurationManager.AppSettings["Rag.Amber.Name"] ?? "Amber";
        public static string AmberGreenName = ConfigurationManager.AppSettings["Rag.AmberGreen.Name"] ?? "Amber/Green";
        public static string GreenName = ConfigurationManager.AppSettings["Rag.Green.Name"] ?? "Green";
        public static string NoneName = ConfigurationManager.AppSettings["Rag.None.Name"] ?? "Undecided";
    }
    public static class CategoryConstants
    {
        public const int MaxCount = 30;
        public static string CapabilityName = ConfigurationManager.AppSettings["Category.Capability.Name"] ?? "Developing our digital capability";
        public static string DataName = ConfigurationManager.AppSettings["Category.Data.Name"] ?? "Data driven FSA";
        public static string ServiceMgmtName = ConfigurationManager.AppSettings["Category.Service.Name"] ?? "IT Service management";
        public static string SupportName = ConfigurationManager.AppSettings["Category.Support.Name"] ?? "Digital services development and support";
        public static string ITName = ConfigurationManager.AppSettings["Category.IT.Name"] ?? "Evergreen IT";
        public static string ResilienceName = ConfigurationManager.AppSettings["Category.Resilience.Name"] ?? "Protecting data and business resilience";
        public static string NotSetName = ConfigurationManager.AppSettings["Category.NotSet.Name"] ?? "None";
    }

    public static class ProjectSizeConstants
    {
        public const int MaxCount = 30;
        public static string SmallName = ConfigurationManager.AppSettings["Size.Small.Name"] ?? "Small";
        public static string MediumName = ConfigurationManager.AppSettings["Size.Medium.Name"] ?? "Medium";
        public static string LargeName = ConfigurationManager.AppSettings["Size.Large.Name"] ?? "Large";
        public static string ExtraLargeName = ConfigurationManager.AppSettings["Size.ExtraLarge.Name"] ?? "Extra Large";
        public static string NotSetName = ConfigurationManager.AppSettings["Size.NotSet.Name"] ?? "None";
    }

    public static class BudgetTypeConstants
    {
        public const int MaxCount = 30;
        public static string AdminName = ConfigurationManager.AppSettings["BudgetType.Admin.Name"] ?? "Admin";
        public static string ProgrammeName = ConfigurationManager.AppSettings["BudgetType.Programme.Name"] ?? "Programme";
        public static string CapitalName = ConfigurationManager.AppSettings["BudgetType.Capital.Name"] ?? "Capital";
        public static string NotSetName = ConfigurationManager.AppSettings["BudgetType.NotSet.Name"] ?? "None";
        public static string NotSetViewKey = ConfigurationManager.AppSettings["BudgetType.NotSet.ViewKey"] ?? "none";
    }

    public static class PriorityGroupConstants
    {
        public static int MaxPriority = int.Parse(ConfigurationManager.AppSettings["PriorityGroup.High.Max"] ?? "20");
        public static int HighGroupCutoff = int.Parse(ConfigurationManager.AppSettings["PriorityGroup.High.Cutoff"] ?? "15");
        public static int MediumGroupCutoff = int.Parse(ConfigurationManager.AppSettings["PriorityGroup.High.Cutoff"] ?? "8");
        public static string HighName = ConfigurationManager.AppSettings["PriorityGroup.High.Name"] ?? "High";
        public static string MediumName = ConfigurationManager.AppSettings["PriorityGroup.Medium.Name"] ?? "Medium";
        public static string LowName = ConfigurationManager.AppSettings["PriorityGroup.Low.Name"] ?? "Low";
        public static string NotSetName = ConfigurationManager.AppSettings["PriorityGroup.NotSet.Name"] ?? "None";

        public static string LowViewKey = ConfigurationManager.AppSettings["PriorityGroup.Low.ViewKey"] ?? "low";
        public static string MediumViewKey = ConfigurationManager.AppSettings["PriorityGroup.Medium.ViewKey"] ?? "medium";
        public static string HighViewKey = ConfigurationManager.AppSettings["PriorityGroup.High.ViewKey"] ?? "high";
        public static string NotSetViewKey = ConfigurationManager.AppSettings["PriorityGroup.NotSet.ViewKey"] ?? "none";

    }

}