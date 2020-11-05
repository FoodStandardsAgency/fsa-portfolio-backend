﻿using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Mapping.Organisation.Resolvers.Summaries
{
    public class PortfolioSummaryResolver : IValueResolver<Portfolio, PortfolioSummaryModel, IEnumerable<ProjectSummaryModel>>
    {
        public IEnumerable<ProjectSummaryModel> Resolve(Portfolio source, PortfolioSummaryModel destination, IEnumerable<ProjectSummaryModel> destMember, ResolutionContext context)
        {
            IEnumerable<ProjectSummaryModel> result;
            var summaryType = context.Items[nameof(PortfolioSummaryModel)] as string;
            switch (summaryType)
            {
                case PortfolioSummaryModel.ByCategory:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.Categories.OrderBy(c => c.Order));
                    break;
                case PortfolioSummaryModel.ByPriorityGroup:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.PriorityGroups.OrderBy(c => c.Order));
                    break;
                case PortfolioSummaryModel.ByRagStatus:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.RAGStatuses.OrderBy(c => c.Order));
                    break;
                case PortfolioSummaryModel.ByOnHoldStatus:
                    result = context.Mapper.Map<IEnumerable<ProjectSummaryModel>>(source.Configuration.OnHoldStatuses.OrderBy(c => c.Order));
                    break;
                default:
                    throw new ArgumentException($"Unrecognised summary type: {summaryType}");
            }
            return result;
        }
    }

    internal static class SummaryLinqQuery
    {
        public static IEnumerable<PhaseProjectsModel> GetQuery(PortfolioConfiguration config, Func<Project, bool> projectPredicate, ResolutionContext context)
        {
            var q = from ph in config.Phases.Where(p => p.Id != config.CompletedPhase.Id) // Non-completed phases...
                    join pr in config.Portfolio.Projects
                        .Where(p => p.LatestUpdate?.Phase != null)
                        .Where(projectPredicate)
                        on ph.Id equals pr.LatestUpdate.Phase.Id into projects // ... get projects joined to each phase ...
                    from pr in projects.DefaultIfEmpty() // ... need to get all phases ...
                    group pr by ph into phaseGroup // ... group projects by phase ...
                    select new PhaseProjectsModel()
                    {
                        ViewKey = phaseGroup.Key.ViewKey,
                        Order = phaseGroup.Key.Order,
                        Projects = context.Mapper.Map<IEnumerable<ProjectIndexModel>>(phaseGroup.Where(p => p != null))
                    };
            return q.OrderBy(p => p.Order);
        }
    }

    public class PhaseProjectsByPriorityGroupResolver : IValueResolver<PriorityGroup, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(PriorityGroup source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.PriorityGroup.ViewKey == source.ViewKey, context);
        }
    }

    public class PhaseProjectsByCategoryResolver : IValueResolver<ProjectCategory, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectCategory source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.ProjectCategory_Id == source.Id, context);
        }
    }

    public class PhaseProjectsByRAGResolver : IValueResolver<ProjectRAGStatus, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectRAGStatus source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.LatestUpdate.RAGStatus.Id == source.Id, context);
        }
    }

    public class PhaseProjectsByOnHoldStatusResolver : IValueResolver<ProjectOnHoldStatus, ProjectSummaryModel, IEnumerable<PhaseProjectsModel>>
    {
        public IEnumerable<PhaseProjectsModel> Resolve(ProjectOnHoldStatus source, ProjectSummaryModel destination, IEnumerable<PhaseProjectsModel> destMember, ResolutionContext context)
        {
            return SummaryLinqQuery.GetQuery(source.Configuration, p => p.LatestUpdate.OnHoldStatus.Id == source.Id, context);
        }
    }
}