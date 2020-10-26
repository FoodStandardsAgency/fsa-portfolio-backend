﻿using AutoMapper;
using AutoMapper.Configuration.Annotations;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Controllers;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Reflection;
using Newtonsoft.Json;

namespace FSAPortfolio.WebAPI.Mapping.Projects
{
    public class ProjectViewModelProfile : Profile
    {
        internal const string TimeOutputFormat = "dd/MM/yyyy hh:mm";
        private const string DateOutputFormat = "dd/MM/yyyy";

        public ProjectViewModelProfile()
        {
            CreateMap<DateTime, string>().ConvertUsing(d => d.ToString(DateOutputFormat));
            CreateMap<DateTime?, string>().ConvertUsing(d => d.HasValue ? d.Value.ToString(DateOutputFormat) : "00/00/00");

            // Outbound
            Project__ProjectViewModel();
            ProjectUpdateItem__UpdateHistoryModel();

        }


        private void Project__ProjectViewModel()
        {
            CreateMap<Project, ProjectViewModel>()
                .ForMember(p => p.id, o => o.MapFrom(s => s.LatestUpdate.SyncId))
                .ForMember(p => p.project_id, o => o.MapFrom(s => s.Reservation.ProjectId))
                .ForMember(p => p.project_name, o => o.MapFrom(s => s.Name))
                .ForMember(p => p.start_date, o => o.MapFrom(s => s.StartDate))
                .ForMember(p => p.short_desc, o => o.MapFrom(s => s.Description))
                .ForMember(p => p.phase, o => o.MapFrom(s => s.LatestUpdate.Phase.ViewKey))
                .ForMember(p => p.category, o => o.MapFrom(s => s.Category.ViewKey))
                .ForMember(p => p.subcat, o => o.MapFrom(s => s.Subcategories.Select(sc => sc.ViewKey).ToArray()))
                .ForMember(p => p.rag, o => o.MapFrom(s => s.LatestUpdate.RAGStatus.ViewKey))
                .ForMember(p => p.update, o => o.MapFrom(s => s.LatestUpdate.Timestamp.Date == DateTime.Today ? s.LatestUpdate.Text : null))
                .ForMember(p => p.UpdateHistory, o => o.MapFrom<UpdateHistoryResolver>())
                .ForMember(p => p.LastUpdate, o => o.MapFrom<LastUpdateResolver>())
                .ForMember(p => p.priority_main, o => o.MapFrom(s => s.Priority.HasValue ? s.Priority.Value.ToString("D2") : string.Empty))
                .ForMember(p => p.funded, o => o.MapFrom(s => s.Funded.ToString()))
                .ForMember(p => p.confidence, o => o.MapFrom(s => s.Confidence.ToString()))
                .ForMember(p => p.priorities, o => o.MapFrom(s => s.Priorities.ToString()))
                .ForMember(p => p.benefits, o => o.MapFrom(s => s.Benefits.ToString()))
                .ForMember(p => p.criticality, o => o.MapFrom(s => s.Criticality.ToString()))
                .ForMember(p => p.budget, o => o.MapFrom(s => Convert.ToInt32(s.LatestUpdate.Budget)))
                .ForMember(p => p.spent, o => o.MapFrom(s => Convert.ToInt32(s.LatestUpdate.Spent)))
                .ForMember(p => p.timestamp, o => o.MapFrom(s => s.LatestUpdate.Timestamp))

                .ForMember(p => p.rels, o => o.MapFrom(s => s.RelatedProjects.Select(rp => new RelatedProjectModel() { ProjectId = rp.Reservation.ProjectId, Name = rp.Name })))
                .ForMember(p => p.dependencies, o => o.MapFrom(s => string.Join(", ", s.DependantProjects.Select(rp => rp.Reservation.ProjectId))))
                .ForMember(p => p.team, o => o.MapFrom(s => s.Team))
                .ForMember(p => p.onhold, o => o.MapFrom(s => s.LatestUpdate.OnHoldStatus.Name))
                .ForMember(p => p.expend, o => o.MapFrom(s => s.ExpectedEndDate))
                .ForMember(p => p.hardend, o => o.MapFrom(s => s.HardEndDate))
                .ForMember(p => p.actstart, o => o.MapFrom(s => s.ActualStartDate))
                .ForMember(p => p.actual_end_date, o => o.MapFrom(s => s.ActualEndDate))
                .ForMember(p => p.project_size, o => o.MapFrom(s => s.Size.ViewKey))
                .ForMember(p => p.budgettype, o => o.MapFrom(s => s.BudgetType.ViewKey))
                .ForMember(p => p.direct, o => o.MapFrom(s => s.Directorate))
                .ForMember(p => p.expendp, o => o.MapFrom(s => s.LatestUpdate.ExpectedCurrentPhaseEnd))
                .ForMember(p => p.p_comp, o => o.MapFrom(s => s.LatestUpdate.PercentageComplete))
                .ForMember(p => p.max_time, o => o.MapFrom(s => s.LatestUpdate.Timestamp))
                .ForMember(p => p.min_time, o => o.MapFrom(s => s.FirstUpdate.Timestamp))
                .ForMember(p => p.g6team, o => o.MapFrom(s => s.Lead.G6team))
                .ForMember(p => p.new_flag, o => o.MapFrom(s => s.IsNew ? "Y" : "N"))
                .ForMember(p => p.first_completed, o => o.MapFrom<FirstCompletedResolver, Project>(s => s))

                .ForMember(p => p.oddlead, o => o.Ignore()) // TODO: add a field for the lead name
                .ForMember(p => p.oddlead_email, o => o.MapFrom(s => s.Lead.Email))
                .ForMember(p => p.servicelead, o => o.Ignore()) // TODO: add a field for the lead name
                .ForMember(p => p.servicelead_email, o => o.MapFrom(s => s.ServiceLead.Email))

                .ForMember(p => p.documents, o => o.MapFrom(s => s.Documents.OrderBy(d => d.Order))) // TODO: add a field for this

                .ForMember(p => p.pgroup, o => o.Ignore()) // TODO: add a field for this
                .ForMember(p => p.link, o => o.Ignore()) // TODO: add a field for this

                .ForMember(p => p.oddlead_role, o => o.Ignore()) // TODO: add a field for this

                .ForMember(p => p.milestones, o => o.Ignore())

                // Below this line are project data items
                .ForMember(p => p.business_case_number, o => o.Ignore())
                .ForMember(p => p.fs_number, o => o.Ignore())
                .ForMember(p => p.risk_rating, o => o.Ignore())
                .ForMember(p => p.theme, o => o.Ignore())
                .ForMember(p => p.project_type, o => o.Ignore())
                .ForMember(p => p.strategic_objectives, o => o.Ignore())
                .ForMember(p => p.programme, o => o.Ignore())
                .ForMember(p => p.programme_description, o => o.Ignore())
                .ForMember(p => p.key_contact1, o => o.Ignore())
                .ForMember(p => p.key_contact2, o => o.Ignore())
                .ForMember(p => p.key_contact3, o => o.Ignore())
                .ForMember(p => p.supplier, o => o.Ignore())
                .ForMember(p => p.how_get_green, o => o.Ignore())
                .ForMember(p => p.forward_look, o => o.Ignore())
                .ForMember(p => p.emerging_issues, o => o.Ignore())
                .ForMember(p => p.forecast_spend, o => o.Ignore())
                .ForMember(p => p.budget_field1, o => o.Ignore())
                .ForMember(p => p.cost_centre, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_gatenumber, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_gatecompleted, o => o.Ignore())
                .ForMember(p => p.fsaproc_assurance_nextgate, o => o.Ignore())
                .AfterMap(ProjectDataOutboundMapper.Map)
                ;

            CreateMap<Document, LinkModel>()
                .ForMember(d => d.name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.link, o => o.MapFrom(s => s.Link))
                ;
        }

