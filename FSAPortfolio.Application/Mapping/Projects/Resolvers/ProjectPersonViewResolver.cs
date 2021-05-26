using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects.Resolvers
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