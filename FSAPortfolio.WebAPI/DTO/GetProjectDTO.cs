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
        public PortfolioConfigModel Config { get; set; }

        [JsonProperty("project")]
        public ProjectModel Project { get; set; }
    }
}