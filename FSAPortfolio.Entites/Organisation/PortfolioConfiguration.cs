using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Entities
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
        public PortfolioLabelConfig ProjectIdLabel { get; set; }
        public PortfolioLabelConfig BCNumberLabel { get; set; }
    }

    public class PortfolioLabelConfig
    {
        public int Id { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }
        public bool Included { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; }

        [StringLength(50)]
        public string Label { get; set; }
        public int Configuration_Id { get; set; }
    }
}