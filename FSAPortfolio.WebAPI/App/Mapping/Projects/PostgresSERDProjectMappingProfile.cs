using AutoMapper;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.PostgreSQL.Projects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data.Entity;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.App.Mapping.Projects.Resolvers;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects
{
    public class PostgresSERDProjectMappingProfile : Profile
    {
        public PostgresSERDProjectMappingProfile()
        {
            // Inbound
            project__Project();
            project__ProjectUpdateItem();
        }
        private void project__Project()
        {
            CreateMap<serdproject, Project>()
                .ForMember(p => p.Name, o => o.MapFrom(s => s.project_name))
                .ForMember(p => p.StartDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.start_date))
                .ForMember(p => p.ActualStartDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.actstart))
                .ForMember(p => p.ExpectedEndDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.expend))
                .ForMember(p => p.HardEndDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => s.hardend))
                .ForMember(p => p.ActualEndDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => "00/00/00")) // Isn't one!?
                .ForMember(p => p.AssuranceGateCompletedDate, o => o.MapFrom<PostgresProjectDateResolver, string>(s => "00/00/00")) // Isn't one!?
                .ForMember(p => p.Description, o => o.MapFrom(s => s.short_desc))
                .ForMember(p => p.Priority, o => o.MapFrom<NullableIntResolver, string>(s => s.priority_main))
                .ForMember(p => p.Funded, o => o.MapFrom<PostgresIntResolver, string>(s => s.funded))
                .ForMember(p => p.Confidence, o => o.MapFrom<PostgresIntResolver, string>(s => s.confidence))
                .ForMember(p => p.Benefits, o => o.MapFrom<PostgresIntResolver, string>(s => s.benefits))
                .ForMember(p => p.Criticality, o => o.MapFrom<PostgresIntResolver, string>(s => s.criticality))
                .ForMember(p => p.People, o => o.MapFrom<PostgresTeamCollectionResolver, string>(s => s.team))
                .ForMember(p => p.Lead, o => o.MapFrom<PostgresPersonFromEmailResolver, string>(s => s.lead_email))
                .ForMember(p => p.Supplier, o => o.MapFrom(s => s.supplier))
                .ForMember(p => p.RelatedProjects, o => o.MapFrom<PostgresProjectCollectionResolver, string>(s => s.rels))
                .ForMember(p => p.DependantProjects, o => o.MapFrom<PostgresProjectCollectionResolver, string>(s => s.dependencies))
                .ForMember(p => p.Category, o => o.MapFrom<ConfigCategoryResolver, string>(s => SyncMaps.serd_categoryKeyMap[s.category ?? "13"]))

                .ForMember(p => p.Subcategories, o => o.MapFrom(new SubCategoryResolver(SyncMaps.serd_categoryKeyMap), s => s.subcat))

                .ForMember(p => p.Size, o => o.MapFrom<ConfigProjectSizeResolver, string>(s => SyncMaps.sizeKeyMap[string.Empty])) // TODO: isn't one!?
                .ForMember(p => p.BudgetType, o => o.MapFrom<ConfigBudgetTypeResolver, string>(s => SyncMaps.serd_budgetTypeKeyMap[(s.budgettype ?? "none").Trim()]))
                .ForMember(p => p.ProjectType, o => o.MapFrom(s => SyncMaps.serdProjectTypeMaps[s.project_type ?? string.Empty])) // TODO: SERD
                .ForMember(p => p.ChannelLink, o => o.MapFrom<PostgresLinkResolver, string>(s => s.link))
                .ForMember(p => p.Documents, o => o.MapFrom<PostgresDocumentResolver, string>(s => s.documents))
                .ForMember(p => p.LeadRole, o => o.Ignore()) // Isn't one!?

                .ForMember(p => p.TeamSettings, o => new ProjectGenericSettings())
                .ForMember(p => p.PlanSettings, o => new ProjectGenericSettings())
                .ForMember(p => p.ProgressSettings, o => new ProjectGenericSettings())
                .ForMember(p => p.BudgetSettings, o => new ProjectGenericBudgetSettings())
                .ForMember(p => p.ProcessSettings, o => new ProjectGenericSettings())

                // Ignore these
                .ForMember(p => p.KeyContact1, o => o.Ignore())
                .ForMember(p => p.KeyContact2, o => o.Ignore())
                .ForMember(p => p.KeyContact3, o => o.Ignore())
                .ForMember(p => p.ProjectData, o => o.Ignore())
                .ForMember(p => p.Reservation, o => o.Ignore())
                .ForMember(p => p.Portfolios, o => o.Ignore())
                .ForMember(p => p.Updates, o => o.Ignore())
                .ForMember(p => p.FirstUpdate, o => o.Ignore())
                .ForMember(p => p.LatestUpdate, o => o.Ignore())
                .ForMember(p => p.AuditLogs, o => o.Ignore())
                .ForMember(p => p.Theme, o => o.Ignore())
                .ForMember(p => p.StrategicObjectives, o => o.Ignore())
                .ForMember(p => p.Milestones, o => o.Ignore())
                .ForMember(p => p.Programme, o => o.Ignore())
                .ForMember(p => p.Priorities, o => o.Ignore())

                .ForMember(p => p.BusinessCaseNumber, o => o.Ignore())
                .ForMember(p => p.FSNumber, o => o.Ignore())
                .ForMember(p => p.RiskRating, o => o.Ignore())
                .ForMember(p => p.ProgrammeDescription, o => o.Ignore())

                .ForMember(p => p.HowToGetToGreen, o => o.Ignore())
                .ForMember(p => p.ForwardLook, o => o.Ignore())
                .ForMember(p => p.EmergingIssues, o => o.Ignore())
                .ForMember(p => p.ForecastSpend, o => o.Ignore())
                .ForMember(p => p.CostCentre, o => o.Ignore())
                .ForMember(p => p.AssuranceGateNumber, o => o.Ignore())
                .ForMember(p => p.NextAssuranceGateNumber, o => o.Ignore())


                // Ignore the keys
                .ForMember(p => p.ProjectReservation_Id, o => o.Ignore())
                .ForMember(p => p.ProjectCategory_Id, o => o.Ignore())
                .ForMember(p => p.ProjectSize_Id, o => o.Ignore())
                .ForMember(p => p.BudgetType_Id, o => o.Ignore())
                .ForMember(p => p.Lead_Id, o => o.Ignore())
                .ForMember(p => p.FirstUpdate_Id, o => o.Ignore())
                .ForMember(p => p.LatestUpdate_Id, o => o.Ignore())
            ;
        }

        private void project__ProjectUpdateItem()
        {
            CreateMap<serdproject, ProjectUpdateItem>()
                .ForMember(p => p.Id, o => o.Ignore())
                .ForMember(p => p.Project_Id, o => o.Ignore())
                .ForMember(p => p.Project, o => o.Ignore())
                .ForMember(p => p.Person, o => o.Ignore())
                .ForMember(p => p.SyncId, o => o.MapFrom(s => s.id))
                .ForMember(p => p.Timestamp, o => o.MapFrom(s => s.timestamp))
                .ForMember(p => p.Text, o => o.MapFrom(s => s.update))
                .ForMember(p => p.PercentageComplete, o => o.Ignore()) // TODO: gone
                .ForMember(p => p.RAGStatus, o => o.MapFrom<string>(new ConfigRAGStatusResolver(true), s => s.rag))
                .ForMember(p => p.Phase, o => o.MapFrom<string>(new ConfigPhaseStatusResolver(true), s => SyncMaps.phaseKeyMap["serd"][(s.phase ?? "com").Trim()]))
                .ForMember(p => p.OnHoldStatus, o => o.MapFrom<string>(new ConfigOnHoldStatusResolver(true), s => SyncMaps.onholdKeyMap[s.onhold ?? "n"]))
                .ForMember(p => p.Budget, o => o.MapFrom<DecimalResolver, string>(s => s.budget))
                .ForMember(p => p.Spent, o => o.MapFrom<DecimalResolver, string>(s => s.spent))
                .ForMember(p => p.ExpectedCurrentPhaseEnd, o => o.MapFrom<PostgresProjectDateResolver, string>(s => string.Empty)) // TODO: isn't one!?
                ;
        }

    }

    public class SubCategoryResolver : IMemberValueResolver<object, Project, string, ICollection<ProjectCategory>>
    {
        private Dictionary<string, string> categoryKeyMap;
        private ConfigCategoryResolver resolver;

        public SubCategoryResolver(Dictionary<string, string> categoryKeyMap)
        {
            this.categoryKeyMap = categoryKeyMap;
            this.resolver = new ConfigCategoryResolver();
        }

        public ICollection<ProjectCategory> Resolve(object source, Project destination, string sourceMember, ICollection<ProjectCategory> destMember, ResolutionContext context)
        {
            var result = destMember?.ToList() ?? new List<ProjectCategory>();
            if (!string.IsNullOrWhiteSpace(sourceMember))
            {
                var categoryKeys = sourceMember.Split(',').Select(s => categoryKeyMap[s.Trim()]);
                foreach (var key in categoryKeys)
                {
                    var cat = resolver.Resolve(source, destination, key, null, context);
                    if(cat != null && !result.Any(c => c.ViewKey == cat.ViewKey)) 
                        result.Add(cat);
                }
            }
            return result;
        }
    }
}