using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Organisation
{
    public class PortfolioConfigAuditLog
    {
        public int Id { get; set; }

        public virtual PortfolioConfiguration PortfolioConfiguration { get; set; }

        public int PortfolioConfiguration_Id { get; set; }

        [StringLength(50)]
        public string AuditType { get; set; }
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }

    }
}
