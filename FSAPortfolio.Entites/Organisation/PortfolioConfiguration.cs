using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Entities.Organisation
{
    public class PortfolioConfiguration
    {
        public int Portfolio_Id { get; set; }

        public virtual Portfolio Portfolio { get; set; }
        public virtual ICollection<ProjectPhase> Phases { get; set; }
        public virtual ProjectPhase CompletedPhase { get; set; }
        public virtual ICollection<ProjectRAGStatus> RAGStatuses { get; set; }
        public virtual ICollection<ProjectOnHoldStatus> OnHoldStatuses { get; set; }

        public virtual ICollection<ProjectCategory> Categories { get; set; }
        public virtual ICollection<ProjectSize> ProjectSizes { get; set; }
        public virtual ICollection<BudgetType> BudgetTypes { get; set; }

        public virtual ICollection<PortfolioLabelConfig> Labels { get; set; }
        public virtual ICollection<PortfolioLabelGroup> LabelGroups { get; set; }

        public virtual ICollection<PortfolioConfigAuditLog> AuditLogs { get; set; }

        public IEnumerable<PriorityGroup> PriorityGroups => lazyPriorityGroups.Value;

        private Lazy<IEnumerable<PriorityGroup>> lazyPriorityGroups;

        public PortfolioConfiguration()
        {
            lazyPriorityGroups = new Lazy<IEnumerable<PriorityGroup>>(() => {
                return new PriorityGroup[] {
                    new PriorityGroup() {
                        ViewKey = PriorityGroupConstants.HighViewKey,
                        LowLimit = PriorityGroupConstants.HighGroupCutoff, 
                        HighLimit = PriorityGroupConstants.MaxPriority, 
                        Name = PriorityGroupConstants.HighName, 
                        Configuration = this, 
                        Order = 0 
                    },
                    new PriorityGroup() {
                        ViewKey = PriorityGroupConstants.MediumViewKey,
                        LowLimit = PriorityGroupConstants.MediumGroupCutoff, 
                        HighLimit = PriorityGroupConstants.HighGroupCutoff - 1, 
                        Name = PriorityGroupConstants.MediumName, 
                        Configuration = this, 
                        Order = 1 
                    },
                    new PriorityGroup() {
                        ViewKey = PriorityGroupConstants.LowViewKey,
                        LowLimit = 0, 
                        HighLimit = PriorityGroupConstants.MediumGroupCutoff - 1, 
                        Name = PriorityGroupConstants.LowName, 
                        Configuration = this, 
                        Order = 2 
                    },
                    new PriorityGroup() {
                        ViewKey = PriorityGroupConstants.NotSetViewKey,
                        LowLimit = -1,
                        HighLimit = -1,
                        Name = PriorityGroupConstants.NotSetName,
                        Configuration = this,
                        Order = 3
                    }                
                };
            });
        }

    }

    public class PriorityGroup
    {
        public string ViewKey { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }
        public PortfolioConfiguration Configuration { get; set; }
    }
}