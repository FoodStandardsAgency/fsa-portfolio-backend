using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects.Resolvers
{
    public class ProjectPersonViewResolver : IMemberValueResolver<Project, ProjectModel, Person, string>
    {
        public string Resolve(Project source, ProjectModel destination, Person sourceMember, string destMember, ResolutionContext context)
        {
            string result = sourceMember?.DisplayName;
            return result;
        }
    }
}