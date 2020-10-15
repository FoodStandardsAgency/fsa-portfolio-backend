using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Controllers;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects
{
    public class ProjectMappingProfile : Profile
    {
        public const string PortfolioContextKey = "portfolioContext";
        public const string PortfolioConfigKey = "portfolioConfig";
        internal const string TimeOutputFormat = "dd/MM/yyyy hh:mm";
        private const string DateOutputFormat = "dd/MM/yyyy";

        public ProjectMappingProfile()
        {
            CreateMap<DateTime, string>().ConvertUsing(d => d.ToString(DateOutputFormat));
            CreateMap<DateTime?, string>().ConvertUsing(d => d.HasValue ? d.Value.ToString(DateOutputFormat) : "00/00/00");

            // Outbound
            Project__ProjectModel();
            ProjectUpdateItem__ProjectUpdateModel();

            // Inbound
            ProjectModel__Project();
            ProjectModel__ProjectUpdateItem();
        }


        private void Project__ProjectModel()
        {
            CreateMap<Project, ProjectModel>()
                .ForMember(p => p.id, o => o.MapFrom(s => s.LatestUpdate.SyncId))
                .ForMember(p => p.project_id, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(p => p.project_name, o => o.MapFrom(s => s.Name))
                .ForMember(p => p.start_date, o => o.MapFrom(s => s.StartDate))
                .ForMember(p => p.short_desc, o => o.MapFrom(s => s.Description))
                .ForMember(p => p.phase, o => o.MapFrom(s => s.LatestUpdate.Phase.ViewKey))
                .ForMember(p => p.category, o => o.MapFrom(s => s.Category.ViewKey))
                .ForMember(p => p.subcat, o => o.Ignore()) // TODO: add a field for this
                .ForMember(p => p.rag, o => o.MapFrom(s => s.LatestUpdate.RAGStatus.ViewKey))
                .ForMember(p => p.update, o => o.MapFrom(s => s.LatestUpdate.Text))
                .ForMember(p => p.oddlead, o => o.Ignore()) // TODO: add a field for the lead name
                .ForMember(p => p.oddlead_email, o => o.MapFrom(s => s.Lead.Email))
                .ForMember(p => p.servicelead, o => o.Ignore()) // TODO: add a field for the lead name
                .ForMember(p => p.servicelead_email, o => o.MapFrom(s => s.ServiceLead.Email))
                .ForMember(p => p.priority_main, o => o.MapFrom(s => s.Priority.HasValue ? s.Priority.Value.ToString("D2") : string.Empty))
                .ForMember(p => p.funded, o => o.MapFrom(s => s.Funded.ToString()))
                .ForMember(p => p.confidence, o => o.MapFrom(s => s.Confidence.ToString()))
                .ForMember(p => p.priorities, o => o.MapFrom(s => s.Priorities.ToString()))
                .ForMember(p => p.benefits, o => o.MapFrom(s => s.Benefits.ToString()))
                .ForMember(p => p.criticality, o => o.MapFrom(s => s.Criticality.ToString()))
                .ForMember(p => p.budget, o => o.MapFrom(s => s.LatestUpdate.Budget))
                .ForMember(p => p.spent, o => o.MapFrom(s => s.LatestUpdate.Spent))
                .ForMember(p => p.documents, o => o.Ignore()) // TODO: add a field for this
                .ForMember(p => p.timestamp, o => o.MapFrom(s => s.LatestUpdate.Timestamp))

                .ForMember(p => p.pgroup, o => o.Ignore()) // TODO: add a field for this
                .ForMember(p => p.link, o => o.Ignore()) // TODO: add a field for this
                .ForMember(p => p.rels, o => o.MapFrom(s => string.Join(", ", s.RelatedProjects.Select(rp => rp.Reservation.ProjectId))))
                .ForMember(p => p.dependencies, o => o.MapFrom(s => string.Join(", ", s.DependantProjects.Select(rp => rp.Reservation.ProjectId))))
                .ForMember(p => p.team, o => o.MapFrom(s => s.Team))
                .ForMember(p => p.onhold, o => o.MapFrom(s => s.LatestUpdate.OnHoldStatus.Name))
                .ForMember(p => p.expend, o => o.MapFrom(s => s.ExpectedEndDate))
                .ForMember(p => p.hardend, o => o.MapFrom(s => s.HardEndDate))
                .ForMember(p => p.actstart, o => o.MapFrom(s => s.ActualStartDate))
                .ForMember(p => p.project_size, o => o.MapFrom(s => s.Size.ViewKey))
                .ForMember(p => p.oddlead_role, o => o.Ignore()) // TODO: add a field for this
                .ForMember(p => p.budgettype, o => o.MapFrom(s => s.BudgetType.ViewKey))
                .ForMember(p => p.direct, o => o.MapFrom(s => s.Directorate))
                .ForMember(p => p.expendp, o => o.MapFrom(s => s.LatestUpdate.ExpectedCurrentPhaseEnd))
                .ForMember(p => p.p_comp, o => o.MapFrom(s => s.LatestUpdate.PercentageComplete))
                .ForMember(p => p.max_time, o => o.MapFrom(s => s.LatestUpdate.Timestamp))
                .ForMember(p => p.min_time, o => o.MapFrom(s => s.FirstUpdate.Timestamp))
                .ForMember(p => p.g6team, o => o.MapFrom(s => s.Lead.G6team))
                .ForMember(p => p.new_flag, o => o.MapFrom(s => s.IsNew ? "Y" : "N"))
                .ForMember(p => p.first_completed, o => o.MapFrom<FirstCompletedResolver, Project>(s => s))
                .ForMember(p => p.business_case_number, o => o.Ignore())
                .ForMember(p => p.fs_number, o => o.Ignore())
                .ForMember(p => p.risk_rating, o => o.Ignore())
                .ForMember(p => p.theme, o => o.Ignore())
                .ForMember(p => p.project_type, o => o.Ignore())
                .ForMember(p => p.strategic_objectives, o => o.Ignore())
                .ForMember(p => p.programme, o => o.Ignore())
                .ForMember(p => p.programme_description, o => o.Ignore())
                .ForMember(p => p.key_contact1, o => o.Ignore())
                .ForMember(p => p.key_contact2, o => o.Ignore())
                .ForMember(p => p.key_contact3, o => o.Ignore())
                .ForMember(p => p.supplier, o => o.Ignore())
                .ForMember(p => p.actual_end_date, o => o.Ignore())
                .ForMember(p => p.milestones, o => o.Ignore())
                .ForMember(p => p.how_get_green, o => o.Ignore())
                .ForMember(p => p.forward_look, o => o.Ignore())
                .ForMember(p => p.emerging_issues, o => o.Ignore())
                .ForMember(p => p.forecast_spend, o => o.Ignore())
                .ForMember(p => p.budget_field1, o => o.Ignore())
                .ForMember(p => p.cost_centre, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_gatenumber, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_gatecompleted, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_nextgate, o => o.Ignore())
                ;
        }

        private void ProjectModel__Project()
        {
            CreateMap<ProjectModel, Project>()
                .ForMember(p => p.Name, o => o.MapFrom(s => s.project_name))
                .ForMember(p => p.StartDate, o => o.MapFrom<PostgresDateResolver, string>(s => s.start_date))
                .ForMember(p => p.ActualStartDate, o => o.MapFrom<PostgresDateResolver, string>(s => s.actstart))
                .ForMember(p => p.ExpectedEndDate, o => o.MapFrom<PostgresDateResolver, string>(s => s.expend))
                .ForMember(p => p.HardEndDate, o => o.MapFrom<PostgresDateResolver, string>(s => s.hardend))
                .ForMember(p => p.Description, o => o.MapFrom(s => s.short_desc))
                .ForMember(p => p.Priority, o => o.MapFrom<NullableIntResolver, string>(s => s.priority_main))
                .ForMember(p => p.Directorate, o => o.MapFrom(s => s.direct))
                .ForMember(p => p.Funded, o => o.MapFrom<IntResolver, string>(s => s.funded))
                .ForMember(p => p.Confidence, o => o.MapFrom<IntResolver, string>(s => s.confidence))
                .ForMember(p => p.Benefits, o => o.MapFrom<IntResolver, string>(s => s.benefits))
                .ForMember(p => p.Criticality, o => o.MapFrom<IntResolver, string>(s => s.criticality))
                .ForMember(p => p.Team, o => o.MapFrom(s => s.team))
                .ForMember(p => p.Lead, o => o.MapFrom<ProjectLeadResolver, string>(s => s.oddlead_email))
                .ForMember(p => p.ServiceLead, o => o.MapFrom<ProjectLeadResolver, string>(s => s.servicelead_email))
                .ForMember(p => p.RelatedProjects, o => o.MapFrom<ProjectCollectionResolver, string>(s => s.rels))
                .ForMember(p => p.DependantProjects, o => o.MapFrom<ProjectCollectionResolver, string>(s => s.dependencies))
                .ForMember(p => p.Category, o => o.MapFrom<ConfigCategoryResolver, string>(s => s.category))
                .ForMember(p => p.Size, o => o.MapFrom<ConfigProjectSizeResolver, string>(s => s.project_size))
                .ForMember(p => p.BudgetType, o => o.MapFrom<ConfigBudgetTypeResolver, string>(s => s.budgettype))
                // Ignore these
                .ForMember(p => p.Portfolios, o => o.Ignore())
                .ForMember(p => p.Updates, o => o.Ignore())
                .ForMember(p => p.FirstUpdate, o => o.Ignore())
                .ForMember(p => p.LatestUpdate, o => o.Ignore())
                .ForMember(p => p.AuditLogs, o => o.Ignore())
                .ForMember(p => p.Reservation, o => o.Ignore())
                // Ignore the keys
                .ForMember(p => p.ProjectReservation_Id, o => o.Ignore())
                .ForMember(p => p.ProjectCategory_Id, o => o.Ignore())
                .ForMember(p => p.ProjectSize_Id, o => o.Ignore())
                .ForMember(p => p.BudgetType_Id, o => o.Ignore())
                .ForMember(p => p.Lead_Id, o => o.Ignore())
                .ForMember(p => p.ServiceLead_Id, o => o.Ignore())
                .ForMember(p => p.FirstUpdate_Id, o => o.Ignore())
                .ForMember(p => p.LatestUpdate_Id, o => o.Ignore())
            ;
        }

        private void ProjectModel__ProjectUpdateItem()
        {
            CreateMap<ProjectModel, ProjectUpdateItem>()
                .ForMember(p => p.Id, o => o.Ignore())
                .ForMember(p => p.Project_Id, o => o.Ignore())
                .ForMember(p => p.Project, o => o.Ignore())
                .ForMember(p => p.Person, o => o.Ignore())
                .ForMember(p => p.SyncId, o => o.MapFrom(s => s.id))
                .ForMember(p => p.Timestamp, o => o.MapFrom(s => s.timestamp))
                .ForMember(p => p.Text, o => o.MapFrom(s => s.update))
                .ForMember(p => p.PercentageComplete, o => o.MapFrom(s => s.p_comp))
                .ForMember(p => p.RAGStatus, o => o.MapFrom<ConfigRAGStatusResolver, string>(s => s.rag))
                .ForMember(p => p.Phase, o => o.MapFrom<ConfigPhaseStatusResolver, string>(s => s.phase))
                .ForMember(p => p.OnHoldStatus, o => o.MapFrom<ConfigOnHoldStatusResolver, string>(s => s.onhold))
                .ForMember(p => p.Budget, o => o.MapFrom<DecimalResolver, string>(s => s.budget))
                .ForMember(p => p.Spent, o => o.MapFrom<DecimalResolver, string>(s => s.spent))
                .ForMember(p => p.ExpectedCurrentPhaseEnd, o => o.MapFrom<PostgresDateResolver, string>(s => s.expendp))
                ;
        }

        private void ProjectUpdateItem__ProjectUpdateModel()
        {
            CreateMap<ProjectUpdateItem, ProjectUpdateModel>()
                .ForMember(d => d.project_id, o => o.MapFrom(s => s.Project.Reservation.ProjectId))
                .ForMember(d => d.timestamp, o => o.MapFrom<OutputTimestampResolver, DateTime>(s => s.Timestamp))
                .ForMember(d => d.max_timestamp, o => o.MapFrom<OutputTimestampResolver, DateTime>(s => s.Project.LatestUpdate.Timestamp))
                .ForMember(d => d.date, o => o.MapFrom(s => s.Timestamp.Date))
                .ForMember(d => d.update, o => o.MapFrom(s => s.Text))
                ;
        }


    }

    public class IntResolver : IMemberValueResolver<object, object, string, int>
    {
        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            int result;
            return int.TryParse(sourceMember, out result) ? result : 0;
        }
    }
    public class NullableIntResolver : IMemberValueResolver<object, object, string, int?>
    {
        public int? Resolve(object source, object destination, string sourceMember, int? destMember, ResolutionContext context)
        {
            int result;
            return int.TryParse(sourceMember, out result) ? (int?)result : null;
        }
    }
    public class DecimalResolver : IMemberValueResolver<object, object, string, decimal>
    {
        public decimal Resolve(object source, object destination, string sourceMember, decimal destMember, ResolutionContext context)
        {
            decimal result;
            return decimal.TryParse(sourceMember, out result) ? result : 0m;
        }
    }

    public class ProjectLeadResolver : IMemberValueResolver<object, object, string, Person>
    {
        public Person Resolve(object source, object destination, string sourceMember, Person destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return portfolioContext.People.SingleOrDefault(p => p.Email == sourceMember);
        }
    }


    public class FirstCompletedResolver : IMemberValueResolver<Project, ProjectModel, Project, DateTime?>
    {
        public DateTime? Resolve(Project source, ProjectModel destination, Project sourceMember, DateTime? destMember, ResolutionContext context)
        {
            var completedPhase = source.Reservation.Portfolio.Configuration.CompletedPhase;
            var firstCompletePhase = source.Updates.Where(u => u.Phase == completedPhase).OrderBy(u => u.Timestamp).FirstOrDefault();
            return firstCompletePhase?.Timestamp;
        }
    }

    public class OutputTimestampResolver : IMemberValueResolver<object, object, DateTime, string>
    {
        public string Resolve(object source, object destination, DateTime sourceMember, string destMember, ResolutionContext context)
        {
            return sourceMember.ToString(ProjectMappingProfile.TimeOutputFormat);
        }
    }



    public class ProjectCollectionResolver : IMemberValueResolver<object, Project, string, ICollection<Project>>
    {
        public ICollection<Project> Resolve(object source, Project destination, string sourceMember, ICollection<Project> destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            var result = new List<Project>();
            if (!string.IsNullOrEmpty(sourceMember))
            {
                // Add missing related projects
                var projectIds = sourceMember.Split(',');
                foreach (var relatedProjectId in projectIds)
                {
                    var trimmedId = relatedProjectId.Trim();
                    if (!result.Any(p => p.Reservation.ProjectId == trimmedId))
                    {
                        var project = portfolioContext.Projects.SingleOrDefault(p => p.Reservation.ProjectId == trimmedId);
                        if (project != null)
                        {
                            result.Add(project);
                        }
                    }
                }
            }
            return result;
        }
    }


    public class ConfigCategoryResolver : IMemberValueResolver<object, Project, string, ProjectCategory>
    {
        public ProjectCategory Resolve(object source, Project destination, string sourceMember, ProjectCategory destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return destination.Reservation.Portfolio.Configuration.Categories.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class ConfigProjectSizeResolver : IMemberValueResolver<object, Project, string, ProjectSize>
    {
        public ProjectSize Resolve(object source, Project destination, string sourceMember, ProjectSize destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return destination.Reservation.Portfolio.Configuration.ProjectSizes.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class ConfigBudgetTypeResolver : IMemberValueResolver<object, Project, string, BudgetType>
    {
        public BudgetType Resolve(object source, Project destination, string sourceMember, BudgetType destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return destination.Reservation.Portfolio.Configuration.BudgetTypes.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }


    public class ConfigRAGStatusResolver : IMemberValueResolver<object, ProjectUpdateItem, string, ProjectRAGStatus>
    {
        public ProjectRAGStatus Resolve(object source, ProjectUpdateItem destination, string sourceMember, ProjectRAGStatus destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return destination.Project.Reservation.Portfolio.Configuration.RAGStatuses.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class ConfigPhaseStatusResolver : IMemberValueResolver<object, ProjectUpdateItem, string, ProjectPhase>
    {
        public ProjectPhase Resolve(object source, ProjectUpdateItem destination, string sourceMember, ProjectPhase destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return destination.Project.Reservation.Portfolio.Configuration.Phases.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class ConfigOnHoldStatusResolver : IMemberValueResolver<object, ProjectUpdateItem, string, ProjectOnHoldStatus>
    {
        public ProjectOnHoldStatus Resolve(object source, ProjectUpdateItem destination, string sourceMember, ProjectOnHoldStatus destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return destination.Project.Reservation.Portfolio.Configuration.OnHoldStatuses.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }


}