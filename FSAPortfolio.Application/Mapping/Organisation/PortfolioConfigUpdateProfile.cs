using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Mapping.Organisation
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
                .ForMember(d => d.Flags, o => o.MapFrom<FlagUpdateResolver, PortfolioFieldFlags>(s => s.Flags))
                .ForAllOtherMembers(o => o.Ignore())
                ;
        }

        public class FlagUpdateResolver : IMemberValueResolver<PortfolioLabelConfig, PortfolioLabelConfig, PortfolioFieldFlags, PortfolioFieldFlags>
        {
            public PortfolioFieldFlags Resolve(PortfolioLabelConfig source, PortfolioLabelConfig destination, PortfolioFieldFlags sourceMember, PortfolioFieldFlags destMember, ResolutionContext context)
            {
                return destMember.HasFlag(PortfolioFieldFlags.Filterable) ? (destMember & ~PortfolioFieldFlags.FilterProject) | (sourceMember & PortfolioFieldFlags.FilterProject) : destMember;
            }
        }
    }
}