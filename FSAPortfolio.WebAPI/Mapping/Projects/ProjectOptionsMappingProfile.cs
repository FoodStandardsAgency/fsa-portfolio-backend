using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects
{
    public class ProjectOptionsMappingProfile : Profile
    {
        public ProjectOptionsMappingProfile()
        {
            PortfolioConfiguration_ProjectLabelConfigModel();

            CreateMap<PortfolioConfiguration, ProjectOptionsModel>()
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
                .ForMember(d => d.RiskRating, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.risk_rating))))
                .ForMember(d => d.Theme, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.theme))))
                .ForMember(d => d.ProjectType, o => o.MapFrom(new LabelDropDownResolver(nameof(ProjectModel.project_type))))
                .ForMember(d => d.Programme, o => o.MapFrom(new SelectPickerResolver(nameof(ProjectModel.programme), "Select the programmes...")))
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
                .ForMember(d => d.FieldGroup, o => o.MapFrom(s => s.Group == null ? DefaultFieldLabels.FieldGroupName_Ungrouped : s.Group.Name))
                .ForMember(d => d.GroupOrder, o => o.MapFrom(s => s.Group.Order))
                .ForMember(d => d.FieldOrder, o => o.MapFrom(s => s.FieldOrder))
                .ForMember(d => d.FieldTitle, o => o.MapFrom(s => s.FieldTitle))
                .ForMember(d => d.Included, o => o.MapFrom(s => s.Included && (s.MasterLabel == null || s.MasterLabel.Included)))
                .ForMember(d => d.AdminOnly, o => o.MapFrom(s => s.AdminOnly))
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
            var flags = (PortfolioFieldFlags)context.Items[nameof(PortfolioFieldFlags)];
            return context.Mapper.Map<ICollection<ProjectLabelModel>>(sourceMember.Where(s => (s.Flags & flags) != 0))
                                 .OrderBy(l => l.GroupOrder)
                                 .ThenBy(l => l.FieldOrder)
                                 .ToList();
        }
    }

    public class LabelDropDownResolver : IValueResolver<PortfolioConfiguration, ProjectOptionsModel, IEnumerable<DropDownItemModel>>
    {
        private string fieldName;

        public LabelDropDownResolver(string fieldName)
        {
            this.fieldName = fieldName;
        }

        public IEnumerable<DropDownItemModel> Resolve(PortfolioConfiguration source, ProjectOptionsModel destination, IEnumerable<DropDownItemModel> destMember, ResolutionContext context)
        {
            IEnumerable<DropDownItemModel> items = null;
            var label = source.Labels.SingleOrDefault(l => l.FieldName == fieldName);
            if(label != null)
            {
                items = label.FieldOptions.Split(',').Select((l, i ) => new DropDownItemModel() { Display = l, Value = l, Order = i });
            }
            return items;
        }
    }

    public class SelectPickerResolver : IValueResolver<PortfolioConfiguration, ProjectOptionsModel, SelectPickerModel>
    {
        private string fieldName;
        private string header;

        public SelectPickerResolver(string fieldName, string header)
        {
            this.fieldName = fieldName;
            this.header = header;
        }

        public SelectPickerModel Resolve(PortfolioConfiguration source, ProjectOptionsModel destination, SelectPickerModel destMember, ResolutionContext context)
        {
            SelectPickerModel model = null;
            var label = source.Labels.SingleOrDefault(l => l.FieldName == fieldName);
            if (label != null)
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

}