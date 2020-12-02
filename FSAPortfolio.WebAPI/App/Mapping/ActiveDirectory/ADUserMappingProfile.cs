using AutoMapper;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.ActiveDirectory
{
    public class ADUserMappingProfile : Profile
    {
        public ADUserMappingProfile()
        {
            CreateMap<MicrosoftGraphUserListResponse, UserSearchResponseModel>()
                .ForMember(d => d.SearchResults, o => o.MapFrom<UserSearchResponseModelResolver, List<MicrosoftGraphUserModel>>(s => s.value))
                ;

            CreateMap<MicrosoftGraphUserModel, UserSearchModel>()
                .ForMember(d => d.Department, o => o.MapFrom(s => s.department))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.displayName))
                .ForMember(d => d.GivenName, o => o.MapFrom(s => s.givenName))
                .ForMember(d => d.Surname, o => o.MapFrom(s => s.surname))
                .ForMember(d => d.UserPrincipalName, o => o.MapFrom(s => s.userPrincipalName))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                ;

            CreateMap<MicrosoftGraphUserModel, Person>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Email, o => o.MapFrom(s => s.mail ?? s.userPrincipalName))
                .ForMember(d => d.Firstname, o => o.MapFrom(s => s.givenName))
                .ForMember(d => d.Surname, o => o.MapFrom(s => s.surname))
                .ForMember(d => d.ActiveDirectoryDisplayName, o => o.MapFrom(s => s.displayName))
                .ForMember(d => d.Department, o => o.MapFrom(s => s.department))
                .ForMember(d => d.ActiveDirectoryPrincipalName, o => o.MapFrom(s => s.userPrincipalName))
                .ForMember(d => d.ActiveDirectoryId, o => o.MapFrom(s => s.id))
                ;
        }

        public class UserSearchResponseModelResolver : IMemberValueResolver<MicrosoftGraphUserListResponse, UserSearchResponseModel, List<MicrosoftGraphUserModel>, IEnumerable<UserSearchModel>>
        {
            public IEnumerable<UserSearchModel> Resolve(MicrosoftGraphUserListResponse source, UserSearchResponseModel destination, List<MicrosoftGraphUserModel> sourceMember, IEnumerable<UserSearchModel> destMember, ResolutionContext context)
            {
                var results = context.Mapper.Map<List<UserSearchModel>>(sourceMember);
                var key = nameof(ActiveDirectoryUserSelectModel.NoneOption);
                if(context.Items.ContainsKey(key) && (bool)context.Items[key])
                {
                    results.Insert(0, new UserSearchModel() { DisplayName = "None" });
                }
                return results;
            }
        }
    }
}