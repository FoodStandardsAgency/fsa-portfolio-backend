using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class ProjectUpdateExportModel
    {
        public DateTime? timestamp { get; set; }

        [JsonProperty(ProjectPropertyConstants.ProjectId)]
        public string project_id { get; set; }

        public string phase { get; set; }
        public string rag { get; set; }
        public string onhold { get; set; }
        public string update { get; set; }
        public string budget { get; set; }
        public string spent { get; set; }
        public string expendp { get; set; }
        public float? p_comp { get; set; }

    }
}