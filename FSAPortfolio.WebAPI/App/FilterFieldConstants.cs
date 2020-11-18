using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    internal static class FilterFieldConstants
    {
        internal const string TeamMemberNameName = "Project team";
        internal const string TeamMemberNameFilter = "projectname_filter";

        internal const string NoUpdatesName = "Show projects with no updates";
        internal const string NoUpdatesFilter = "noupdates_filter";

        internal const string LastUpdateName = "Last update before";
        internal const string LastUpdateFilter = "lastupdate_filter";

        internal const string PastIntendedStartDateName = "Past intended start date";
        internal const string PastIntendedStartDateFilter = "paststart_filter";

        internal const string MissedEndDateName = "Missed end date or deadline";
        internal const string MissedEndDateFilter = "missedend_filter";

        internal const string PriorityGroupName = "Priority group";
        internal const string PriorityGroupFilter = "pgroup_filter";

        internal const string LeadTeamName = "Lead team";
        internal const string LeadTeamFilter = "leadteam_filter";

    }
}