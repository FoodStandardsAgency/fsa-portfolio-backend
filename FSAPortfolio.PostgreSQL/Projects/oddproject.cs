namespace FSAPortfolio.PostgreSQL.Projects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class oddproject : IPostgresProject
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
        public string oddlead { get; set; }

        [StringLength(150)]
        public string oddlead_email { get; set; }

        [StringLength(150)]
        public string servicelead { get; set; }

        [StringLength(150)]
        public string servicelead_email { get; set; }

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

        [Key]
        [Column(Order = 1)]
        public DateTime timestamp { get; set; }

        [StringLength(20)]
        public string pgroup { get; set; }

        [StringLength(1000)]
        public string link { get; set; }

        [StringLength(1)]
        public string toupdate { get; set; }

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
        public string project_size { get; set; }

        [StringLength(5)]
        public string oddlead_role { get; set; }

        [StringLength(15)]
        public string budgettype { get; set; }

        [StringLength(150)]
        public string direct { get; set; }

        [StringLength(12)]
        public string expendp { get; set; }

        public float? p_comp { get; set; }

        public bool IsDuplicate(object project)
        {
            var p = (oddproject)project;
            return update != null &&
                rag == p.rag &&
                onhold == p.onhold &&
                phase == p.phase &&
                p_comp == p.p_comp &&
                budget == p.budget &&
                spent == p.spent &&
                expendp == p.expendp &&
                (string.IsNullOrWhiteSpace(update) || update == p.update)
                ;
        }

    }
}
