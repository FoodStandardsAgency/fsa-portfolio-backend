using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Projects
{
    public class ProjectQueryModelProfile : Profile
    {
        public ProjectQueryModelProfile()
        {
            CreateMap<Project[], ProjectQueryResultModel>().ConvertUsing<ProjectQueryTypeConverter>();

            CreateMap<Project, ProjectQueryResultProjectModel>()
                .ForMember(s => s.PortfolioViewKey, o => o.MapFrom(d => d.Reservation.Portfolio.ViewKey))
                .ForMember(s => s.ProjectId, o => o.MapFrom(d => d.Reservation.ProjectId))
                .ForMember(s => s.ProjectName, o => o.MapFrom(d => d.Name))
                .ForMember(s => s.Priority, o => o.MapFrom(d => d.Priority))
                ;

        }
    }

    public class ProjectQueryTypeConverter : ITypeConverter<Project[], ProjectQueryResultModel>
    {
        public ProjectQueryResultModel Convert(Project[] source, ProjectQueryResultModel destination, ResolutionContext context)
        {
            ProjectQueryResultModel result = new ProjectQueryResultModel()
            {
                Projects = context.Mapper.Map<IEnumerable<ProjectQueryResultProjectModel>>(source),
                ResultCount = source.Length
            };
            return result;
        }
    }
}