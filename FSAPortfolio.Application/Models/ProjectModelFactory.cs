using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public static class ProjectModelFactory
    {
        public static ProjectEditViewModel GetProjectEditModel(Project project, bool includeHistory = false, bool includeLastUpdate = false)
            => GetProjectModel<ProjectEditViewModel>(project, includeHistory, includeLastUpdate);

        public static T GetProjectModel<T>(Project project, bool includeHistory, bool includeLastUpdate)
            where T : ProjectModel
        {
            return PortfolioMapper.ProjectMapper.Map<T>(project,
                opt =>
                {
                    opt.Items[nameof(ProjectViewModel.UpdateHistory)] = includeHistory;
                    opt.Items[nameof(ProjectEditViewModel.LastUpdate)] = includeLastUpdate;
                });
        }

    }
}