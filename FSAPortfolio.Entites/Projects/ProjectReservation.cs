using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class ProjectReservation
    {
        public int Id { get; set; }
        public int Portfolio_Id { get; set; }
        public virtual Portfolio Portfolio { get; set; }

        private string projectId;
        [StringLength(20)]
        public string ProjectId 
        { 
            get
            {
                if(projectId == null)
                {
                    projectId = $"{Portfolio.IDPrefix}{Year % 100:00}{Month:00}{Index:000}";
                }
                return projectId;
            }
            set
            {
                projectId = value;
            }
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public int Index { get; set; }

        public DateTime ReservedAt { get; set; }
        public virtual Project Project { get; set; }
    }
}
