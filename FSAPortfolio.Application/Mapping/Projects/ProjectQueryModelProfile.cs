using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Mapping.Projects
{
    public class ProjectQueryModelProfile : Profile
    {
        public ProjectQueryModelProfile()
        {
            CreateMap<Project[], ProjectQueryResultModel>().ConvertUsing<ProjectQueryTypeConverter>();

            CreateMap<Project, ProjectQueryResultProjectModel>()
                .ForMember(d => d.PortfolioViewKey, o => o.MapFrom(s => s.Reservation.Portfolio.ViewKey))
                .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority))
                .ForMember(d => d.PriorityGroup, o => o.MapFrom(s => s.PriorityGroup.Name))
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