        private void ProjectUpdateItem__UpdateHistoryModel()
        {
            CreateMap<ProjectUpdateItem, UpdateHistoryModel>()
                .ForMember(d => d.Text, o => o.MapFrom(s => s.Text))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp))
                ;
        }


    }

    public class FirstCompletedResolver : IMemberValueResolver<Project, ProjectModel, Project, DateTime?>
    {
        public DateTime? Resolve(Project source, ProjectModel destination, Project sourceMember, DateTime? destMember, ResolutionContext context)
        {
            var completedPhase = source.Reservation.Portfolio.Configuration.CompletedPhase;
            var firstCompletePhase = source.Updates.Where(u => u.Phase == completedPhase).OrderBy(u => u.Timestamp).FirstOrDefault();
            return firstCompletePhase?.Timestamp;
        }
    }

    public class UpdateHistoryResolver : IValueResolver<Project, ProjectModel, UpdateHistoryModel[]>
    {
        public UpdateHistoryModel[] Resolve(Project source, ProjectModel destination, UpdateHistoryModel[] destMember, ResolutionContext context)
        {
            UpdateHistoryModel[] result = null;
            object includeHistory;
            if (context.Items.TryGetValue(nameof(ProjectViewModel.UpdateHistory), out includeHistory) && (includeHistory as bool? ?? false))
            {
                // History is updates except the latest if it was added today.
                //result = context.Mapper.Map<UpdateHistoryModel[]>(source.Updates.Where(u => !(u.Timestamp.Date == DateTime.Today && u.Id == source.LatestUpdate_Id)).OrderBy(u => u.Timestamp));

                // History is all updates 
                result = context.Mapper.Map<UpdateHistoryModel[]>(source.Updates.Where(u => !string.IsNullOrWhiteSpace(u.Text)).OrderBy(u => u.Timestamp));
            }
            return result;
        }
    }

    public class LastUpdateResolver : IValueResolver<Project, ProjectModel, UpdateHistoryModel>
    {
        public UpdateHistoryModel Resolve(Project source, ProjectModel destination, UpdateHistoryModel destMember, ResolutionContext context)
        {
            UpdateHistoryModel result = null;
            object includeHistory;
            if (context.Items.TryGetValue(nameof(ProjectViewModel.UpdateHistory), out includeHistory) && (includeHistory as bool? ?? false))
            {
                result = context.Mapper.Map<UpdateHistoryModel>(source.Updates.Where(u => u.Timestamp.Date != DateTime.Today).OrderBy(u => u.Timestamp).FirstOrDefault());
            }
            return result;
        }
    }

}