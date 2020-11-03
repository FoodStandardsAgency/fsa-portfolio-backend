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

        [JsonProperty(nameof(ProjectUpdateModel.oddlead))]
        public string[] Leads { get; set; }
    }

}