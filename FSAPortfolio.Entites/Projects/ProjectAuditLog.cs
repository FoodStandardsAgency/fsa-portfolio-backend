using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class ProjectAuditLog
    {
        public int Id { get; set; }

        public virtual Project Project { get; set; }

        public int Project_Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }

    }
}
