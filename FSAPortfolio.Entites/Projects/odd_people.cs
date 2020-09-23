namespace FSAPortfolio.Entites
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class odd_people
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime timestamp { get; set; }

        [StringLength(250)]
        public string surname { get; set; }

        [StringLength(250)]
        public string firstname { get; set; }

        [StringLength(250)]
        public string email { get; set; }

        [StringLength(50)]
        public string g6team { get; set; }
    }
}
