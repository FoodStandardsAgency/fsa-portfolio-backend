using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Mapping.Organisation.Resolvers.Summaries;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Organisation
{
    public class PortfolioSummariesMappingProfile : Profile
    {
        public PortfolioSummariesMappingProfile()
        {
            CreateMap<Portfolio, PortfolioSummaryModel>()
                .ForMember(d => d.Person, o => o.MapFrom<PortfolioPersonResolver>())
                .ForMember(d => d.Summaries, o => o.MapFrom<PortfolioSummaryResolver>())
                .ForMember(d => d.Phases, o => o.MapFrom(s => s.Configuration.Phases.Where(p => p.Id != s.Configuration.CompletedPhase.Id).OrderBy(c => c.Order)))
                .ForMember(d => d.Labels, o => o.MapFrom<ProjectSummaryLabelResolver>())
                .ForMember(d => d.ProjectTypes, o => o.MapFrom<ProjectSummaryProjectTypeResolver>())
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

            CreateMap<Person, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.DisplayName ?? ProjectTeamConstants.NotSetName))
                .ForMember(d => d.Order, o => o.MapFrom(s => 0))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByTeamLeadResolver>())
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
                .ForMember(d => d.IsNew, o => o.MapFrom(s => s.IsNew ? "Y" : "N"))
                ;

        }
    }
}