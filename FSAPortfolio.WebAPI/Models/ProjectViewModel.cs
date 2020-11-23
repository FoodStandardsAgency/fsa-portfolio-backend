using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectViewModel : ProjectModel
    {
        // TODO: move lead and servicelead over to ProjectPersonModel; then move all instances back into ProjectModel
        public string oddlead_email { get; set; } 
        public string servicelead_email { get; set; }

        [JsonProperty(ProjectPropertyConstants.ProjectLead)]
        public string oddlead { get; set; }

        public string servicelead { get; set; }

        public string team { get; set; }


        [JsonProperty("phaseviewkey")]
        public string phaseviewkey { get; set; }


        [JsonProperty("updates")]
        public UpdateHistoryModel[] UpdateHistory { get; set; }

        [JsonProperty("laststatusupdate")]
        public StatusUpdateHistoryModel LastStatusUpdate { get; set; }

        public RelatedProjectModel[] rels { get; set; }
        public RelatedProjectModel[] dependencies { get; set; }

        public ProjectDateViewModel start_date { get; set; }
        public ProjectDateViewModel expend { get; set; }
        public ProjectDateViewModel hardend { get; set; }
        public ProjectDateViewModel actstart { get; set; }
        public ProjectDateViewModel expendp { get; set; }
        public ProjectDateViewModel actual_end_date { get; set; }


    }

    public class RelatedProjectModel
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("project_name")]
        public string Name { get; set; }
    }

    public class ProjectDateViewModel
    {
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("flag")]
        public string Flag { get; set; }

    }

}