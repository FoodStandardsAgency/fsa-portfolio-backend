using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class PortfolioModel
    {
        [JsonProperty("viewkey")]
        public string ViewKey { get; set; }

        [JsonProperty("abbr")]
        public string ShortName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }

    public class NewPortfolioModel
    {
        [JsonProperty("viewkey")]
        public string ViewKey { get; set; }

        [JsonProperty("abbr")]
        public string ShortName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

    }

    public class PortfolioPermissionModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("permission")]
        public string Permission { get; set; }

    }

}