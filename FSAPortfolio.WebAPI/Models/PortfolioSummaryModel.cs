using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class PortfolioSummaryModel
    {
        [JsonProperty("categories")]
        public IEnumerable<CategorySummaryModel> Categories { get; set; }

        [JsonProperty("phases")]
        public IEnumerable<PhaseSummaryModel> Phases { get; set; }
    }

    public class CategorySummaryModel
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
    }
}