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
using System.Data.Entity;
using System.Reflection;
using Newtonsoft.Json;
using FSAPortfolio.WebAPI.App.Mapping.Projects.Resolvers;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects
{
    public class ProjectUpdateModelProfile : Profile
    {

        public ProjectUpdateModelProfile()
        {
            // Inbound
            ProjectUpdateModel__Project();
            ProjectUpdateModel__ProjectUpdateItem();

            CreateMap<ProjectDateEditModel, ProjectDate>().ConvertUsing<ProjectDateEditViewResolver>();
            CreateMap<LinkModel, ProjectLink>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Link, o => o.MapFrom(s => s.Link))
                ;

        }

        private void ProjectUpdateModel__Project()
        {
            CreateMap<ProjectUpdateModel, Project>()
                .ForMember(p => p.Name, o => o.MapFrom(s => s.project_name))
                .ForMember(p => p.StartDate, o => { o.MapFrom(s => s.start_date); o.NullSubstitute(new ProjectDateEditModel()); })
                .ForMember(p => p.ActualStartDate, o => { o.MapFrom(s => s.actstart); o.NullSubstitute(new ProjectDateEditModel()); })
                .ForMember(p => p.ExpectedEndDate, o => { o.MapFrom(s => s.expend); o.NullSubstitute(new ProjectDateEditModel()); })
                .ForMember(p => p.HardEndDate, o => { o.MapFrom(s => s.hardend); o.NullSubstitute(new ProjectDateEditModel()); })
                .ForMember(p => p.ActualEndDate, o => { o.MapFrom(s => s.actual_end_date); o.NullSubstitute(new ProjectDateEditModel()); })
                .ForMember(p => p.AssuranceGateCompletedDate, o => { o.MapFrom(s => s.fsaproc_assurance_gatecompleted); o.NullSubstitute(new ProjectDateEditModel()); })
                .ForMember(p => p.Description, o => o.MapFrom(s => s.short_desc))
                .ForMember(p => p.Priority, o => o.MapFrom<NullableIntResolver, string>(s => s.priority_main))
                .ForMember(p => p.Directorate, o => o.MapFrom<DirectorateResolver, string>(s => s.direct))
                .ForMember(p => p.Funded, o => o.MapFrom(s => s.funded))
                .ForMember(p => p.Confidence, o => o.MapFrom(s => s.confidence))
                .ForMember(p => p.Benefits, o => o.MapFrom(s => s.benefits))
                .ForMember(p => p.Criticality, o => o.MapFrom(s => s.criticality))
                .ForMember(p => p.Theme, o => o.MapFrom(s => s.theme))
                .ForMember(p => p.ProjectType, o => o.MapFrom(s => s.project_type))
                .ForMember(p => p.StrategicObjectives, o => o.MapFrom(s => s.strategic_objectives))
                .ForMember(p => p.Programme, o => o.MapFrom(s => s.programme))
                .ForMember(p => p.RelatedProjects, o => o.MapFrom<ProjectCollectionResolver, string[]>(s => s.rels))
                .ForMember(p => p.DependantProjects, o => o.MapFrom<ProjectCollectionResolver, string[]>(s => s.dependencies))
                .ForMember(p => p.Category, o => o.MapFrom<ConfigCategoryResolver, string>(s => s.category))
                .ForMember(p => p.Subcategories, o => o.MapFrom<ConfigSubcategoryResolver, string[]>(s => s.subcat))
                .ForMember(p => p.Size, o => o.MapFrom<ConfigProjectSizeResolver, string>(s => s.project_size))
                .ForMember(p => p.BudgetType, o => o.MapFrom<ConfigBudgetTypeResolver, string>(s => s.budgettype))
                .ForMember(p => p.ProjectData, o => o.MapFrom<ProjectDataInboundResolver>())
                .ForMember(p => p.Documents, o => o.MapFrom(s => s.documents))
                .ForMember(p => p.Milestones, o => o.MapFrom<MilestoneResolver, MilestoneEditModel[]>(s => s.milestones))
                .ForMember(p => p.Supplier, o => o.MapFrom(s => s.supplier))
                .ForMember(p => p.LeadRole, o => o.MapFrom(s => s.oddlead_role))

                .ForMember(p => p.BusinessCaseNumber, o => o.MapFrom(s => s.business_case_number))
                .ForMember(p => p.FSNumber, o => o.MapFrom(s => s.fs_number))
                .ForMember(p => p.RiskRating, o => o.MapFrom(s => s.risk_rating))
                .ForMember(p => p.ProgrammeDescription, o => o.MapFrom(s => s.programme_description))
                .ForMember(p => p.ChannelLink, o => { o.MapFrom(s => s.link); o.NullSubstitute(new LinkModel()); })

                .ForPath(p => p.TeamSettings.Setting1, o => o.MapFrom(s => s.project_team_setting1))
                .ForPath(p => p.TeamSettings.Setting2, o => o.MapFrom(s => s.project_team_setting2))
                .ForPath(p => p.TeamSettings.Option1, o => o.MapFrom(s => s.project_team_option1))
                .ForPath(p => p.TeamSettings.Option2, o => o.MapFrom(s => s.project_team_option2))

                .ForPath(p => p.PlanSettings.Setting1, o => o.MapFrom(s => s.project_plan_setting1))
                .ForPath(p => p.PlanSettings.Setting2, o => o.MapFrom(s => s.project_plan_setting2))
                .ForPath(p => p.PlanSettings.Option1, o => o.MapFrom(s => s.project_plan_option1))
                .ForPath(p => p.PlanSettings.Option2, o => o.MapFrom(s => s.project_plan_option2))

                .ForPath(p => p.ProgressSettings.Setting1, o => o.MapFrom(s => s.progress_setting1))
                .ForPath(p => p.ProgressSettings.Setting2, o => o.MapFrom(s => s.progress_setting2))
                .ForPath(p => p.ProgressSettings.Option1, o => o.MapFrom(s => s.progress_option1))
                .ForPath(p => p.ProgressSettings.Option2, o => o.MapFrom(s => s.progress_option2))

                .ForPath(p => p.BudgetSettings.Setting1, o => o.MapFrom(s => s.budget_field1))
                .ForPath(p => p.BudgetSettings.Setting2, o => o.MapFrom(s => s.budget_field2))
                .ForPath(p => p.BudgetSettings.Option1, o => o.MapFrom(s => s.budget_option1))
                .ForPath(p => p.BudgetSettings.Option2, o => o.MapFrom(s => s.budget_option2))

                .ForPath(p => p.ProcessSettings.Setting1, o => o.MapFrom(s => s.processes_setting1))
                .ForPath(p => p.ProcessSettings.Setting2, o => o.MapFrom(s => s.processes_setting2))
                .ForPath(p => p.ProcessSettings.Option1, o => o.MapFrom(s => s.processes_option1))
                .ForPath(p => p.ProcessSettings.Option2, o => o.MapFrom(s => s.processes_option2))

                .ForMember(p => p.HowToGetToGreen, o => o.MapFrom(s => s.how_get_green))
                .ForMember(p => p.ForwardLook, o => o.MapFrom(s => s.forward_look))
                .ForMember(p => p.EmergingIssues, o => o.MapFrom(s => s.emerging_issues))
                .ForMember(p => p.ForecastSpend, o => o.MapFrom(s => s.forecast_spend))
                .ForMember(p => p.CostCentre, o => o.MapFrom(s => s.cost_centre))
                .ForMember(p => p.AssuranceGateNumber, o => o.MapFrom(s => s.fsaproc_assurance_gatenumber))
                .ForMember(p => p.NextAssuranceGateNumber, o => o.MapFrom(s => s.fsaproc_assurance_nextgate))


                // Have to be mapped manually as requires async request to AD
                .ForMember(p => p.Lead, o => o.Ignore())
                .ForMember(p => p.People, o => o.Ignore())
                .ForMember(p => p.KeyContact1, o => o.Ignore())
                .ForMember(p => p.KeyContact2, o => o.Ignore())
                .ForMember(p => p.KeyContact3, o => o.Ignore())

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
                .ForMember(p => p.FirstUpdate_Id, o => o.Ignore())
                .ForMember(p => p.LatestUpdate_Id, o => o.Ignore())
            ;

            CreateMap<LinkModel, Document>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Link, o => o.MapFrom(s => s.Link))
                .ForMember(p => p.Id, o => o.Ignore())
                .ForMember(p => p.Order, o => o.Ignore())
                ;

            CreateMap<MilestoneEditModel, Milestone>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Project, o => o.Ignore())
                .ForMember(d => d.Project_ProjectReservation_Id, o => o.Ignore())
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.Deadline, o => o.MapFrom(s => s.Deadline))
                ;


        }

        private void ProjectUpdateModel__ProjectUpdateItem()
        {
            CreateMap<ProjectUpdateModel, ProjectUpdateItem>()
                .ForMember(p => p.Id, o => o.Ignore())
                .ForMember(p => p.Project_Id, o => o.Ignore())
                .ForMember(p => p.Project, o => o.Ignore())
                .ForMember(p => p.Person, o => o.Ignore())
                .ForMember(p => p.SyncId, o => o.MapFrom(s => s.id))
                .ForMember(p => p.Timestamp, o => o.Ignore())
                .ForMember(p => p.Text, o => o.MapFrom(s => s.update))
                .ForMember(p => p.PercentageComplete, o => o.MapFrom(s => s.p_comp))
                .ForMember(p => p.RAGStatus, o => o.MapFrom<ConfigRAGStatusResolver, string>(s => s.rag))
                .ForMember(p => p.Phase, o => o.MapFrom<ConfigPhaseStatusResolver, string>(s => s.phase))
                .ForMember(p => p.OnHoldStatus, o => o.MapFrom<ConfigOnHoldStatusResolver, string>(s => s.onhold))
                .ForMember(p => p.Budget, o => o.MapFrom<DecimalResolver, string>(s => s.budget))
                .ForMember(p => p.Spent, o => o.MapFrom<DecimalResolver, string>(s => s.spent))
                .ForMember(p => p.ExpectedCurrentPhaseEnd, o => o.MapFrom(s => s.expendp))
                ;
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


    public class ProjectCollectionResolver : IMemberValueResolver<object, Project, string[], ICollection<Project>>
    {
        public ICollection<Project> Resolve(object source, Project destination, string[] projectIds, ICollection<Project> destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            var result = new List<Project>();
            if (projectIds != null && projectIds.Length > 0)
            {
                // Add missing related projects
                foreach (var projectId in projectIds)
                {
                    var trimmedId = projectId.Trim();
                    if (!result.Any(p => p.Reservation.ProjectId == trimmedId))
                    {
                        var project = portfolioContext.Projects.Include(p => p.Reservation).SingleOrDefault(p => p.Reservation.ProjectId == trimmedId);
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

 
    public class EndOfMonthResolver : IMemberValueResolver<ProjectUpdateModel, Project, DateTime?, DateTime?>
    {
        public DateTime? Resolve(ProjectUpdateModel source, Project destination, DateTime? sourceMember, DateTime? destMember, ResolutionContext context)
        {
            DateTime? result = null;
            if(sourceMember.HasValue)
            {
                result = new DateTime(sourceMember.Value.Year, sourceMember.Value.Month, DateTime.DaysInMonth(sourceMember.Value.Year, sourceMember.Value.Month));
            }
            return result;
        }
    }

    public class ProjectDateEditViewResolver : ITypeConverter<ProjectDateEditModel, ProjectDate>
    {
        public ProjectDate Convert(ProjectDateEditModel source, ProjectDate destination, ResolutionContext context)
        {
            ProjectDate result = new ProjectDate();
            if (source != null)
            {
                if(source.Year.HasValue)
                {
                    result.Flags |= ProjectDateFlags.Year;
                    int day, month;
                    int year = source.Year.Value;

                    if (source.Month.HasValue)
                    {
                        month = source.Month.Value;
                        result.Flags |= ProjectDateFlags.Month;
                    }
                    else
                    {
                        month = 12;
                    }

                    if (source.Day.HasValue)
                    {
                        day = source.Day.Value;
                        result.Flags |= ProjectDateFlags.Day;
                    }
                    else
                    {
                        day = DateTime.DaysInMonth(year, month);
                    }
                    result.Date = new DateTime(year, month, day);
                }
            }
            return result;
        }
    }

    public class MilestoneResolver : IMemberValueResolver<ProjectUpdateModel, Project, MilestoneEditModel[], ICollection<Milestone>>
    {
        public ICollection<Milestone> Resolve(ProjectUpdateModel source, Project destination, MilestoneEditModel[] sourceMember, ICollection<Milestone> destMember, ResolutionContext context)
        {
            ICollection<Milestone> result = new List<Milestone>();

            // Remove 
            if (destMember != null)
            {
                foreach (var entity in destMember.ToList())
                {
                    var portfolioContext = context.Items[nameof(PortfolioContext)] as PortfolioContext;
                    if (sourceMember == null || !sourceMember.Any(s => s.Id == entity.Id))
                        portfolioContext.Milestones.Remove(entity);
                }
            }

            if (sourceMember != null && sourceMember.Length > 0)
            {
                foreach (var milestone in sourceMember)
                {
                    Milestone entity = (milestone.Id > 0 ? destMember.FirstOrDefault(m => m.Id == milestone.Id) : null) ?? new Milestone();
                    context.Mapper.Map(milestone, entity);
                    result.Add(entity);
                }
            }
            return result;
        }
    }
}