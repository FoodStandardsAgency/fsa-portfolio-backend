using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class Project
    {
        public int ProjectReservation_Id { get; set; }

        public ProjectReservation Reservation { get; set; }


        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public virtual Directorate Directorate { get; set; }

        [StringLength(50)]
        public string Theme { get; set; }

        [StringLength(50)]
        public string ProjectType { get; set; }

        [StringLength(50)]
        public string StrategicObjectives { get; set; }

        [StringLength(150)]
        public string Programme { get; set; }

        public virtual ProjectCategory Category { get; set; }
        public int? ProjectCategory_Id { get; set; }

        public virtual ICollection<ProjectCategory> Subcategories { get; set; }

        public virtual ProjectSize Size { get; set; }
        public int? ProjectSize_Id { get; set; }
        public virtual BudgetType BudgetType { get; set; }
        public int? BudgetType_Id { get; set; }
        public int Funded { get; set; }
        public int Confidence { get; set; }
        public int Priorities { get; set; }
        public int Benefits { get; set; }
        public int Criticality { get; set; }

        public virtual ICollection<Portfolio> Portfolios { get; set; }
        public virtual ICollection<Project> RelatedProjects { get; set; }
        public virtual ICollection<Project> DependantProjects { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<ProjectDataItem> ProjectData { get; set; }

        public ProjectLink ChannelLink { get; set; }

        public virtual Person Lead { get; set; }
        public int? Lead_Id { get; set; }


        public virtual Person KeyContact1 { get; set; }
        public virtual Person KeyContact2 { get; set; }
        public virtual Person KeyContact3 { get; set; }

        public virtual Person ServiceLead { get; set; }
        public int? ServiceLead_Id { get; set; }

        public virtual ICollection<Person> People { get; set; }

        [StringLength(150)]
        public string Supplier { get; set; }

        public int? Priority { get; set; }
        public ProjectDate StartDate { get; set; }
        public ProjectDate ActualStartDate { get; set; }
        public ProjectDate ExpectedEndDate { get; set; }
        public ProjectDate HardEndDate { get; set; }
        public ProjectDate ActualEndDate { get; set; }

        [StringLength(50)]
        public string BusinessCaseNumber { get; set; }
        [StringLength(50)]
        public string FSNumber { get; set; }
        [StringLength(50)]
        public string RiskRating { get; set; }

        public string ProgrammeDescription { get; set; }

        public virtual ICollection<ProjectUpdateItem> Updates { get; set; }
        public virtual ProjectUpdateItem LatestUpdate { get; set; }
        public int? LatestUpdate_Id { get; set; }
        public virtual ProjectUpdateItem FirstUpdate { get; set; }
        public int? FirstUpdate_Id { get; set; }

        public virtual ICollection<ProjectAuditLog> AuditLogs { get; set; }

        public bool IsNew => FirstUpdate == null ? true : FirstUpdate.Timestamp >= DateTime.Today.AddDays(-PortfolioSettings.NewProjectLimitDays);
        public PriorityGroup PriorityGroup {
            get {
                PriorityGroup priorityGroup = null;
                if (Reservation?.Portfolio?.Configuration != null)
                {
                    if (Priority.HasValue)
                    {
                        priorityGroup = Reservation.Portfolio.Configuration.PriorityGroups.SingleOrDefault(pg => Priority.Value >= pg.LowLimit && Priority.Value <= pg.HighLimit);
                    }
                    else
                    {
                        priorityGroup = Reservation.Portfolio.Configuration.PriorityGroups.SingleOrDefault(pg => pg.ViewKey == PriorityGroupConstants.NotSetViewKey);
                    }
                }
                return priorityGroup;
            }
        }
    }
}
