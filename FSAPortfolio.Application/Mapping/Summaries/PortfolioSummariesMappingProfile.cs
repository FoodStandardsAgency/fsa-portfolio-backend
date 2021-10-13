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
using FSAPortfolio.WebAPI.App.Mapping.Projects;
using FSAPortfolio.Entities;

namespace FSAPortfolio.WebAPI.App.Mapping.Organisation
{
    public class PortfolioSummariesMappingProfile : Profile
    {
        public PortfolioSummariesMappingProfile()
        {
            CreateMap<Portfolio, PortfolioSummaryModel>()
                .ForMember(d => d.Person, o => o.MapFrom<PortfolioPersonResolver>())
                .ForMember(d => d.Summaries, o => o.MapFrom<PortfolioSummaryResolver>())
                .ForMember(d => d.Phases, o =>
                {
                    o.PreCondition((s, d, c) => { return c.Items.ContainsKey(PortfolioSummaryResolver.SummaryTypeKey); });
                    o.MapFrom(s => s.Configuration.Phases.Where(p => p.Id != s.Configuration.CompletedPhase.Id).OrderBy(c => c.Order));
                })
                .ForMember(d => d.Labels, o => o.MapFrom<ProjectSummaryLabelResolver>())
                .ForMember(d => d.ProjectTypes, o => o.MapFrom<ProjectSummaryProjectTypeResolver>())
                ;

            // Summary type mappings
            CreateMap<ProjectCategory, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByCategoryResolver>())
                .ForMember(d => d.Actions, o => o.MapFrom(s => false))
                ;

            CreateMap<ProjectUserCategory, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByUserCategoryResolver>())
                .ForMember(d => d.Actions, o => o.MapFrom(s => s.CategoryType == ProjectUserCategoryType.Lead))
                ;

            CreateMap<PriorityGroup, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByPriorityGroupResolver>())
                .ForMember(d => d.Actions, o => o.MapFrom(s => false))
                ;

            CreateMap<ProjectRAGStatus, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByRAGResolver>())
                .ForMember(d => d.Actions, o => o.MapFrom(s => false))
                ;

            CreateMap<ProjectPhase, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByPhaseResolver>())
                .ForMember(d => d.Actions, o => o.MapFrom(s => false))
                ;

            CreateMap<Team, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByTeamResolver>())
                .ForMember(d => d.Actions, o => o.MapFrom(s => false))
                ;

            CreateMap<Person, ProjectSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.DisplayName ?? ProjectTeamConstants.NotSetName))
                .ForMember(d => d.Order, o => o.MapFrom(s => 0))
                .ForMember(d => d.PhaseProjects, o => o.MapFrom<PhaseProjectsByTeamLeadResolver>())
                .ForMember(d => d.Actions, o => o.MapFrom(s => false))
                ;


            // Phase mappings
            CreateMap<ProjectPhase, PhaseSummaryModel>()
                .ForMember(d => d.ViewKey, o => o.MapFrom(s => s.ViewKey))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Order, o => o.MapFrom(s => s.Order))
                .ForMember(d => d.Count, o => o.MapFrom<ProjectCountByPhaseResolver>())
                ;

            CreateMap<ProjectDate, ProjectDateViewModel>().ConvertUsing<ProjectDateConverter>();
            CreateMap<Project, ProjectIndexModel>()
                .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(d => d.Name, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Name) ? s.Reservation.ProjectId : s.Name))
                .ForMember(d => d.IsNew, o => o.MapFrom(s => s.IsNew ? "Y" : "N"))
                .ForMember(d => d.Actions, o => o.MapFrom<ProjectActionsResolver>())
                .ForMember(d => d.Deadline, o => o.MapFrom<ProjectIndexDateResolver>())
                .ForMember(d => d.Priority, o => o.MapFrom<ProjectIndexPriorityResolver>())
                ;

        }
    }

}