using AutoMapper;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.ActiveDirectory
{
    public class ADUserMappingProfile : Profile
    {
        public ADUserMappingProfile()
        {
            CreateMap<MicrosoftGraphUserListResponse, UserSearchResponseModel>()
                .ForMember(d => d.SearchResults, o => o.MapFrom(s => s.value))
                ;

            CreateMap<MicrosoftGraphUserModel, UserSearchModel>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.displayName))
                .ForMember(d => d.GivenName, o => o.MapFrom(s => s.givenName))
                .ForMember(d => d.Surname, o => o.MapFrom(s => s.surname))
                .ForMember(d => d.UserPrincipalName, o => o.MapFrom(s => s.userPrincipalName))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                ;
        }
    }
}