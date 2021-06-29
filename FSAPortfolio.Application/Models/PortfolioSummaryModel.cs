using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class PortfolioSummaryModel
    {
        public const string ByCategory = "category";
        public const string ByPriorityGroup = "priority";
        public const string ByRagStatus = "rag";
        public const string ByPhase = "phase";
        public const string ByLead = "lead";
        public const string ByTeam = "team";
        public const string NewProjectsByTeam = "newbyteam";

        [JsonProperty("person")]
        public string Person { get; set; }

        [JsonProperty("summaries")]
        public IEnumerable<ProjectSummaryModel> Summaries { get; set; }

        [JsonProperty("phases")]
        public IEnumerable<PhaseSummaryModel> Phases { get; set; }

        [JsonProperty("labels")]
        public IEnumerable<PortfolioLabelModel> Labels { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_type)]
        public List<DropDownItemModel> ProjectTypes { get; set; }


    }

    public class ProjectSummaryModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("viewkey")]
        public string ViewKey { get; set; }

        [JsonProperty("phases")]
        public IEnumerable<PhaseProjectsModel> PhaseProjects { get; set; }

    }

    public class PhaseSummaryModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("viewkey")]
        public string ViewKey { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class PhaseProjectsModel
    {
        [JsonProperty("viewkey")]
        public string ViewKey { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("projects")]
        public IEnumerable<ProjectIndexModel> Projects { get; set; }
    }

    public class ProjectIndexModel
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("new_flag")]
        public string IsNew { get; set; }

        [JsonProperty("date")]
        public ProjectDateIndexModel Deadline { get; set; }

        [JsonProperty("priority")]
        public ProjectPriorityIndexModel Priority { get; set; }
    }

    public class ProjectDateIndexModel
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public ProjectDateViewModel Value { get; set; }

    }
    public class ProjectPriorityIndexModel
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

    }
}