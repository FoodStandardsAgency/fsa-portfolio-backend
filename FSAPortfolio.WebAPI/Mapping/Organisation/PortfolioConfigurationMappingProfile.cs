using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace FSAPortfolio.WebAPI.Mapping.Organisation
{
    public class PortfolioConfigurationMappingProfile : Profile
    {

        public PortfolioConfigurationMappingProfile()
        {
            CreateMap<PortfolioConfigAddLabelRequest, PortfolioLabelConfig>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Configuration_Id, o => o.Ignore())
                .ForMember(d => d.Configuration, o => o.Ignore())
                .ForMember(d => d.Group, o => o.Ignore())
                .ForMember(d => d.MasterLabel, o => o.Ignore())
                .ForMember(d => d.MasterLabel_Id, o => o.Ignore())
                .ForMember(d => d.FieldName, o => o.MapFrom(s => s.FieldName))
                .ForMember(d => d.FieldOrder, o => o.MapFrom(s => s.FieldOrder))
                .ForMember(d => d.FieldTitle, o => o.MapFrom(s => s.FieldTitle))
                .ForMember(d => d.Included, o => o.MapFrom(s => s.Included))
                .ForMember(d => d.AdminOnly, o => o.MapFrom(s => s.AdminOnly))
                .ForMember(d => d.IncludedLock, o => o.MapFrom(s => s.IncludedLock))
                .ForMember(d => d.AdminOnlyLock, o => o.MapFrom(s => s.AdminOnlyLock))
                .ForMember(d => d.Label, o => o.MapFrom(s => s.FieldLabel))
                .ForMember(d => d.FieldType, o => o.MapFrom(s => s.FieldType))
                .ForMember(d => d.FieldTypeLocked, o => o.MapFrom(s => s.FieldTypeLocked))
                .ForMember(d => d.Flags, o => o.MapFrom(s => s.Flags))
                ;

            CreateMap<PortfolioConfiguration, PortfolioConfigModel>()
                .ForMember(d => d.Labels, o => o.MapFrom(s => s.Labels))
                ;


            CreateMap<PortfolioLabelConfig, PortfolioLabelModel>()
                .ForMember(d => d.FieldName, o => o.MapFrom(s => s.FieldName))
                .ForMember(d => d.FieldGroup, o => o.MapFrom(s => s.Group == null ? DefaultFieldLabels.FieldGroupName_Ungrouped : s.Group.Name))
                .ForMember(d => d.GroupOrder, o => o.MapFrom(s => s.Group.Order))
                .ForMember(d => d.FieldOrder, o => o.MapFrom(s => s.FieldOrder))
                .ForMember(d => d.FieldTitle, o => o.MapFrom(s => s.FieldTitle))
                .ForMember(d => d.Included, o => o.MapFrom(s => s.Included))
                .ForMember(d => d.AdminOnly, o => o.MapFrom(s => s.AdminOnly))
                .ForMember(d => d.IncludedLock, o => o.MapFrom(s => s.IncludedLock))
                .ForMember(d => d.AdminOnlyLock, o => o.MapFrom(s => s.AdminOnlyLock))
                .ForMember(d => d.Label, o => o.MapFrom(s => s.Label))
                .ForMember(d => d.FieldType, o => o.MapFrom(s => s.FieldType.ToString().ToLower()))
                .ForMember(d => d.FieldTypeDescription, o => o.MapFrom(s => PortfolioFieldTypeDescriptions.Map[s.FieldType]))
                .ForMember(d => d.FieldTypeLocked, o => o.MapFrom(s => s.FieldTypeLocked))
                .ForMember(d => d.InputValue, o => o.MapFrom<OutboundLabelInputValueResolver>())
                .ForMember(d => d.MasterField, o => o.MapFrom(s => s.MasterLabel == null ? null : s.MasterLabel.FieldName))
                .ReverseMap()
                .ForMember(d => d.MasterLabel, o => o.Ignore())
                .ForMember(d => d.FieldOptions, o => o.MapFrom<InboundLabelInputValueResolver>())
                ;
        }

    }

    public class OutboundLabelInputValueResolver : IValueResolver<PortfolioLabelConfig, PortfolioLabelModel, string>
    {
        public string Resolve(PortfolioLabelConfig source, PortfolioLabelModel destination, string destMember, ResolutionContext context)
        {
            string result = null;
            switch(source.FieldName)
            {
                case nameof(ProjectModel.category):
                    result = GetValue(source.Configuration.Categories);
                    break;
                case nameof(ProjectModel.onhold):
                    result = GetValue(source.Configuration.OnHoldStatuses);
                    break;
                case nameof(ProjectModel.phase):
                    result = GetValue(source.Configuration.Phases);
                    break;
                case nameof(ProjectModel.rag):
                    result = GetValue(source.Configuration.RAGStatuses);
                    break;
                case nameof(ProjectModel.project_size):
                    result = GetValue(source.Configuration.ProjectSizes);
                    break;
                case nameof(ProjectModel.budgettype):
                    result = GetValue(source.Configuration.BudgetTypes);
                    break;
                default:
                    result = source.FieldOptions;
                    break;
            }
            return result;
        }

        private string GetValue<T>(ICollection<T> collection) where T : IProjectOption
        {
            return string.Join(", ", collection.OrderBy(o => o.Order).Select(o => o.Name));
        }

    }

    public class InboundLabelInputValueResolver : IValueResolver<PortfolioLabelModel, PortfolioLabelConfig, string>
    {
        public string Resolve(PortfolioLabelModel source, PortfolioLabelConfig destination, string destMember, ResolutionContext context)
        {
            string result = null;
            switch (destination.FieldName)
            {
                case nameof(ProjectModel.category):
                case nameof(ProjectModel.onhold):
                case nameof(ProjectModel.phase):
                case nameof(ProjectModel.rag):
                case nameof(ProjectModel.project_size):
                case nameof(ProjectModel.budgettype):
//                    result = GetValue(destination.Configuration.BudgetTypes);
                    // TODO: set the appropiate collections
                    break;
                default:
                    result = source.InputValue;
                    break;
            }
            return result;
        }
    }
}