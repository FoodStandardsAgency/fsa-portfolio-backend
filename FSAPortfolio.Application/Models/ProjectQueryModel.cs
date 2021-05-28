using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class ProjectQueryModel
    {
        [JsonProperty("portfolio")]
        public string PortfolioViewKey { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_name)]
        public string Name { get; set; }

        [JsonProperty(FilterFieldConstants.TeamMemberNameFilter)]
        public string TeamMemberName { get; set; }

        [JsonProperty(ProjectPropertyConstants.phase)]
        public string[] Phases { get; set; }

        [JsonProperty(ProjectPropertyConstants.theme)]
        public string[] Themes { get; set; }

        [JsonProperty(PropertyName = ProjectPropertyConstants.project_type)]
        public string[] ProjectTypes { get; set; }

        [JsonProperty(ProjectPropertyConstants.rag)]
        public string[] RAGStatuses { get; set; }

        [JsonProperty(ProjectPropertyConstants.onhold)]
        public string[] OnHoldStatuses { get; set; }

        [JsonProperty(ProjectPropertyConstants.category)]
        public string[] Categories { get; set; }

        [JsonProperty(ProjectPropertyConstants.direct)]
        public string[] Directorates { get; set; }

        [JsonProperty(ProjectPropertyConstants.strategic_objectives)]
        public string[] StrategicObjectives { get; set; }

        [JsonProperty(ProjectPropertyConstants.programme)]
        public string[] Programmes { get; set; }

        [JsonProperty(ProjectPropertyConstants.ProjectLead)]
        public string ProjectLeadName { get; set; }

        [JsonProperty(ProjectPropertyConstants.priority_main)]
        public int[] Priorities { get; set; }

        [JsonProperty(FilterFieldConstants.LeadTeamFilter)]
        public string[] Teams { get; set; }

        [JsonProperty(FilterFieldConstants.PriorityGroupFilter)]
        public string[] PriorityGroups { get; set; }



        [JsonProperty(FilterFieldConstants.LastUpdateFilter)]
        public DateTime? LastUpdateBefore { get; set; }

        [JsonProperty(FilterFieldConstants.NoUpdatesFilter)]
        public bool? NoUpdates { get; set; }

        [JsonProperty(FilterFieldConstants.PastIntendedStartDateFilter)]
        public bool? PastStartDate { get; set; }

        [JsonProperty(FilterFieldConstants.MissedEndDateFilter)]
        public bool? MissedEndDate { get; set; }

    }

}