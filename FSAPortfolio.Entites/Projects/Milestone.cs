using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class Milestone
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int Project_ProjectReservation_Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public ProjectDate Deadline { get; set; }

        public int Order { get; set; }
    }
}
