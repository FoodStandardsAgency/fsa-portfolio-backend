using FSAPortfolio.UnitTests.APIClients;
using FSAPortfolio.UnitTests.TestMappings;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.Application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    public class ProjectTestData
    {
        private string projectId;
        public GetProjectDTO<ProjectEditViewModel> DTO { get; private set; }
        public ProjectEditViewModel ProjectEditView => DTO.Project;
        public ProjectUpdateModel ProjectUpdate { get; private set; }
        public ProjectTestData(string projectId)
        {
            this.projectId = projectId;
        }
        public async Task LoadAsync()
        {
            DTO = await ProjectClient.GetProjectAsync(projectId);
            ProjectUpdate = TestMapper.ProjectMapper.Map<ProjectUpdateModel>(ProjectEditView);
        }
        public ProjectTestData Clone()
        {
            var clone = new ProjectTestData(projectId);
            clone.DTO = TestMapper.ProjectMapper.Map<GetProjectDTO<ProjectEditViewModel>>(DTO);
            clone.ProjectUpdate = TestMapper.ProjectMapper.Map<ProjectUpdateModel>(clone.ProjectEditView);
            return clone;
        }

        internal async Task UpdateAsync()
        {
            await ProjectClient.UpdateProjectAsync(ProjectUpdate);
        }


        public static async Task<ProjectTestData> LoadAsync(string projectId)
        {
            var project = new ProjectTestData(projectId);
            await project.LoadAsync();
            return project;
        }

    }
}
