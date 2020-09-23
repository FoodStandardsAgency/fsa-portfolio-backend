namespace FSAPortfolio.Entites.Users
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime timestamp { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(300)]
        public string pass_hash { get; set; }

        [StringLength(3)]
        public string access_group { get; set; }
    }
}
