using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectModel
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }
        [JsonProperty("project_name")]
        public string Name { get; set; }
    }

}