using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class PortfolioConfigLabelRequest
    {
        [JsonProperty("portfolio")]
        public string portfolio { get; set; }

        [JsonProperty("included")]
        public bool included { get; set; }

        [JsonProperty("field")]
        [StringLength(50)]
        public string field { get; set; }

        [JsonProperty("label")]
        [StringLength(50)]
        public string label { get; set; }

    }
}