using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectModel
    {
        public string id { get; set; }
        public string project_id { get; set; }
        public string project_name { get; set; }
        public string start_date { get; set; }
        public string short_desc { get; set; }
        public string phase { get; set; }
        public string category { get; set; }
        public string[] subcat { get; set; }
        public string rag { get; set; }
        public string update { get; set; }
        public string oddlead { get; set; }
        public string oddlead_email { get; set; }
        public string servicelead { get; set; }
        public string servicelead_email { get; set; }
        public string priority_main { get; set; }
        public string funded { get; set; }
        public string confidence { get; set; }
        public string priorities { get; set; }
        public string benefits { get; set; }
        public string criticality { get; set; }
        public string budget { get; set; }
        public string spent { get; set; }
        public string documents { get; set; }
        public DateTime? timestamp { get; set; }
        public string pgroup { get; set; }
        public string link { get; set; }
        public string rels { get; set; }
        public string team { get; set; }
        public string onhold { get; set; }
        public string expend { get; set; }
        public string hardend { get; set; }
        public string actstart { get; set; }
        public string dependencies { get; set; }
        public string project_size { get; set; }
        public string oddlead_role { get; set; }
        public string budgettype { get; set; }
        public string direct { get; set; }
        public string expendp { get; set; }
        public float? p_comp { get; set; }
        public DateTime? max_time { get; set; }
        public DateTime? min_time { get; set; }
        public string g6team { get; set; }
        public string new_flag { get; set; }
        public DateTime? first_completed { get; set; }

        // New fields
        public string business_case_number { get; set; }
        public string fs_number { get; set; }
        public string risk_rating { get; set; }
        public string theme { get; set; }
        public string project_type { get; set; }
        public string strategic_objectives { get; set; }
        public string programme { get; set; }
        public string programme_description { get; set; }


        public string key_contact1 { get; set; }
        public string key_contact2 { get; set; }
        public string key_contact3 { get; set; }
        public string supplier { get; set; }


        public string actual_end_date { get; set; }
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
}