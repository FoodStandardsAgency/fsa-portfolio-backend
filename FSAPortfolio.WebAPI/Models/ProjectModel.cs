using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    /// <summary>
    /// This class forms the main contract between the front end and the API.
    /// The names of these fields are stored in the label configurations in the database.
    /// Take caution when renaming these fields as this is likely to break:
    /// - existing label configurations
    /// - front end views and forms
    /// </summary>
    public class ProjectModel
    {
        public string id { get; set; }

        [JsonProperty(ProjectPropertyConstants.ProjectId)]
        public string project_id { get; set; }

        public string project_name { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? expend { get; set; }
        public DateTime? hardend { get; set; }
        public DateTime? actstart { get; set; }
        public DateTime? expendp { get; set; }
        public DateTime? actual_end_date { get; set; }

        public string short_desc { get; set; }
        public string phase { get; set; }
        public string category { get; set; }
        public string[] subcat { get; set; }
        public string rag { get; set; }
        public string update { get; set; }
        public string priority_main { get; set; } // TODO: make this an int
        public int funded { get; set; }
        public int confidence { get; set; }
        public int priorities { get; set; }
        public int benefits { get; set; }
        public int criticality { get; set; }
        public string budget { get; set; }
        public string spent { get; set; }
        public LinkModel[] documents { get; set; }
        public DateTime? timestamp { get; set; }
        public string pgroup { get; set; }
        public LinkModel link { get; set; }
        public string onhold { get; set; }
        public string project_size { get; set; }
        public string oddlead_role { get; set; }
        public string budgettype { get; set; }
        public string direct { get; set; }
        public float? p_comp { get; set; }
        public DateTime? max_time { get; set; }
        public DateTime? min_time { get; set; }
        public string g6team { get; set; }
        public string new_flag { get; set; }
        public DateTime? first_completed { get; set; }

        public ProjectPersonModel key_contact1 { get; set; }
        public ProjectPersonModel key_contact2 { get; set; }
        public ProjectPersonModel key_contact3 { get; set; }


        // New fields
        public string business_case_number { get; set; }
        public string fs_number { get; set; }
        public string risk_rating { get; set; }
        public string theme { get; set; }
        public string project_type { get; set; }
        public string strategic_objectives { get; set; }
        public string programme { get; set; }
        public string programme_description { get; set; }

        public string supplier { get; set; }

        public string milestones { get; set; }

        public string how_get_green { get; set; }
        public string forward_look { get; set; }
        public string emerging_issues { get; set; }
        

        public int forecast_spend { get; set; }
        public int budget_field1 { get; set; }
        public string cost_centre { get; set; }
        public string fsaproc_assurance_gatenumber { get; set; }
        public DateTime? fsaproc_assurance_gatecompleted { get; set; }
        public string fsaproc_assurance_nextgate { get; set; }
    }

    public class LinkModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }
    }

    public class UpdateHistoryModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class StatusUpdateHistoryModel
    {
        [JsonProperty("rag")]
        public string RAGStatus { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}