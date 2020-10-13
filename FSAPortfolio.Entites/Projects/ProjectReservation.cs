using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
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

        public int Year { get; set; }
        public int Month { get; set; }
        public int Index { get; set; }

        public DateTime ReservedAt { get; set; }
        public virtual Project Project { get; set; }

        public string ProjectId => $"{Portfolio.IDPrefix}{Year % 100:00}{Month:00}{Index:000}";

    }
}
