using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectQueryModel
    {
        [JsonProperty("portfolio")]
        public string PortfolioViewKey { get; set; }

        [JsonProperty(nameof(ProjectModel.project_name))]
        public string Name { get; set; }

        [JsonProperty(FilterFieldConstants.TeamMemberNameFilter)]
        public string TeamMemberName { get; set; }

        [JsonProperty(nameof(ProjectModel.phase))]
        public string[] Phases { get; set; }

        [JsonProperty(nameof(ProjectModel.theme))]
        public string[] Themes { get; set; }

        [JsonProperty(nameof(ProjectModel.project_type))]
        public string[] ProjectTypes { get; set; }

        [JsonProperty(nameof(ProjectModel.rag))]
        public string[] RAGStatuses { get; set; }

        [JsonProperty(nameof(ProjectModel.onhold))]
        public string[] OnHoldStatuses { get; set; }

        [JsonProperty(nameof(ProjectModel.category))]
        public string[] Categories { get; set; }

        [JsonProperty(nameof(ProjectModel.direct))]
        public string[] Directorates { get; set; }

        [JsonProperty(nameof(ProjectModel.strategic_objectives))]
        public string[] StrategicObjectives { get; set; }

        [JsonProperty(nameof(ProjectModel.programme))]
        public string[] Programmes { get; set; }

        [JsonProperty(ProjectPropertyConstants.ProjectLead)]
        public string ProjectLeadName { get; set; }

        [JsonProperty(nameof(ProjectModel.priority_main))]
        public int[] Priorities { get; set; }



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