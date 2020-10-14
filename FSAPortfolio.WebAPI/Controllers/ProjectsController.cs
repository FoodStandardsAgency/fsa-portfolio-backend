﻿using FSAPortfolio.Entities;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.PostgreSQL.Projects;
using System.Text;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.App.Projects;
using FSAPortfolio.WebAPI.Mapping.Projects;
using FSAPortfolio.Entities.Organisation;

namespace FSAPortfolio.WebAPI.Controllers
{
    public class ProjectsController : ApiController
    {
        // POST: api/Projects
        [HttpPost]
        public async Task Post([FromBody]ProjectModel update)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var timestamp = DateTime.Now;

                    // Load and map the project
                    var project = await context.Projects.ProjectIncludes().SingleAsync(p => p.ProjectId == update.project_id);
                    PortfolioMapper.ProjectMapper.Map(update, project, opt => opt.Items[ProjectMappingProfile.PortfolioContextKey] = context);

                    // Record changes
                    AuditProvider.LogChanges(
                        context,
                        (ts, txt) => auditLogFactory(ts, txt),
                        project.AuditLogs,
                        DateTime.Now);

                    // Create a new update
                    var projectUpdate = new ProjectUpdateItem() { Project = project };
                    PortfolioMapper.ProjectMapper.Map(update, projectUpdate, opt => opt.Items[ProjectMappingProfile.PortfolioContextKey] = context);
                    projectUpdate.Timestamp = DateTime.Now;
                    if (!projectUpdate.IsDuplicate(project.LatestUpdate))
                    {
                        project.Updates.Add(projectUpdate);
                        project.LatestUpdate = projectUpdate;
                    }

                    // Save
                    await context.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        // GET: api/Projects?portfolio={portfolio}&filter={filter}
        [HttpGet]
        public async Task<IEnumerable<latest_projects>> Get([FromUri]string portfolio, [FromUri] string filter)
        {
            try
            {
                IEnumerable<latest_projects> result = null;
                using (var context = new PortfolioContext())
                {
                    var config = await context.Portfolios
                        .Where(p => p.ViewKey == portfolio)
                        .Select(p => p.Configuration)
                        .Include(c => c.CompletedPhase)
                        .SingleAsync();

                    IQueryable<Project> query = ProjectWithIncludes(context, portfolio);
                    switch (filter)
                    {
                        case "new":
                            var newCutoff = DateTime.Now.AddDays(-PortfolioSettings.NewProjectLimitDays);
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id != config.CompletedPhase.Id && p.FirstUpdate.Timestamp > newCutoff
                                    orderby p.Priority descending, p.Name
                                    select p;
                            break;
                        case "complete":
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id == config.CompletedPhase.Id
                                    orderby p.LatestUpdate.Timestamp descending, p.Name
                                    select p;
                            break;
                        case "current":
                        case "latest":
                        default:
                            query = from p in query
                                    where p.LatestUpdate.Phase.Id != config.CompletedPhase.Id
                                    orderby p.Priority descending, p.Name
                                    select p;
                            break;
                    }
                    var projects = await query.ToListAsync();
                    result = PortfolioMapper.ProjectMapper.Map<IEnumerable<latest_projects>>(projects);
                }
                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets a new project with any default settings and reserves an project_id.
        /// Gets the label configuration for the view, with options set for any view components such as drop down lists.
        /// </summary>
        /// <param name="portfolio">The portfolio to create the new project for</param>
        /// <returns>A DTO with the label config, options and default project data.</returns>
        /// <remarks>Labels must have the Create flag set in order to be included in the config data.</remarks>
        [HttpGet]
        public async Task<GetNewProjectDTO> GetNewProject([FromUri] string portfolio)
        {
            using (var provider = new ProjectProvider(portfolio))
            {
                var config = await provider.GetConfigAsync();
                var reservation = await provider.GetProjectReservationAsync(config);

                var result = new GetNewProjectDTO()
                {
                    Config = PortfolioMapper.ProjectMapper.Map<ProjectLabelConfigModel>(config, opts => opts.Items[nameof(PortfolioFieldFlags)] = PortfolioFieldFlags.Create),
                    Options = PortfolioMapper.ProjectMapper.Map<ProjectOptionsModel>(config),
                    Project = new ProjectModel() { project_id = reservation.ProjectId }
                };
                return result;
            }
        }


        [HttpGet]
        public async Task<GetProjectDTO> Get(string projectId)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var project = (from p in context.Projects.ProjectIncludes().ConfigIncludes()
                                   where p.ProjectId == projectId select p).Single();
                    var result = new GetProjectDTO()
                    {
                        Config = PortfolioMapper.ProjectMapper.Map<ProjectLabelConfigModel>(project.OwningPortfolio.Configuration),
                        Project = PortfolioMapper.ProjectMapper.Map<ProjectModel>(project)
                    };
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public Task<IEnumerable<ProjectModel>> GetFiltered(string projectId, [FromUri] string filter)
        {
            switch(filter)
            {
                case "related":
                    return GetRelatedProjects(projectId);
                case "dependent":
                    return GetDependantProjects(projectId);
                default:
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> GetRelatedProjects(string projectId)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var projects = await context.Projects.ProjectIncludes()
                        .Where(p => p.ProjectId == projectId)
                        .SelectMany(p => p.RelatedProjects)
                        .ToListAsync();
                    var result = PortfolioMapper.ProjectMapper.Map<IEnumerable<ProjectModel>>(projects);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> GetDependantProjects(string projectId)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var projects = await context.Projects.ProjectIncludes()
                        .Where(p => p.ProjectId == projectId)
                        .SelectMany(p => p.DependantProjects)
                        .ToListAsync(); ;
                    var result = PortfolioMapper.ProjectMapper.Map<IEnumerable<ProjectModel>>(projects);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Gets text for non-empty updates with timestamps and dates.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ProjectUpdateModel>> GetUpdates(string projectId)
        {
            try
            {
                using (var context = new PortfolioContext())
                {
                    var project = await (from u in context.Projects
                                         .Include(p => p.Updates)
                                         .Include(p => p.LatestUpdate)
                                         where u.ProjectId == projectId
                                         select u)
                                   .SingleAsync();
                    var updates = (from u in project.Updates
                                   where !string.IsNullOrEmpty(u.Text)
                                   orderby u.Timestamp descending
                                   select u).ToList();
                    var result = PortfolioMapper.ProjectMapper.Map<IEnumerable<ProjectUpdateModel>>(updates);
                    return result;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private static IQueryable<Project> ProjectWithIncludes(PortfolioContext context, string portfolio)
        {
            return context.Projects.ProjectIncludes().Where(p => p.Portfolios.Any(po => po.ViewKey == portfolio));
        }

        private ProjectAuditLog auditLogFactory(DateTime timestamp, string text)
        {
            return new ProjectAuditLog()
            {
                Timestamp = timestamp,
                Text = text
            };
        }
    }
}
