using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Organisation
{
    public class PortfolioMappingProfile : Profile
    {
        public PortfolioMappingProfile()
        {
            CreateMap<Portfolio, PortfolioModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ShortName, o => o.MapFrom(s => s.ShortName))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                ;

            CreateMap<NewPortfolioModel, Portfolio>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ShortName, o => o.MapFrom(s => s.ShortName))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForAllOtherMembers(o => o.Ignore())
                ;
        }
    }
}