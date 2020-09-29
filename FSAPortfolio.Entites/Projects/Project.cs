using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entites.Projects
{
    public class Project
    {
        public int Id { get; set; }

        [StringLength(10)]
        public string ProjectId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }


        public DateTime StartDate { get; set; }

        public virtual ICollection<ProjectUpdateItem> Updates { get; set; }
        public virtual ProjectUpdateItem LatestUpdate { get; set; }
        public int? LatestUpdate_Id { get; set; }
    }
}
