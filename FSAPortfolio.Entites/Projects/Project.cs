using FSAPortfolio.Entites.Users;
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

        public virtual Person Lead { get; set; }
        public int? Lead_Id { get; set; }

        public int Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public DateTime? HardEndDate { get; set; }

        public virtual ICollection<ProjectUpdateItem> Updates { get; set; }
        public virtual ProjectUpdateItem LatestUpdate { get; set; }
        public int? LatestUpdate_Id { get; set; }
        public virtual ProjectUpdateItem FirstUpdate { get; set; }
        public int? FirstUpdate_Id { get; set; }
    }
}
