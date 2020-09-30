using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class SyncRequestModel
    {
        [JsonProperty("projectId")]
        public string ProjectId { get; set; }

    }
}