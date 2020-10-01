using AutoMapper;
using FSAPortfolio.Entites.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, ProjectModel>();
        }
    }
}