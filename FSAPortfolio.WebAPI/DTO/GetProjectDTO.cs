using FSAPortfolio.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.DTO
{
    public class GetProjectDTO
    {
        [JsonProperty("config")]
        public ProjectLabelConfigModel Config { get; set; }

        [JsonProperty("project")]
        public ProjectViewModel Project { get; set; }

        [JsonProperty("options")]
        public ProjectOptionsModel Options { get; set; }

    }
}