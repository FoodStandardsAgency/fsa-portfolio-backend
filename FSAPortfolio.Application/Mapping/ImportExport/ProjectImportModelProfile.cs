using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.ImportExport
{
    public class ProjectImportModelProfile : Profile
    {
        public ProjectImportModelProfile()
        {
            CreateMap<string, string[]>().ConvertUsing(s => string.IsNullOrWhiteSpace(s) ? default(string[]) : s.Split(',', ';', '|'));
            CreateMap<string, float?>().ConvertUsing<ProjectImportFloatConverter>();
            CreateMap<string, float>().ConvertUsing(s => float.Parse(s));
            CreateMap<string, ProjectDateEditModel>().ConvertUsing<ProjectImportDateConverter>();
            CreateMap<string, ProjectPersonModel>().ConvertUsing(s => new ProjectPersonModel() { Value = s, Email = s });
            CreateMap<string, LinkModel>().ConvertUsing<ProjectLinkImportResolver>();
            CreateMap<string, LinkModel[]>().ConvertUsing<ProjectLinkCollectionImportResolver>();

        }
    }
}