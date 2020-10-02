using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.PostgreSQL.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Controllers;
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
            CreateMap<Project, latest_projects>()
                .ForMember(p => p.id, o => o.MapFrom(s => s.Id))
                .ForMember(p => p.project_id, o => o.MapFrom(s => s.ProjectId))
                .ForMember(p => p.project_name, o => o.MapFrom(s => s.Name))
                .ForMember(p => p.start_date, o => o.MapFrom(s => s.StartDate))
                .ForMember(p => p.short_desc, o => o.MapFrom(s => s.Description))
                .ForMember(p => p.phase, o => o.MapFrom(s => s.LatestUpdate.Phase.ViewKey))
                .ForMember(p => p.category, o => o.MapFrom(s => s.Category.ViewKey))
                .ForMember(p => p.subcat, o => o.Ignore())
                .ForMember(p => p.rag, o => o.MapFrom(s => s.LatestUpdate.RAGStatus.ViewKey))
                .ForMember(p => p.update, o => o.MapFrom(s => s.LatestUpdate.Text))
                .ForMember(p => p.oddlead, o => o.Ignore())
                .ForMember(p => p.oddlead_email, o => o.MapFrom(s => s.Lead.Email))
                .ForMember(p => p.servicelead, o => o.Ignore())
                .ForMember(p => p.servicelead_email, o => o.MapFrom(s => s.ServiceLead.Email))
                .ForMember(p => p.priority_main, o => o.MapFrom(s => s.Priority.ToString("D2")))
                .ForMember(p => p.funded, o => o.Ignore())
                .ForMember(p => p.confidence, o => o.Ignore())
                .ForMember(p => p.priorities, o => o.Ignore())
                .ForMember(p => p.benefits, o => o.Ignore())
                .ForMember(p => p.criticality, o => o.Ignore())
                .ForMember(p => p.budget, o => o.Ignore())
                .ForMember(p => p.spent, o => o.Ignore())
                .ForMember(p => p.documents, o => o.Ignore())
                .ForMember(p => p.timestamp, o => o.MapFrom(s => s.LatestUpdate.Timestamp))
                .ForMember(p => p.pgroup, o => o.Ignore())
                .ForMember(p => p.link, o => o.Ignore())
                .ForMember(p => p.toupdate, o => o.Ignore())
                .ForMember(p => p.rels, o => o.Ignore())
                .ForMember(p => p.team, o => o.Ignore())
                .ForMember(p => p.onhold, o => o.MapFrom(s => s.LatestUpdate.OnHoldStatus.Name))
                .ForMember(p => p.expend, o => o.Ignore())
                .ForMember(p => p.hardend, o => o.MapFrom(s => s.HardEndDate))
                .ForMember(p => p.actstart, o => o.MapFrom(s => s.ActualStartDate))
                .ForMember(p => p.dependencies, o => o.Ignore())
                .ForMember(p => p.project_size, o => o.Ignore())
                .ForMember(p => p.oddlead_role, o => o.Ignore())
                .ForMember(p => p.budgettype, o => o.Ignore())
                .ForMember(p => p.direct, o => o.Ignore())
                .ForMember(p => p.expendp, o => o.Ignore())
                .ForMember(p => p.p_comp, o => o.Ignore())
                .ForMember(p => p.max_time, o => o.MapFrom(s => s.LatestUpdate.Timestamp))
                .ForMember(p => p.min_time, o => o.MapFrom(s => s.FirstUpdate.Timestamp))
                .ForMember(p => p.g6team, o => o.MapFrom(s => s.Lead.G6team))
                .ForMember(p => p.new_flag, o => o.MapFrom(s => s.IsNew ? "Y" : "N"))
                ;

        }
    }
}