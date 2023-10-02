namespace FSAPortfolio.PostgreSQL.Projects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class serdproject : IPostgresProject
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [StringLength(10)]
        public string project_id { get; set; }

        [StringLength(250)]
        public string project_name { get; set; }

        [StringLength(50)]
        public string start_date { get; set; }

        [StringLength(1000)]
        public string short_desc { get; set; }

        [StringLength(50)]
        public string phase { get; set; }

        [StringLength(50)]
        public string category { get; set; }

        [StringLength(150)]
        public string subcat { get; set; }

        [StringLength(50)]
        public string rag { get; set; }

        public string update { get; set; }

        [StringLength(150)]
        public string lead { get; set; }

        [StringLength(150)]
        public string lead_email { get; set; }


        [StringLength(3)]
        public string priority_main { get; set; }

        [StringLength(3)]
        public string funded { get; set; }

        [StringLength(3)]
        public string confidence { get; set; }

        [StringLength(3)]
        public string priorities { get; set; }

        [StringLength(3)]
        public string benefits { get; set; }

        [StringLength(3)]
        public string criticality { get; set; }

        [StringLength(15)]
        public string budget { get; set; }

        [StringLength(15)]
        public string spent { get; set; }

        [StringLength(2000)]
        public string documents { get; set; }

        [StringLength(2000)]
        public string forecasts { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime timestamp { get; set; }

        [StringLength(20)]
        public string pgroup { get; set; }

        [StringLength(1000)]
        public string link { get; set; }

        [StringLength(500)]
        public string rels { get; set; }

        [StringLength(500)]
        public string team { get; set; }

        [StringLength(10)]
        public string onhold { get; set; }

        [StringLength(50)]
        public string expend { get; set; }

        [StringLength(50)]
        public string hardend { get; set; }

        [StringLength(50)]
        public string actstart { get; set; }

        [StringLength(500)]
        public string dependencies { get; set; }

        [StringLength(5)]
        public string project_type { get; set; } // TODO: NEW

        [StringLength(5)]
        public string lead_team { get; set; } // TODO: NEW

        [StringLength(15)]
        public string budgettype { get; set; }

        [StringLength(150)]
        public string fsno { get; set; }
        [StringLength(150)]
        public string ibbcno { get; set; }
        [StringLength(150)]
        public string supplier { get; set; }
        [StringLength(150)]
        public string tot_budget { get; set; }

        [StringLength(3)]
        public string rd { get; set; }

        public bool IsDuplicate(object project)
        {
            var p = (serdproject)project;
            return update != null &&
                rag == p.rag &&
                onhold == p.onhold &&
                phase == p.phase &&
                budget == p.budget &&
                spent == p.spent &&
                (string.IsNullOrWhiteSpace(p.update) || update == p.update)
                ;
        }

    }
}
