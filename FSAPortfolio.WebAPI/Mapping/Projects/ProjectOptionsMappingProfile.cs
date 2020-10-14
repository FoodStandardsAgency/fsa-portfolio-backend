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
                ;

            CreateMap<IProjectOption, DropDownItemModel>()
                .ForMember(d => d.Display, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Value, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                ;
        }

        private void PortfolioConfiguration_ProjectLabelConfigModel()
        {
            CreateMap<PortfolioConfiguration, ProjectLabelConfigModel>()
                .ForMember(d => d.Labels, o => o.MapFrom(s => s.Labels.OrderBy(l => l.Group.Order).ThenBy(l => l.FieldOrder)))
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
}