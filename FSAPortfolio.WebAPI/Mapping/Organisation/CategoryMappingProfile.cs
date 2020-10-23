using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Organisation
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Portfolio, PortfolioSummaryModel>()
                .ForMember(d => d.Categories, o => o.MapFrom(s => s.Configuration.Categories.OrderBy(c => c.Order)))
                .ForMember(d => d.Phases, o => o.MapFrom(s => s.Configuration.Phases.Where(p => p.Id != s.Configuration.CompletedPhase.Id).OrderBy(c => c.Order)))
                ;

            CreateMap<ProjectCategory, CategorySummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<ProjectsGroupedByPhaseResolver>())
                ;

            CreateMap<ProjectPhase, PhaseSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.Count, o => o.MapFrom<ProjectCountByPhaseResolver>())
                ;

            CreateMap<Project, ProjectIndexModel>()
                .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.IsNew, o => o.MapFrom(s => s.IsNew))
                ;

        }
    }

    public class ProjectsGroupedByPhaseResolver : IValueResolver<ProjectCategory, CategorySummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectCategory source, CategorySummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            var q = from ph in source.Configuration.Phases // Phases...
                    orderby ph.Order
                    where ph.Id != source.Configuration.CompletedPhase.Id // ...where phase not completed...
                    join pr in source.Configuration.Portfolio.Projects on ph.Id equals pr.LatestUpdate.Phase.Id into projects // ... get projects joined to each phase ...
                    from pr in projects.DefaultIfEmpty() // ... need to get all phases ...
                    where pr == null || pr.ProjectCategory_Id == source.Id
                    group pr by ph into phaseGroup // ... group projects by phase ...
                    select new PhaseProjectsModel() { 
                        ViewKey = phaseGroup.Key.ViewKey, 
                        Order = phaseGroup.Key.Order, 
                        Projects = context.Mapper.Map<IEnumerable<ProjectIndexModel>>(phaseGroup.Where(p => p != null)) 
                    };
            return q;
        }
    }

    public class ProjectCountByPhaseResolver : IValueResolver<ProjectPhase, PhaseSummaryModel, int>
    {
        public int Resolve(ProjectPhase source, PhaseSummaryModel destination, int destMember, ResolutionContext context)
        {
            return source.Configuration.Portfolio.Projects.Count(p => p.LatestUpdate.Phase.Id == source.Id);
        }
    }

}