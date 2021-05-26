using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectQueryResultModel
    {
        [JsonProperty("projects")]
        public IEnumerable<ProjectQueryResultProjectModel> Projects { get; set; }

        [JsonProperty("project_cnt")]
        public int ResultCount { get; set; }
    }
    public class ProjectQueryResultProjectModel
    {
        [JsonProperty("portfolio")]
        public string PortfolioViewKey { get; set; }

        [JsonProperty(ProjectPropertyConstants.ProjectId)]
        public string ProjectId { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_name)]
        public string ProjectName { get; set; }

        [JsonProperty(ProjectPropertyConstants.priority_main)]
        public int? Priority { get; set; }

        [JsonProperty(ProjectPropertyConstants.pgroup)]
        public string PriorityGroup { get; set; }

    }
}