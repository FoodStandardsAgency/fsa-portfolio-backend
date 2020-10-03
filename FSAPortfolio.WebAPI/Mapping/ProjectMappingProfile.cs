using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.PostgreSQL.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Controllers;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public const string PortfolioContextKey = "portfolioContext";

        public ProjectMappingProfile()
        {
            CreateMap<DateTime, string>().ConvertUsing(d => d.ToString("dd/mm/yyyy"));
            CreateMap<DateTime?, string>().ConvertUsing(d => d.HasValue ? d.Value.ToString("dd/mm/yyyy") : "00/00/00");

            // Outbound
            Project__latest_projects();

            // Inbound
            project__Project();
            project__ProjectUpdateItem();
        }

        private void project__ProjectUpdateItem()
        {
            CreateMap<project, ProjectUpdateItem>()
                .ForMember(p => p.Id, o => o.Ignore())
                .ForMember(p => p.Project_Id, o => o.Ignore())
                .ForMember(p => p.Project, o => o.Ignore())
                .ForMember(p => p.Person, o => o.Ignore())
                .ForMember(p => p.SyncId, o => o.MapFrom(s => s.id))
                .ForMember(p => p.Timestamp, o => o.MapFrom(s => s.timestamp))
                .ForMember(p => p.Text, o => o.MapFrom(s => s.update))
                .ForMember(p => p.PercentageComplete, o => o.MapFrom(s => s.p_comp))
                .ForMember(p => p.RAGStatus, o => o.MapFrom<PostgresRAGStatusResolver, string>(s => s.rag))
                .ForMember(p => p.Phase, o => o.MapFrom<PostgresPhaseStatusResolver, string>(s => s.phase))
                .ForMember(p => p.OnHoldStatus, o => o.MapFrom<PostgresOnHoldStatusResolver, string>(s => s.onhold))
                .ForMember(p => p.Budget, o => o.MapFrom<PostgresDecimalResolver, string>(s => s.budget))
                .ForMember(p => p.Spent, o => o.MapFrom<PostgresDecimalResolver, string>(s => s.spent))
                .ForMember(p => p.ExpectedCurrentPhaseEnd, o => o.MapFrom<PostgresDateResolver, string>(s => s.expendp))
                ;
        }

        private void project__Project()
        {
            CreateMap<project, Project>()
                .ForMember(p => p.ProjectId, o => o.MapFrom(s => s.project_id))
                .ForMember(p => p.Name, o => o.MapFrom(s => s.project_name))
                .ForMember(p => p.StartDate, o => o.MapFrom<PostgresDateResolver, string>(s => s.start_date))
                .ForMember(p => p.ActualStartDate, o => o.MapFrom<PostgresDateResolver, string>(s => s.actstart))
                .ForMember(p => p.ExpectedEndDate, o => o.MapFrom<PostgresDateResolver, string>(s => s.expend))
                .ForMember(p => p.HardEndDate, o => o.MapFrom<PostgresDateResolver, string>(s => s.hardend))
                .ForMember(p => p.Description, o => o.MapFrom(s => s.short_desc))
                .ForMember(p => p.Priority, o => o.MapFrom<PostgresNullableIntResolver, string>(s => s.priority_main))
                .ForMember(p => p.Directorate, o => o.MapFrom(s => s.direct))
                .ForMember(p => p.Funded, o => o.MapFrom<PostgresIntResolver, string>(s => s.funded))
                .ForMember(p => p.Confidence, o => o.MapFrom<PostgresIntResolver, string>(s => s.confidence))
                .ForMember(p => p.Benefits, o => o.MapFrom<PostgresIntResolver, string>(s => s.benefits))
                .ForMember(p => p.Criticality, o => o.MapFrom<PostgresIntResolver, string>(s => s.criticality))
                .ForMember(p => p.Team, o => o.MapFrom(s => s.team))
                .ForMember(p => p.Lead, o => o.MapFrom<PostgresProjectLeadResolver, string>(s => s.oddlead_email))
                .ForMember(p => p.ServiceLead, o => o.MapFrom<PostgresProjectLeadResolver, string>(s => s.servicelead_email))
                .ForMember(p => p.RelatedProjects, o => o.MapFrom<RelatedProjectResolver, string>(s => s.rels))
                .ForMember(p => p.Category, o => o.MapFrom<PostgresCategoryResolver, string>(s => s.category))
                .ForMember(p => p.Size, o => o.MapFrom<PostgresSizeResolver, string>(s => s.project_size))
                .ForMember(p => p.BudgetType, o => o.MapFrom<PostgresBudgetTypeResolver, string>(s => s.budgettype))
                // Ignore these
                .ForMember(p => p.Portfolios, o => o.Ignore())
                .ForMember(p => p.Updates, o => o.Ignore())
                .ForMember(p => p.FirstUpdate, o => o.Ignore())
                .ForMember(p => p.LatestUpdate, o => o.Ignore())
                .ForMember(p => p.AuditLogs, o => o.Ignore())
                // Ignore the keys
                .ForMember(p => p.Id, o => o.Ignore())
                .ForMember(p => p.ProjectCategory_Id, o => o.Ignore())
                .ForMember(p => p.ProjectSize_Id, o => o.Ignore())
                .ForMember(p => p.BudgetType_Id, o => o.Ignore())
                .ForMember(p => p.Lead_Id, o => o.Ignore())
                .ForMember(p => p.ServiceLead_Id, o => o.Ignore())
                .ForMember(p => p.FirstUpdate_Id, o => o.Ignore())
                .ForMember(p => p.LatestUpdate_Id, o => o.Ignore())
            ;
        }

        private void Project__latest_projects()
        {
            CreateMap<Project, latest_projects>()
                .ForMember(p => p.id, o => o.MapFrom(s => s.LatestUpdate.SyncId))
                .ForMember(p => p.project_id, o => o.MapFrom(s => s.ProjectId))
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
                .ForMember(p => p.toupdate, o => o.Ignore()) // TODO: add a field for this
                .ForMember(p => p.rels, o => o.MapFrom(s => string.Join(", ", s.RelatedProjects.Select(rp => rp.ProjectId))))
                .ForMember(p => p.team, o => o.MapFrom(s => s.Team))
                .ForMember(p => p.onhold, o => o.MapFrom(s => s.LatestUpdate.OnHoldStatus.Name))
                .ForMember(p => p.expend, o => o.MapFrom(s => s.ExpectedEndDate))
                .ForMember(p => p.hardend, o => o.MapFrom(s => s.HardEndDate))
                .ForMember(p => p.actstart, o => o.MapFrom(s => s.ActualStartDate))
                .ForMember(p => p.dependencies, o => o.Ignore()) // TODO: add a field for this
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
                ;
        }
    }

    public class PostgresDateResolver : IMemberValueResolver<object, object, string, DateTime?>
    {
        public DateTime? Resolve(object source, object destination, string date, DateTime? destMember, ResolutionContext context)
        {
            DateTime result;
            return DateTime.TryParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? (DateTime?)result : null;
        }
    }
    public class PostgresIntResolver : IMemberValueResolver<object, object, string, int>
    {
        public int Resolve(object source, object destination, string sourceMember, int destMember, ResolutionContext context)
        {
            int result;
            return int.TryParse(sourceMember, out result) ? result : 0;
        }
    }
    public class PostgresNullableIntResolver : IMemberValueResolver<project, Project, string, int?>
    {
        public int? Resolve(project source, Project destination, string sourceMember, int? destMember, ResolutionContext context)
        {
            int result;
            return int.TryParse(sourceMember, out result) ? (int?)result : null;
        }
    }
    public class PostgresDecimalResolver : IMemberValueResolver<object, object, string, decimal>
    {
        public decimal Resolve(object source, object destination, string sourceMember, decimal destMember, ResolutionContext context)
        {
            decimal result;
            return decimal.TryParse(sourceMember, out result) ? result : 0m;
        }
    }

    public class PostgresCategoryResolver : IMemberValueResolver<object, object, string, ProjectCategory>
    {
        public ProjectCategory Resolve(object source, object destination, string sourceMember, ProjectCategory destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return portfolioContext.ProjectCategories.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class PostgresSizeResolver : IMemberValueResolver<object, object, string, ProjectSize>
    {
        public ProjectSize Resolve(object source, object destination, string sourceMember, ProjectSize destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return portfolioContext.ProjectSizes.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class PostgresBudgetTypeResolver : IMemberValueResolver<object, object, string, BudgetType>
    {
        public BudgetType Resolve(object source, object destination, string sourceMember, BudgetType destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return portfolioContext.BudgetTypes.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }

    public class PostgresProjectLeadResolver : IMemberValueResolver<object, object, string, Person>
    {
        public Person Resolve(object source, object destination, string sourceMember, Person destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return portfolioContext.People.SingleOrDefault(p => p.Email == sourceMember);
        }
    }

    public class PostgresRAGStatusResolver : IMemberValueResolver<object, object, string, ProjectRAGStatus>
    {
        public ProjectRAGStatus Resolve(object source, object destination, string sourceMember, ProjectRAGStatus destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return portfolioContext.ProjectRAGStatuses.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class PostgresPhaseStatusResolver : IMemberValueResolver<object, object, string, ProjectPhase>
    {
        public ProjectPhase Resolve(object source, object destination, string sourceMember, ProjectPhase destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return portfolioContext.ProjectPhases.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class PostgresOnHoldStatusResolver : IMemberValueResolver<object, object, string, ProjectOnHoldStatus>
    {
        public ProjectOnHoldStatus Resolve(object source, object destination, string sourceMember, ProjectOnHoldStatus destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            return portfolioContext.ProjectOnHoldStatuses.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }

    public class RelatedProjectResolver : IMemberValueResolver<object, object, string, ICollection<Project>>
    {
        public ICollection<Project> Resolve(object source, object destination, string sourceMember, ICollection<Project> destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[ProjectMappingProfile.PortfolioContextKey];
            var relatedProjects = destMember ?? new List<Project>();
            if (string.IsNullOrEmpty(sourceMember))
            {
                relatedProjects.Clear();
            }
            else
            {
                // Add missing related projects
                var relatedProjectIds = sourceMember.Split(',');
                foreach (var relatedProjectId in relatedProjectIds)
                {
                    var trimmedId = relatedProjectId.Trim();
                    if (!relatedProjects.Any(p => p.ProjectId == trimmedId))
                    {
                        var relatedProject = portfolioContext.Projects.SingleOrDefault(p => p.ProjectId == trimmedId);
                        if (relatedProject != null)
                        {
                            relatedProjects.Add(relatedProject);
                        }
                    }
                }

                // Remove unrequired related projects
                foreach(var p in relatedProjects.ToArray())
                {
                    if(!relatedProjectIds.Any(id => id == p.ProjectId))
                    {
                        relatedProjects.Remove(p);
                    }
                }
            }


            return relatedProjects;
        }
    }

}