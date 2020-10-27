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

namespace FSAPortfolio.WebAPI.Mapping.Projects
{
    public class ProjectUpdateModelProfile : Profile
    {

        public ProjectUpdateModelProfile()
        {
            // Inbound
            ProjectUpdateModel__Project();
            ProjectUpdateModel__ProjectUpdateItem();
        }

        private void ProjectUpdateModel__Project()
        {
            CreateMap<ProjectUpdateModel, Project>()
                .ForMember(p => p.Name, o => o.MapFrom(s => s.project_name))
                .ForMember(p => p.StartDate, o => o.MapFrom(s => s.start_date))
                .ForMember(p => p.ActualStartDate, o => o.MapFrom(s => s.actstart))
                .ForMember(p => p.ExpectedEndDate, o => o.MapFrom(s => s.expend))
                .ForMember(p => p.HardEndDate, o => o.MapFrom(s => s.hardend))
                .ForMember(p => p.ActualEndDate, o => o.MapFrom(s => s.actual_end_date))
                .ForMember(p => p.Description, o => o.MapFrom(s => s.short_desc))
                .ForMember(p => p.Priority, o => o.MapFrom<NullableIntResolver, string>(s => s.priority_main))
                .ForMember(p => p.Directorate, o => o.MapFrom(s => s.direct))
                .ForMember(p => p.Funded, o => o.MapFrom(s => s.funded))
                .ForMember(p => p.Confidence, o => o.MapFrom(s => s.confidence))
                .ForMember(p => p.Benefits, o => o.MapFrom(s => s.benefits))
                .ForMember(p => p.Criticality, o => o.MapFrom(s => s.criticality))
                .ForMember(p => p.Team, o => o.MapFrom(s => s.team))
                .ForMember(p => p.Lead, o => o.MapFrom<ProjectLeadResolver, string>(s => s.oddlead_email))
                .ForMember(p => p.ServiceLead, o => o.MapFrom<ProjectLeadResolver, string>(s => s.servicelead_email))
                .ForMember(p => p.RelatedProjects, o => o.MapFrom<ProjectCollectionResolver, string[]>(s => s.rels))
                .ForMember(p => p.DependantProjects, o => o.MapFrom<ProjectCollectionResolver, string[]>(s => s.dependencies))
                .ForMember(p => p.Category, o => o.MapFrom<ConfigCategoryResolver, string>(s => s.category))
                .ForMember(p => p.Subcategories, o => o.MapFrom<ConfigSubcategoryResolver, string[]>(s => s.subcat))
                .ForMember(p => p.Size, o => o.MapFrom<ConfigProjectSizeResolver, string>(s => s.project_size))
                .ForMember(p => p.BudgetType, o => o.MapFrom<ConfigBudgetTypeResolver, string>(s => s.budgettype))
                .ForMember(p => p.ProjectData, o => o.MapFrom<ProjectDataInboundResolver>())
                .ForMember(p => p.Documents, o => o.Ignore()) // TODO: need a mapping for this.
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
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return portfolioContext.People.SingleOrDefault(p => p.Email == sourceMember);
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


    public class ConfigCategoryResolver : IMemberValueResolver<object, Project, string, ProjectCategory>
    {
        public ProjectCategory Resolve(object source, Project destination, string sourceMember, ProjectCategory destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return destination.Reservation.Portfolio.Configuration.Categories.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class ConfigSubcategoryResolver : IMemberValueResolver<object, Project, string[], ICollection<ProjectCategory>>
    {
        public ICollection<ProjectCategory> Resolve(object source, Project destination, string[] sourceMember, ICollection<ProjectCategory> destMember, ResolutionContext context)
        {
            ICollection<ProjectCategory> result = null;
            if (sourceMember != null && sourceMember.Length > 0)
            {
                var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
                result = destination.Reservation.Portfolio.Configuration.Categories.Where(c => sourceMember.Contains(c.ViewKey)).ToList();
            }
            return result;
        }
    }
    public class ConfigProjectSizeResolver : IMemberValueResolver<object, Project, string, ProjectSize>
    {
        public ProjectSize Resolve(object source, Project destination, string sourceMember, ProjectSize destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return destination.Reservation.Portfolio.Configuration.ProjectSizes.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class ConfigBudgetTypeResolver : IMemberValueResolver<object, Project, string, BudgetType>
    {
        public BudgetType Resolve(object source, Project destination, string sourceMember, BudgetType destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return destination.Reservation.Portfolio.Configuration.BudgetTypes.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }


    public class ConfigRAGStatusResolver : IMemberValueResolver<object, ProjectUpdateItem, string, ProjectRAGStatus>
    {
        public ProjectRAGStatus Resolve(object source, ProjectUpdateItem destination, string sourceMember, ProjectRAGStatus destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return destination.Project.Reservation.Portfolio.Configuration.RAGStatuses.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class ConfigPhaseStatusResolver : IMemberValueResolver<object, ProjectUpdateItem, string, ProjectPhase>
    {
        public ProjectPhase Resolve(object source, ProjectUpdateItem destination, string sourceMember, ProjectPhase destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return destination.Project.Reservation.Portfolio.Configuration.Phases.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }
    public class ConfigOnHoldStatusResolver : IMemberValueResolver<object, ProjectUpdateItem, string, ProjectOnHoldStatus>
    {
        public ProjectOnHoldStatus Resolve(object source, ProjectUpdateItem destination, string sourceMember, ProjectOnHoldStatus destMember, ResolutionContext context)
        {
            var portfolioContext = (PortfolioContext)context.Items[nameof(PortfolioContext)];
            return destination.Project.Reservation.Portfolio.Configuration.OnHoldStatuses.SingleOrDefault(c => c.ViewKey == sourceMember);
        }
    }

}