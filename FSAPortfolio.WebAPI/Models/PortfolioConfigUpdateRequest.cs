using FSAPortfolio.Entities;
using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

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