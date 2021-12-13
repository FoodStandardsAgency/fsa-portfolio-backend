using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Mapping.Projects.Resolvers
{
    public class ProjectPersonViewResolver : IMemberValueResolver<Project, object, Person, string>
    {
        public string Resolve(Project source, object destination, Person sourceMember, string destMember, ResolutionContext context)
        {
            string result = sourceMember?.DisplayName;
            return result;
        }
    }
}