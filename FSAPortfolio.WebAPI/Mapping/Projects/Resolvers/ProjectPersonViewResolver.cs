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
            string result = null;
            if(sourceMember != null)
            {
                if (!string.IsNullOrWhiteSpace(sourceMember.Firstname) && !string.IsNullOrWhiteSpace(sourceMember.Surname))
                {
                    result = $"{sourceMember.Firstname} {sourceMember.Surname}";
                }
                else if (!string.IsNullOrWhiteSpace(sourceMember.Email))
                {
                    result = sourceMember.Email;
                }
                else if (!string.IsNullOrWhiteSpace(sourceMember.ActiveDirectoryPrincipleName))
                {
                    result = sourceMember.ActiveDirectoryPrincipleName;
                }
            }
            return result;
        }
    }
}