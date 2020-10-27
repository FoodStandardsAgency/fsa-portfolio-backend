using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectViewModel : ProjectModel
    {
        [JsonProperty("updates")]
        public UpdateHistoryModel[] UpdateHistory { get; set; }

        [JsonProperty("lastupdate")]
        public UpdateHistoryModel LastUpdate { get; set; }

        [JsonProperty("laststatusupdate")]
        public StatusUpdateHistoryModel LastStatusUpdate { get; set; }


    }
}