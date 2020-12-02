using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Organisation
{
    /// <summary>
    /// This controls which fields can be updated for a given object - hence the mappings are from type to the same type.
    /// </summary>
    public class PortfolioConfigUpdateProfile : Profile
    {
        public PortfolioConfigUpdateProfile()
        {
            CreateMap<PortfolioLabelConfig, PortfolioLabelConfig>()
                .ForMember(d => d.Included, o => o.MapFrom(s => s.Included))
                .ForMember(d => d.AdminOnly, o => o.MapFrom(s => s.AdminOnly))
                .ForMember(d => d.Label, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Label) ? null : s.Label))
                .ForMember(d => d.FieldType, o => o.MapFrom(s => s.FieldType))
                .ForMember(d => d.FieldOptions, o => o.MapFrom(s => s.FieldOptions))
                .ForAllOtherMembers(o => o.Ignore())
                ;
        }
    }
}