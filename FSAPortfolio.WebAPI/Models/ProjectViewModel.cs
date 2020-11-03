using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectViewModel : ProjectModel
    {
        public string oddlead_email { get; set; } // TODO: view only! Move to view model
        public string servicelead_email { get; set; } // TODO: view only! Move to view model
        public string oddlead { get; set; }
        public string servicelead { get; set; }



        [JsonProperty("updates")]
        public UpdateHistoryModel[] UpdateHistory { get; set; }

        [JsonProperty("lastupdate")]
        public UpdateHistoryModel LastUpdate { get; set; }

        [JsonProperty("laststatusupdate")]
        public StatusUpdateHistoryModel LastStatusUpdate { get; set; }

        public RelatedProjectModel[] rels { get; set; }
        public RelatedProjectModel[] dependencies { get; set; }



    }

    public class RelatedProjectModel
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("project_name")]
        public string Name { get; set; }
    }

}