using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class PortfolioConfigUpdateRequest
    {
        [JsonProperty("portfolio")]
        public string ViewKey { get; set; }

        [JsonProperty("labels")]
        public IEnumerable<PortfolioLabelModel> Labels { get; set; }

    }

}