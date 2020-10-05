using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Entities.Organisation
{
    public class PortfolioConfiguration
    {
        public int Id { get; set; }

        public virtual Portfolio Portfolio { get; set; }
        public virtual ICollection<ProjectPhase> Phases { get; set; }
        public virtual ProjectPhase CompletedPhase { get; set; }
        public virtual ICollection<ProjectRAGStatus> RAGStatuses { get; set; }
        public virtual ICollection<ProjectOnHoldStatus> OnHoldStatuses { get; set; }

        public virtual ICollection<ProjectCategory> Categories { get; set; }
        public virtual ICollection<ProjectSize> ProjectSizes { get; set; }
        public virtual ICollection<BudgetType> BudgetTypes { get; set; }

        public virtual ICollection<PortfolioLabelConfig> Labels { get; set; }

        public virtual ICollection<PortfolioConfigAuditLog> AuditLogs { get; set; }

    }
}