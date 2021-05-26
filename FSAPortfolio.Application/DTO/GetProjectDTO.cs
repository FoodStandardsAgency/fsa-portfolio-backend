using FSAPortfolio.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.DTO
{
    public class GetProjectDTO<T> where T : ProjectModel, new()
    {
        [JsonProperty("config")]
        public ProjectLabelConfigModel Config { get; set; }

        [JsonProperty("project")]
        public T Project { get; set; }

        [JsonProperty("options")]
        public ProjectEditOptionsModel Options { get; set; }

    }
}