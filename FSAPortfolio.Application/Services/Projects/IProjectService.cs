using FSAPortfolio.Application.Models;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using System;
using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.App.Projects
{
    public interface IProjectService
    {
        Project CreateNewProject(ProjectReservation reservation);
        Task<ProjectReservation> GetProjectReservationAsync(string projectId);
        Task UpdateProject(ProjectUpdateModel update, ProjectReservation reservation = null, Action<Portfolio> permissionCallback = null);
    }
}