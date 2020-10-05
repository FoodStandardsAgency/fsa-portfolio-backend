using AutoMapper;
using AutoMapper.Configuration.Annotations;
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
    public class PortfolioConfigurationMappingProfile : Profile
    {

        public PortfolioConfigurationMappingProfile()
        {
            CreateMap<PortfolioConfigAddLabelRequest, PortfolioLabelConfig>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Configuration_Id, o => o.Ignore())
                .ForMember(d => d.Configuration, o => o.Ignore())
                .ForMember(d => d.FieldName, o => o.MapFrom(s => s.FieldName))
                .ForMember(d => d.Included, o => o.MapFrom(s => s.Included))
                .ForMember(d => d.Label, o => o.MapFrom(s => s.FieldLabel))
                ;

            CreateMap<PortfolioConfiguration, PortfolioConfigModel>()
                .ForMember(d => d.Labels, o => o.MapFrom(s => s.Labels))
                ;


            CreateMap<PortfolioLabelConfig, PortfolioLabelModel>()
                .ForMember(d => d.FieldName, o => o.MapFrom(s => s.FieldName))
                .ForMember(d => d.Included, o => o.MapFrom(s => s.Included))
                .ForMember(d => d.Label, o => o.MapFrom(s => s.Label))
                ;
        }

    }
}