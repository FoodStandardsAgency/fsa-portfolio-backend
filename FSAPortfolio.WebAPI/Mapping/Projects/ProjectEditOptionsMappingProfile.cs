using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Mapping.Projects.Resolvers;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects
{
    public class ProjectEditOptionsMappingProfile : Profile
    {
        public ProjectEditOptionsMappingProfile()
        {
            PortfolioConfiguration_ProjectLabelConfigModel();

            CreateMap<PortfolioConfiguration, ProjectEditOptionsModel>()
                .ForMember(d => d.PhaseItems, o => o.MapFrom(config => config.Phases.OrderBy(p => p.Order)))
                .ForMember(d => d.RAGStatusItems, o => o.MapFrom(config => config.RAGStatuses.OrderBy(p => p.Order)))
                .ForMember(d => d.OnHoldStatusItems, o => o.MapFrom(config => config.OnHoldStatuses.OrderBy(p => p.Order)))
                .ForMember(d => d.ProjectSizeItems, o => o.MapFrom(config => config.ProjectSizes.OrderBy(p => p.Order)))
                .ForMember(d => d.BudgetTypeItems, o => o.MapFrom(config => config.BudgetTypes.OrderBy(p => p.Order)))
                .ForMember(d => d.CategoryItems, o => o.MapFrom(config => config.Categories.OrderBy(p => p.Order)))
                .ForMember(d => d.SubCategoryItems, o => o.MapFrom(config => config.Categories.OrderBy(p => p.Order)))
                .ForMember(d => d.Directorates, o => o.Ignore()) // This comes from the context - set by the PortfolioProvider.
                .ForMember(d => d.RelatedProjects, o => o.Ignore()) // This comes from the context - set by the PortfolioProvider.
                .ForMember(d => d.DependantProjects, o => o.Ignore()) // This comes from the context - set by the PortfolioProvider.
                .ForMember(d => d.G6Team, o => o.Ignore()) // This comes from the context - set by the PortfolioProvider.
                .ForMember(d => d.RiskRating, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.risk_rating))))
                .ForMember(d => d.Theme, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.theme))))
                .ForMember(d => d.ProjectType, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.project_type))))
                .ForMember(d => d.Programme, o => o.MapFrom(new SelectPickerResolver(nameof(ProjectModel.programme), "Select the programmes...")))

                .ForMember(d => d.ODDLead, o => o.MapFrom(new StubPersonResolver(ProjectPropertyConstants.ProjectLead, addNoneOption: false))) // TODO: do we need these options if using ajax?
                .ForMember(d => d.ODDLeadRole, o => o.MapFrom(new StubRoleResolver(nameof(ProjectModel.oddlead_role))))
                .ForMember(d => d.PriorityItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.priority_main))))
                .ForMember(d => d.FundedItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.funded))))
                .ForMember(d => d.ConfidenceItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.confidence))))
                .ForMember(d => d.PrioritiesItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.priorities))))
                .ForMember(d => d.BenefitItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.benefits))))
                .ForMember(d => d.CriticalityItems, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.criticality))))
                .ForMember(d => d.ProjectDataOptions, o => o.Ignore())
                .AfterMap<ProjectDataOptionsOutboundMapper>()
                ;


            CreateMap<IProjectOption, DropDownItemModel>()
                .ForMember(d => d.Display, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                ;

            CreateMap<IEnumerable<ProjectCategory>, SelectPickerModel>()
                .ConvertUsing(s => new SelectPickerModel()
                {
                    Header = "Select the subcategories...",
                    Items = s.Select(c => new SelectPickerItemModel() { Display = c.Name, Value = c.ViewKey, Order = c.Order }).ToArray()
                });
        }

        private void PortfolioConfiguration_ProjectLabelConfigModel()
        {
            CreateMap<PortfolioConfiguration, ProjectLabelConfigModel>()
                .ForMember(d => d.Labels, o => o.MapFrom<ConfigLabelFlagResolver, ICollection<PortfolioLabelConfig>>(s => s.Labels))
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
                ;

        }
    }

    /// <summary>
    /// Maps labels filtering on flags passed into the mapping options: only labels with one of the flags set are returned.
    /// </summary>
    public class ConfigLabelFlagResolver : IMemberValueResolver<PortfolioConfiguration, ProjectLabelConfigModel, ICollection<PortfolioLabelConfig>, IEnumerable<ProjectLabelModel>>
    {
        public IEnumerable<ProjectLabelModel> Resolve(PortfolioConfiguration source, ProjectLabelConfigModel destination,
                                                      ICollection<PortfolioLabelConfig> sourceMember,
                                                      IEnumerable<ProjectLabelModel> destMember,
                                                      ResolutionContext context)
        {
            IEnumerable<PortfolioLabelConfig> labels = sourceMember;
            var flagsKey = nameof(PortfolioFieldFlags);
            var includedOnlyKey = nameof(PortfolioLabelConfig.Included);
            var customLabelKey = nameof(PortfolioLabelConfig);

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

            if(context.Items.ContainsKey(customLabelKey))
            {
                IEnumerable<PortfolioLabelConfig> customLabels = (IEnumerable<PortfolioLabelConfig>)context.Items[customLabelKey];
                labels = labels.Union(customLabels);
            }

            var models = context.Mapper.Map<ICollection<ProjectLabelModel>>(labels)
                                 .OrderBy(l => l.GroupOrder)
                                 .ThenBy(l => l.FieldOrder)
                                 .ToList();
            return models;
        }
    }

    public class LabelDropDownResolver : IValueResolver<PortfolioConfiguration, ProjectEditOptionsModel, IEnumerable<DropDownItemModel>>
    {
        private string fieldName;

        public LabelDropDownResolver(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public IEnumerable<DropDownItemModel> Resolve(PortfolioConfiguration source, ProjectEditOptionsModel destination, IEnumerable<DropDownItemModel> destMember, ResolutionContext context)
        {
            IEnumerable<DropDownItemModel> items = null;
            var label = source.Labels.SingleOrDefault(l => l.FieldName == fieldName);
            if(label?.FieldOptions != null)
            {
                items = label.FieldOptions.Split(',').Select((l, i ) => {
                    var value = l.Trim();
                    var display = value;
                    return new DropDownItemModel() { Display = display, Value = value, Order = i };
                    });
            }
            return items;
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
                var items = label.FieldOptions.Split(',').Select((l, i) => new SelectPickerItemModel() { Display = l, Value = l, Order = i });

                model = new SelectPickerModel()
                {
                    Header = header,
                    Items = items
                };
            }
            return model;
        }
    }

    // TODO: implement resolver using AD integration
    public class StubPersonResolver : IValueResolver<PortfolioConfiguration, ProjectEditOptionsModel, SelectPickerModel>
    {
        private string fieldName;
        private bool addNoneOption;

        public StubPersonResolver(string fieldName, bool addNoneOption = true)
        {
            this.fieldName = fieldName;
            this.addNoneOption = addNoneOption;
        }

        public SelectPickerModel Resolve(PortfolioConfiguration source, ProjectEditOptionsModel destination, SelectPickerModel destMember, ResolutionContext context)
        {
            SelectPickerModel model = addNoneOption ? new SelectPickerModel()
            {
                Header = "Select the person...",
                Items = new SelectPickerItemModel[] {
                    new SelectPickerItemModel() { Display = "None", Order = 0 }
                }
            } :
            new SelectPickerModel()
            {
                Header = "Select the person...",
                Items = new SelectPickerItemModel[] {
                }
            };
            return model;
        }
    }

    // TODO: implement resolver using AD integration
    public class StubRoleResolver : IValueResolver<PortfolioConfiguration, ProjectEditOptionsModel, SelectPickerModel>
    {
        private string fieldName;

        public StubRoleResolver(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public SelectPickerModel Resolve(PortfolioConfiguration source, ProjectEditOptionsModel destination, SelectPickerModel destMember, ResolutionContext context)
        {
            SelectPickerModel model = new SelectPickerModel()
            {
                Header = "Select the role...",
                Items = new SelectPickerItemModel[] {
                    new SelectPickerItemModel() { Display = "None", Order = 0 },
                    new SelectPickerItemModel() { Display = "Role0", Value = "r0id", SearchTokens="Role0", Order = 1 },
                    new SelectPickerItemModel() { Display = "Role1", Value = "r1id", SearchTokens="Role1", Order = 2 },
                    new SelectPickerItemModel() { Display = "Role2", Value = "r2id", SearchTokens="Role2", Order = 3 },
                    new SelectPickerItemModel() { Display = "Role3", Value = "r3id", SearchTokens="Role3", Order = 4 },
                }
            };
            return model;
        }
    }

    // TODO: implement resolver using AD integration
    public class StubTeamResolver : IValueResolver<PortfolioConfiguration, ProjectEditOptionsModel, SelectPickerModel>
    {
        private string fieldName;

        public StubTeamResolver(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public SelectPickerModel Resolve(PortfolioConfiguration source, ProjectEditOptionsModel destination, SelectPickerModel destMember, ResolutionContext context)
        {
            // TODO: get the teams from ActiveDirectory
            SelectPickerModel model = model = new SelectPickerModel()
            {
                Header = "Select the team...",
                Items = new SelectPickerItemModel[] {
                    new SelectPickerItemModel() { Display = "None", Order = 0 },
                    new SelectPickerItemModel() { Display = "Team0 (t0@a.b.com)", Value = "t0id", SearchTokens="Team0, t0@a.b.com", Order = 1 },
                    new SelectPickerItemModel() { Display = "Team1 (t1@a.b.com)", Value = "t1id", SearchTokens="Team1, t1@a.b.com", Order = 2 },
                    new SelectPickerItemModel() { Display = "Team2 (t2@a.b.com)", Value = "t2id", SearchTokens="Team2, t2@a.b.com", Order = 3 },
                    new SelectPickerItemModel() { Display = "Team3 (t3@a.b.com)", Value = "t3id", SearchTokens="Team3, t3@a.b.com", Order = 4 },
                }
            };
            return model;
        }
    }
}