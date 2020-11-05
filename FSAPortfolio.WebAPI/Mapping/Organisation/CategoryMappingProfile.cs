using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Mapping.Organisation.Resolvers.Summaries;
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
                .ForMember(d => d.Summaries, o => o.MapFrom<PortfolioSummaryResolver>())
                .ForMember(d => d.Phases, o => o.MapFrom(s => s.Configuration.Phases.Where(p => p.Id != s.Configuration.CompletedPhase.Id).OrderBy(c => c.Order)))
                ;

            // Summary type mappings
            CreateMap<ProjectCategory, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByCategoryResolver>())
                ;

            CreateMap<PriorityGroup, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByPriorityGroupResolver>())
                ;

            CreateMap<ProjectRAGStatus, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByRAGResolver>())
                ;

            CreateMap<ProjectPhase, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByPhaseResolver>())
                ;

            CreateMap<Team, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByTeamResolver>())
                ;


            // Phase mappings
            CreateMap<ProjectPhase, PhaseSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.Count, o => o.MapFrom<ProjectCountByPhaseResolver>())
                ;

            CreateMap<Project, ProjectIndexModel>()
                .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(d => d.Name, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Name) ? s.Reservation.ProjectId : s.Name))
                .ForMember(d => d.IsNew, o => o.MapFrom(s => s.IsNew))
                ;

        }
    }



    public class ProjectCountByPhaseResolver : IValueResolver<ProjectPhase, PhaseSummaryModel, int>
    {
        public int Resolve(ProjectPhase source, PhaseSummaryModel destination, int destMember, ResolutionContext context)
        {
            return source.Configuration.Portfolio.Projects.Count(p => p.LatestUpdate?.Phase?.Id == source.Id);
        }
    }

}