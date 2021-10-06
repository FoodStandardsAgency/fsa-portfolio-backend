using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSAPortfolio.Application.Services.Index.Models;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects
{
    public class ProjectQueryModelProfile : Profile
    {
        public ProjectQueryModelProfile()
        {
            CreateMap<IEnumerable<ProjectQueryResultProjectModel>, ProjectQueryResultModel>().ConvertUsing<ProjectQueryTypeConverter>();

            CreateMap<Project, ProjectQueryResultProjectModel>()
                .ForMember(d => d.PortfolioViewKey, o => o.MapFrom(s => s.Reservation.Portfolio.ViewKey))
                .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority))
                .ForMember(d => d.PriorityGroup, o => o.MapFrom(s => s.PriorityGroup.Name))
                ;

            CreateMap<ProjectSearchIndexModel, ProjectQueryResultProjectModel>()
                .ForMember(d => d.PortfolioViewKey, o => o.MapFrom(s => s.PortfolioViewKey))
                .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.project_id))
                .ForMember(d => d.ProjectName, o => o.MapFrom(s => s.project_name))
                .ForMember(d => d.Priority, o => o.MapFrom(s => s.Priority))
                .ForMember(d => d.PriorityGroup, o => o.MapFrom(s => s.PriorityGroup))
                ;

        }
    }

    public class ProjectQueryTypeConverter : ITypeConverter<IEnumerable<ProjectQueryResultProjectModel>, ProjectQueryResultModel>
    {
        public ProjectQueryResultModel Convert(IEnumerable<ProjectQueryResultProjectModel> source, ProjectQueryResultModel destination, ResolutionContext context)
        {
            ProjectQueryResultModel result = new ProjectQueryResultModel()
            {
                Projects = context.Mapper.Map<IEnumerable<ProjectQueryResultProjectModel>>(source),
                ResultCount = source.Count()
            };
            return result;
        }
    }
}