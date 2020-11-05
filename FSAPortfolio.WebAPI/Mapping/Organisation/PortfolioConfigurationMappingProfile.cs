﻿using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Mapping.Organisation.Resolvers;
using FSAPortfolio.WebAPI.Models;

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
                .ForMember(d => d.FieldOptions, o => o.MapFrom(s => s.FieldOptions))
                .ForMember(d => d.Flags, o => o.MapFrom(s => s.Flags))
                ;

            CreateMap<PortfolioConfiguration, PortfolioConfigModel>()
                .ForMember(d => d.Labels, o => o.MapFrom(s => s.Labels))
                ;


            CreateMap<PortfolioLabelConfig, PortfolioLabelModel>()
                .ForMember(d => d.FieldName, o => o.MapFrom(s => s.FieldName))
                .ForMember(d => d.FieldGroup, o => o.MapFrom(s => s.Group == null ? FieldGroupConstants.FieldGroupName_Ungrouped : s.Group.Name))
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
                .ForMember(d => d.FieldOptions, o => o.MapFrom(s => s.InputValue)) // Just map the raw value here and do the collections in controller/provider
                ;
        }

    }
}