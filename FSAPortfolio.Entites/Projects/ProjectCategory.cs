using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class ProjectCategory
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string ViewKey { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public int Order { get; set; }
    }
}
