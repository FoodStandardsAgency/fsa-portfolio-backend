using FSAPortfolio.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.DTO
{
    public class GetNewProjectDTO
    {
        [JsonProperty("config")]
        public ProjectLabelConfigModel Config { get; set; }

        [JsonProperty("project")]
        public ProjectModel Project { get; set; }

    }
}