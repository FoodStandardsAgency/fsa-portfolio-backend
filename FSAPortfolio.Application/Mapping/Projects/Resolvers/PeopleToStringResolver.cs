using AutoMapper;
using FSAPortfolio.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Mapping.Projects.Resolvers
{
    public class PeopleToStringResolver : IMemberValueResolver<object, object, ICollection<Person>, string>
    {
        public string Resolve(object source, object destination, ICollection<Person> sourceMember, string destMember, ResolutionContext context)
        {
            string result = null;
            if (sourceMember != null)
            {
                result = string.Join("|", sourceMember.Select(s => context.Mapper.Map<string>(s)));
            }
            return result;
        }
    }
}