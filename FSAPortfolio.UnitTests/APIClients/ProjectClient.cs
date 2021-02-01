using FSAPortfolio.UnitTests.ConfigurationTests;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.APIClients
{
    public static class ProjectClient
    {
        public const string TestProjectPrefix = "Unit_Test_";
        public const int TestProjectCount = 20;
        public static async Task<IEnumerable<SelectItemModel>> GetProjectByNameOrIdAsync(string nameOrId)
        {
            var queryParams = new Dictionary<string, string>() {
                    { "term", nameOrId }
            };
            return await BackendAPIClient.GetAsync<IEnumerable<SelectItemModel>>("api/Projects", queryParams);
        }

        public static async Task CreateProjectAsync(string portfolio, string title)
        {
            var project = new ProjectUpdateModel()
            {
                project_name = title,
                phase = "phase0"
            };
            await CreateProjectAsync(portfolio, project);
        }
        public static async Task CreateProjectAsync(string portfolio, ProjectUpdateModel project)
        {
            var dto = await BackendAPIClient.GetAsync<GetProjectDTO<ProjectEditViewModel>>($"api/Projects/{portfolio}/newproject");
            project.project_id = dto.Project.project_id;
            await BackendAPIClient.PostAsync<ProjectUpdateModel>("api/Projects", project);
        }

        internal static async Task UpdateProjectsAsync(List<ProjectUpdateModel> testProjects)
        {
            foreach(var project in testProjects)
            {
                await UpdateProjectAsync(project);
            }
        }

        internal static async Task UpdateProjectAsync(ProjectUpdateModel update)
        {
            await BackendAPIClient.PostAsync($"api/Projects", update);
        }

        internal static async Task<GetProjectDTO<ProjectEditViewModel>> GetProjectAsync(string projectId)
        {
            return await BackendAPIClient.GetAsync<GetProjectDTO<ProjectEditViewModel>>($"api/Projects/{projectId}/edit");
        }


        internal static async Task<List<ProjectUpdateModel>> GetTestProjectsAsync(string prefix = TestProjectPrefix)
        {
            var projects = new List<ProjectUpdateModel>();
            int decade = 0;
            IEnumerable<SelectItemModel> searchResults;
            do
            {
                searchResults = await GetProjectByNameOrIdAsync($"{prefix}{decade++}");
                foreach (var result in searchResults)
                {
                    var project = await ProjectTestData.LoadAsync(result.Value);
                    projects.Add(project.ProjectUpdate);
                }
            }
            while (searchResults.Count() > 0);

            return projects;
        }

        public static async Task EnsureTestProjects(int count = TestProjectCount, string prefix = TestProjectPrefix)
        {
            Func<string, Task> ensureProject = async name =>
            {
                var results = await GetProjectByNameOrIdAsync(name);
                if (results.Count() == 0)
                {
                    await CreateProjectAsync(TestSettings.TestPortfolio, name);
                }
            };

            for (int i = 1; i <= count; i++)
            {
                await ensureProject($"{prefix}{i:D2}");
            }
        }

        public static async Task DeleteTestProjectsAsync(string prefix = TestProjectPrefix)
        {
            IEnumerable<SelectItemModel> searchResults;
            do
            {
                searchResults = await GetProjectByNameOrIdAsync($"{prefix}");
                foreach (var result in searchResults)
                {
                    await DeleteProjectAsync(result.Value);
                }
            }
            while (searchResults.Count() > 0);
        }

        public static async Task DeleteProjectAsync(string projectId)
        {
            await BackendAPIClient.DeleteAsync($"api/Projects/{projectId}");
        }
    }
}
