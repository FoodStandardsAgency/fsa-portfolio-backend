using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Users
{
    public class AccessGroup
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string ViewKey { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
