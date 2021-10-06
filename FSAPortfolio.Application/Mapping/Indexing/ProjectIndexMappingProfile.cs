using AutoMapper;
using FSAPortfolio.Application.Services.Index.Models;
using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Mapping.Indexing
{
    public class ProjectIndexMappingProfile : Profile
    {
        public ProjectIndexMappingProfile()
        {
            CreateMap<Project, ProjectSearchIndexModel>()
                .ForMember(d => d.project_id, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(p => p.project_name, o => o.MapFrom(s => s.Name))
                .ForMember(p => p.short_desc, o => o.MapFrom(s => s.Description))
                .ForMember(p => p.rag, o => o.MapFrom(s => s.LatestUpdate.RAGStatus.Name))
                ;
        }
    }
}
