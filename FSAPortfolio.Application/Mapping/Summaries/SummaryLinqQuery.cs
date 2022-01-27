using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Users;

namespace FSAPortfolio.Application.Mapping.Organisation.Resolvers.Summaries
{
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
                        Projects = context.Mapper.Map<IEnumerable<ProjectIndexModel>>(phaseGroup.Where(p => p != null).OrderByDescending(p => p.Priority))
                    };
            return q.OrderBy(p => p.Order);
        }
    }
}

