using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.App.Mapping.Projects.Resolvers;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects
{
    public class ProjectEditOptionsMappingProfile : Profile
    {
        public ProjectEditOptionsMappingProfile()
        {
            PortfolioConfiguration_ProjectLabelConfigModel();

            CreateMap<PortfolioConfiguration, ProjectEditOptionsModel>()
                .ForMember(d => d.PhaseItems, o => o.MapFrom(config => config.Phases.OrderBy(p => p.Order)))
                .ForMember(d => d.RAGStatusItems, o => o.MapFrom(
                    new ProjectOptionDropDownResolver<ProjectRAGStatus>(),
                    config => config.RAGStatuses.OrderBy(p => p.Order))
                )
                .ForMember(d => d.OnHoldStatusItems, o => o.MapFrom(config => config.OnHoldStatuses.OrderBy(p => p.Order)))
                .ForMember(d => d.ProjectSizeItems, o => o.MapFrom(config => config.ProjectSizes.OrderBy(p => p.Order)))
                .ForMember(d => d.BudgetTypeItems, o => o.MapFrom(config => config.BudgetTypes.OrderBy(p => p.Order)))
                .ForMember(d => d.CategoryItems, o => o.MapFrom(config => config.Categories.OrderBy(p => p.Order)))
                .ForMember(d => d.SubCategoryItems, o => o.MapFrom(config => config.Categories.OrderBy(p => p.Order)))
                .ForMember(d => d.Directorates, o => o.Ignore()) // This comes from the context - set by the PortfolioProvider.
                .ForMember(d => d.G6Team, o => o.Ignore()) // This comes from the context - set by the PortfolioProvider.
                .ForMember(d => d.RiskRating, o => o.MapFrom(new LabelDropDownResolver(ProjectPropertyConstants.risk_rating)))
                .ForMember(d => d.Theme, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.theme))))
                .ForMember(d => d.ProjectType, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.project_type))))
                .ForMember(d => d.Programme, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.programme))))
                .ForMember(d => d.ODDLeadRole, o => o.MapFrom(new LabelDropDownResolver(ProjectPropertyConstants.oddlead_role, addNoneOption: true)))

                .ForMember(d => d.ODDLead, o => o.MapFrom(s => new ActiveDirectoryUserSelectModel() { NoneOption = false }))
                .ForMember(d => d.PriorityItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.priority_main))))
                .ForMember(d => d.PriorityGroupItems, o => o.MapFrom(new PriorityGroupLabelDropDownResolver(nameof(ProjectModel.pgroup))))
                .ForMember(d => d.FundedItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.funded))))
                .ForMember(d => d.ConfidenceItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.confidence))))
                .ForMember(d => d.PrioritiesItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.priorities))))
                .ForMember(d => d.BenefitItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.benefits))))
                .ForMember(d => d.CriticalityItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.criticality))))

                .ForMember(d => d.KeyContact1, o => o.MapFrom(s => new ActiveDirectoryUserSelectModel() { NoneOption = true }))
                .ForMember(d => d.KeyContact2, o => o.MapFrom(s => new ActiveDirectoryUserSelectModel() { NoneOption = true }))
                .ForMember(d => d.KeyContact3, o => o.MapFrom(s => new ActiveDirectoryUserSelectModel() { NoneOption = true }))


                .ForMember(d => d.project_team_option1, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.project_team_option1))))
                .ForMember(d => d.project_team_option2, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.project_team_option2))))

                .ForMember(d => d.project_plan_option1, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.project_plan_option1))))
                .ForMember(d => d.project_plan_option2, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.project_plan_option2))))

                .ForMember(d => d.progress_option1, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.progress_option1))))
                .ForMember(d => d.progress_option2, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.progress_option2))))

                .ForMember(d => d.budget_option1, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.budget_option1))))
                .ForMember(d => d.budget_option2, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.budget_option2))))

                .ForMember(d => d.processes_option1, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.processes_option1))))
                .ForMember(d => d.processes_option2, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.processes_option2))))

                .ForMember(d => d.StrategicObjectives, o => o.Ignore())

                .ForMember(d => d.ProjectDataOptions, o => o.Ignore())
                .AfterMap<ProjectDataOptionsOutboundMapper>()
                ;


            CreateMap<IProjectOption, DropDownItemModel>()
                .Include<Directorate, DropDownItemModel>()
                .Include<Team, DropDownItemModel>()
                .ForMember(d => d.Display, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                ;

            CreateMap<Directorate, DropDownItemModel>();
            CreateMap<Team, DropDownItemModel>();

            CreateMap<IEnumerable<ProjectCategory>, SelectPickerModel>()
                .ConvertUsing(s => new SelectPickerModel()
                {
                    Header = "Select the subcategories...",
                    Items = s.Select(c => new SelectPickerItemModel() { Display = c.Name, Value = c.ViewKey, Order = c.Order }).ToList()
                });
        }

        private void PortfolioConfiguration_ProjectLabelConfigModel()
        {
            CreateMap<PortfolioConfiguration, ProjectLabelConfigModel>()
                .ForMember(d => d.Labels, o => o.MapFrom<ConfigLabelFlagResolver, ICollection<PortfolioLabelConfig>>(s => s.Labels))
                .AfterMap((source, dest, context) => {
                    // TODO: revisit this - can still see the values in the mark up!
                    // If an FSAOnly mapping has been requested, put the input types for FSAOnly labels to None.
                    if (context.Items.ContainsKey(nameof(PortfolioFieldFlags.FSAOnly)) && (bool)context.Items[nameof(PortfolioFieldFlags.FSAOnly)])
                    {
                        foreach (var label in dest.Labels)
                        {
                            if (label.FSAOnly) label.Included = false;
                        }
                    }
                })
                ;

            CreateMap<PortfolioLabelConfig, ProjectLabelModel>()
                .ForMember(d => d.FieldName, o => o.MapFrom(s => s.FieldName))
                .ForMember(d => d.FieldGroup, o => o.MapFrom(s => s.Group == null ? FieldGroupConstants.FieldGroupName_Ungrouped : s.Group.Name))
                .ForMember(d => d.GroupOrder, o => o.MapFrom(s => s.Group.Order))
                .ForMember(d => d.FieldOrder, o => o.MapFrom(s => s.FieldOrder))
                .ForMember(d => d.FieldTitle, o => o.MapFrom(s => s.FieldTitle))
                .ForMember(d => d.Included, o => o.MapFrom(s => s.Included && (s.MasterLabel == null || s.MasterLabel.Included)))
                .ForMember(d => d.AdminOnly, o => o.MapFrom(s => s.AdminOnly))
                .ForMember(d => d.EditorCanView, o => o.MapFrom(s => s.Flags.HasFlag(PortfolioFieldFlags.EditorCanView)))
                .ForMember(d => d.Label, o => o.MapFrom(s => s.Label == null ? s.FieldTitle : s.Label))
                .ForMember(d => d.FieldType, o => o.MapFrom(s => s.FieldType.ToString().ToLower()))
                .ForMember(d => d.InputValue, o => o.Ignore()) // This is set separately as the value can come from anywhere
                .ForMember(d => d.FSAOnly, o => o.MapFrom(s => s.Flags.HasFlag(PortfolioFieldFlags.FSAOnly)))
                .ForMember(d => d.Required, o => o.MapFrom(s => s.Flags.HasFlag(PortfolioFieldFlags.Required)))
                ;

        }
    }

    /// <summary>
    /// Maps labels to models, with filtering on flags and included setting. Can also include custom labels.
    /// </summary>
    /// <remarks>
    /// Set the flags, included options and custom labels in the mapping context. Labels are filtered on flags, included setting and results are merged with custom labels.
    /// </remarks>
    public class ConfigLabelFlagResolver : IMemberValueResolver<PortfolioConfiguration, ProjectLabelConfigModel, ICollection<PortfolioLabelConfig>, IEnumerable<ProjectLabelModel>>
    {
        public IEnumerable<ProjectLabelModel> Resolve(PortfolioConfiguration source, ProjectLabelConfigModel destination,
                                                      ICollection<PortfolioLabelConfig> sourceMember,
                                                      IEnumerable<ProjectLabelModel> destMember,
                                                      ResolutionContext context)
        {
            IEnumerable<PortfolioLabelConfig> labels = sourceMember;

            // Get the settings from the context.
            var flagsKey = nameof(PortfolioFieldFlags);
            var includedOnlyKey = nameof(PortfolioLabelConfig.Included);
            var customLabelKey = nameof(PortfolioLabelConfig);

            // Filter
            if (context.Items.ContainsKey(flagsKey))
            {
                var flags = (PortfolioFieldFlags)context.Items[flagsKey];
                labels = labels.Where(s => (s.Flags & flags) != 0);
            }

            if (context.Items.ContainsKey(includedOnlyKey))
            {
                var includedOnly = (bool)context.Items[includedOnlyKey];
                if(includedOnly) labels = labels.Where(s => s.Included);
            }

            // Add custom labels
            if(context.Items.ContainsKey(customLabelKey))
            {
                IEnumerable<PortfolioLabelConfig> customLabels = (IEnumerable<PortfolioLabelConfig>)context.Items[customLabelKey];
                labels = labels.Union(customLabels);
            }

            // Map to the models
            var models = context.Mapper.Map<ICollection<ProjectLabelModel>>(labels)
                                 .OrderBy(l => l.GroupOrder)
                                 .ThenBy(l => l.FieldOrder)
                                 .ToList();
            return models;
        }
    }

    public class LabelDropDownResolver : IValueResolver<PortfolioConfiguration, ProjectEditOptionsModel, List<DropDownItemModel>>
    {
        private string fieldName;
        private bool emptyOption = false;

        public LabelDropDownResolver(string fieldName, bool addNoneOption = false)
        {
            this.fieldName = fieldName;
            this.emptyOption = addNoneOption;
        }

        public List<DropDownItemModel> Resolve(PortfolioConfiguration source, ProjectEditOptionsModel destination, List<DropDownItemModel> destMember, ResolutionContext context)
        {
            List<DropDownItemModel> items = null;
            var label = source.Labels.SingleOrDefault(l => l.FieldName == fieldName);
            if(!string.IsNullOrEmpty(label?.FieldOptions))
            {
                items = label.FieldOptions.Split(',').Select((l, i ) =>
                {
                    var value = l.Trim();
                    var display = value;
                    return NewDropDownItem(i + 1, value, display);
                })
                    .ToList();
                if (emptyOption)
                    items.Insert(0, NewDropDownItem(1, null, "None"));
            }
            return items;
        }

        public static DropDownItemModel NewDropDownItem(int order, string value, string display)
        {
            return new DropDownItemModel() { Display = display, Value = value, Order = order };
        }
    }

    public class PriorityGroupLabelDropDownResolver : IValueResolver<PortfolioConfiguration, ProjectEditOptionsModel, List<DropDownItemModel>>
    {
        private string fieldName;

        public PriorityGroupLabelDropDownResolver(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public List<DropDownItemModel> Resolve(PortfolioConfiguration source, ProjectEditOptionsModel destination, List<DropDownItemModel> destMember, ResolutionContext context)
        {
            return source.PriorityGroups.Select(pg => new DropDownItemModel() { Display = pg.Name, Value = pg.ViewKey, Order = pg.Order }).ToList();
        }
    }

    public class SelectPickerResolver : IValueResolver<PortfolioConfiguration, ProjectEditOptionsModel, SelectPickerModel>
    {
        private string fieldName;
        private string header;

        public SelectPickerResolver(string fieldName, string header)
        {
            this.fieldName = fieldName;
            this.header = header;
        }

        public SelectPickerModel Resolve(PortfolioConfiguration source, ProjectEditOptionsModel destination, SelectPickerModel destMember, ResolutionContext context)
        {
            SelectPickerModel model = null;
            var label = source.Labels.SingleOrDefault(l => l.FieldName == fieldName);
            if (label?.FieldOptions != null)
            {
                var items = label.FieldOptions.Split(',').Select((l, i) => NewSelectPickerItemModel(l, l, i));

                model = new SelectPickerModel()
                {
                    Header = header,
                    Items = items.ToList()
                };
            }
            return model;
        }

        public static SelectPickerItemModel NewSelectPickerItemModel(string display, string value, int order)
        {
            return new SelectPickerItemModel() { Display = display, Value = value, Order = order };
        }
    }

    public class ProjectOptionDropDownResolver<TOption> : IMemberValueResolver<PortfolioConfiguration, ProjectEditOptionsModel, IEnumerable<TOption>, List<DropDownItemModel>>
        where TOption : IProjectOption, new()
    {
        private string fieldName;
        private bool emptyOption = false;

        public ProjectOptionDropDownResolver(string fieldName = null, bool addNoneOption = false)
        {
            this.fieldName = fieldName;
            this.emptyOption = addNoneOption;
        }

        public List<DropDownItemModel> Resolve(PortfolioConfiguration source, ProjectEditOptionsModel destination, IEnumerable<TOption> sourceMember, List<DropDownItemModel> destMember, ResolutionContext context)
        {
            List<DropDownItemModel> items = null;
            if (sourceMember != null)
            {
                items = context.Mapper.Map<List<DropDownItemModel>>(sourceMember);
                if (emptyOption) items.Insert(0, new DropDownItemModel() { Display = "None", Value = null });
            }
            return items;
        }
    }
}