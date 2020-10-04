using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class PortfolioConfigModel
    {
        [JsonProperty("labels")]
        public IEnumerable<PortfolioLabelModel> Labels { get; set; }
    }

    public class PortfolioLabelModel
    {
        [JsonProperty("field")]
        public string FieldName { get; set; }
        [JsonProperty("included")]
        public bool Included { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }

    }
}