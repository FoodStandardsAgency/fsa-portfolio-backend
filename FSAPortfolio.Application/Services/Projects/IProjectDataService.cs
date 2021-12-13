using FSAPortfolio.Application.Models;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Services.Projects
{
    public interface IProjectDataService : IBaseService
    {
        Task<GetProjectExportDTO> GetProjectExportDTOAsync(string viewKey);
        Task<ProjectCollectionModel> GetProjectDataAsync(string portfolio, string[] projectIds);
        Task<ProjectUpdateCollectionModel> GetProjectUpdateDataAsync(string portfolio, string[] projectIds);
        Task<ProjectChangeCollectionModel> GetProjectChangeDataAsync(string portfolio, string[] projectIds);
        Task<GetProjectDTO<ProjectEditViewModel>> CreateNewProjectAsync(string portfolio);
        Task<GetProjectDTO<ProjectViewModel>> GetProjectAsync(string projectId, bool includeOptions, bool includeHistory, bool includeLastUpdate, bool includeConfig);
        Task<GetProjectDTO<ProjectEditViewModel>> GetProjectForEdit(string projectId);
        Task<Project> DeleteProjectAsync(string projectId);
        Task ImportProjectsAsync(string viewKey, MultipartFormDataStreamProvider files);
        Task UpdateProjectAsync(ProjectUpdateModel update);
    }
}