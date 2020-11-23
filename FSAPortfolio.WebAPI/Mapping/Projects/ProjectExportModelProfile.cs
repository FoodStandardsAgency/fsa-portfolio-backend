using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.Mapping.Projects.Resolvers;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Linq;

namespace FSAPortfolio.WebAPI.Mapping.Projects
{
    public class ProjectExportModelProfile : Profile
    {
        public const string FullDateExportFormat = "dd/MM/yyyy";
        public const string MonthDateExportFormat = "MM/yyyy";
        public const string YearDateExportFormat = "yyyy";
        public ProjectExportModelProfile()
        {
            CreateMap<string, string>().ConvertUsing(s => s != null ? s.Replace(",", "") : null);
            CreateMap<ProjectDate, string>().ConvertUsing<ProjectExportDateConverter>();

            // Outbound
            Project__ProjectExportModel();
        }

        private void Project__ProjectExportModel()
        {
            CreateMap<Project, ProjectExportModel>()
                .ForMember(p => p.project_id, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(p => p.project_name, o => o.MapFrom(s => s.Name))
                .ForMember(p => p.start_date, o => o.MapFrom(s => s.StartDate))
                .ForMember(p => p.short_desc, o => o.MapFrom(s => s.Description))
                .ForMember(p => p.category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(p => p.subcat, o => o.MapFrom(s => string.Join(", ", s.Subcategories.Select(sc => sc.Name))))

                .ForMember(p => p.priority_main, o => o.MapFrom(s => s.Priority.HasValue ? s.Priority.Value.ToString("D2") : string.Empty))
                .ForMember(p => p.funded, o => o.MapFrom(s => s.Funded))
                .ForMember(p => p.confidence, o => o.MapFrom(s => s.Confidence))
                .ForMember(p => p.priorities, o => o.MapFrom(s => s.Priorities))
                .ForMember(p => p.benefits, o => o.MapFrom(s => s.Benefits))
                .ForMember(p => p.criticality, o => o.MapFrom(s => s.Criticality))

                .ForMember(p => p.expend, o => o.MapFrom(s => s.ExpectedEndDate))
                .ForMember(p => p.hardend, o => o.MapFrom(s => s.HardEndDate))
                .ForMember(p => p.actstart, o => o.MapFrom(s => s.ActualStartDate))
                .ForMember(p => p.actual_end_date, o => o.MapFrom(s => s.ActualEndDate))
                .ForMember(p => p.fsaproc_assurance_gatecompleted, o => o.MapFrom(s => s.AssuranceGateCompletedDate))
                .ForMember(p => p.project_size, o => o.MapFrom(s => s.Size.Name))
                .ForMember(p => p.budgettype, o => o.MapFrom(s => s.BudgetType.Name))
                .ForMember(p => p.direct, o => o.MapFrom(s => s.Directorate.Name))

                .ForMember(p => p.theme, o => o.MapFrom(s => s.Theme))
                .ForMember(p => p.project_type, o => o.MapFrom(s => s.ProjectType))
                .ForMember(p => p.strategic_objectives, o => o.MapFrom(s => s.StrategicObjectives))
                .ForMember(p => p.programme, o => o.MapFrom(s => s.Programme))
                .ForMember(p => p.supplier, o => o.MapFrom(s => s.Supplier))

                .ForMember(p => p.g6team, o => o.MapFrom(s => s.Lead.Team.Name))
                .ForMember(p => p.new_flag, o => o.MapFrom(s => s.IsNew ? "Y" : "N"))
                .ForMember(p => p.first_completed, o => o.MapFrom<FirstCompletedResolver>())
                .ForMember(p => p.pgroup, o => o.MapFrom(s => s.PriorityGroup.Name))

                .ForMember(p => p.project_type, o => o.MapFrom(s => s.ProjectType))
                .ForMember(p => p.strategic_objectives, o => o.MapFrom(s => s.StrategicObjectives))
                .ForMember(p => p.programme, o => o.MapFrom(s => s.Programme))
                .ForMember(p => p.theme, o => o.MapFrom(s => s.Theme))
                .ForMember(p => p.documents, o => o.MapFrom(s => string.Join(", ", s.Documents.OrderBy(d => d.Order).Select(d => d.ExportText))))

                .ForMember(p => p.key_contact1, o => o.MapFrom(s => s.KeyContact1.DisplayName))
                .ForMember(p => p.key_contact2, o => o.MapFrom(s => s.KeyContact2.DisplayName))
                .ForMember(p => p.key_contact3, o => o.MapFrom(s => s.KeyContact3.DisplayName))
                .ForMember(p => p.supplier, o => o.MapFrom(s => s.Supplier))
                .ForMember(p => p.link, o => o.MapFrom(s => s.ChannelLink))
                .ForMember(p => p.oddlead, o => o.MapFrom<ProjectPersonViewResolver, Person>(s => s.Lead))
                .ForMember(p => p.oddlead_role, o => o.MapFrom(s => s.LeadRole))
                .ForMember(p => p.servicelead, o => o.MapFrom<ProjectPersonViewResolver, Person>(s => s.ServiceLead))

                // TODO: add persistence and mappings for outstanding fields
                // Outstanding
                .ForMember(p => p.milestones, o => o.Ignore())

                // Latest update and update history
                .ForMember(p => p.phase, o => o.MapFrom(s => s.LatestUpdate.Phase.Name))
                .ForMember(p => p.rag, o => o.MapFrom(s => s.LatestUpdate.RAGStatus.Name))
                .ForMember(p => p.onhold, o => o.MapFrom(s => s.LatestUpdate.OnHoldStatus.Name))
                //.ForMember(p => p.update, o => o.MapFrom<ExportUpdateTextResolver>())
                .ForMember(p => p.update, o => o.Ignore())
                .ForMember(p => p.budget, o => o.MapFrom(s => Convert.ToInt32(s.LatestUpdate.Budget)))
                .ForMember(p => p.spent, o => o.MapFrom(s => Convert.ToInt32(s.LatestUpdate.Spent)))
                .ForMember(p => p.expendp, o => o.MapFrom(s => s.LatestUpdate.ExpectedCurrentPhaseEnd))
                .ForMember(p => p.p_comp, o => o.MapFrom(s => s.LatestUpdate.PercentageComplete))
                .ForMember(p => p.timestamp, o => o.MapFrom(s => s.LatestUpdate.Timestamp))

                // Below this line are project data items
                .ForMember(p => p.business_case_number, o => o.Ignore())
                .ForMember(p => p.fs_number, o => o.Ignore())
                .ForMember(p => p.risk_rating, o => o.Ignore())
                .ForMember(p => p.programme_description, o => o.Ignore())
                .ForMember(p => p.how_get_green, o => o.Ignore())
                .ForMember(p => p.forward_look, o => o.Ignore())
                .ForMember(p => p.emerging_issues, o => o.Ignore())
                .ForMember(p => p.forecast_spend, o => o.Ignore())
                .ForMember(p => p.budget_field1, o => o.Ignore())
                .ForMember(p => p.cost_centre, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_gatenumber, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_gatecompleted, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_nextgate, o => o.Ignore())

                .ForMember(d => d.Properties, o => o.Ignore())

                // TODO: need these after maps if want project data in export
                //.AfterMap<ProjectDataOutboundMapper<ProjectExportModel>>()
                //.AfterMap<ProjectJsonPropertiesOutboundMapper>()
                ;

            CreateMap<ProjectLink, string>().ConvertUsing(s => $"{s.Name}, {s.Link}");

        }

    }
    public class ExportUpdateTextResolver : IValueResolver<Project, ProjectExportModel, string>
    {
        public string Resolve(Project source, ProjectExportModel destination, string destMember, ResolutionContext context)
        {
            string result = null;
            var lastTextUpdate = source.Updates.Where(u => !string.IsNullOrWhiteSpace(u.Text)).OrderBy(u => u.Timestamp).FirstOrDefault();
            result = lastTextUpdate?.Text;
            return result;
        }
    }

    public class ProjectExportDateConverter : ITypeConverter<ProjectDate, string>
    {
        public string Convert(ProjectDate source, string destination, ResolutionContext context)
        {
            string result = string.Empty;
            if (source.Date.HasValue)
            {
                if (source.Flags.HasFlag(ProjectDateFlags.Day)) result = source.Date.Value.ToString(ProjectExportModelProfile.FullDateExportFormat);
                else if (source.Flags.HasFlag(ProjectDateFlags.Month)) result = source.Date.Value.ToString(ProjectExportModelProfile.MonthDateExportFormat);
                else if (source.Flags.HasFlag(ProjectDateFlags.Year)) result = source.Date.Value.ToString(ProjectExportModelProfile.YearDateExportFormat);
            }
            return result;
        }
    }


}