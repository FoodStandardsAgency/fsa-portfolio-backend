using System;
using System.CodeDom;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FSAPortfolio.UnitTests
{
    [TestClass]
    public class APITests
    {
        static HttpClient client = new HttpClient();

        static APITests()
        {
            client.BaseAddress = new Uri("http://localhost/FSAPortfolio.WebAPI/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("APIKey", ConfigurationManager.AppSettings["APIKey"]);
        }

        [TestMethod]
        public void ProjectEndpoints()
        {
            string testProjectId = ConfigurationManager.AppSettings["TestProjectId"];
            TestGetOk("api/Projects/odd/newproject");
            TestGetOk($"api/Projects/{testProjectId}");
        }

        [TestMethod]
        public void UpdateProject()
        {
            UpdateAndVerify("TestData/projectFormData_init.json");
            UpdateAndVerify("TestData/projectFormData_update.json");
            UpdateAndVerify("TestData/projectFormData_init.json"); // Note that because update is on same day, it updates the previous update!
        }

        private static void UpdateAndVerify(string formBody)
        {
            var projectJson = File.ReadAllText(formBody);
            var project = JsonConvert.DeserializeObject<ProjectUpdateModel>(projectJson);

            // Post the project
            var content = new StringContent(projectJson, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/Projects", content).Result;
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            // Get the project json and compare with what was sent as an update
            var projectResponse = client.GetAsync($"api/Projects/{project.project_id}/edit").Result;
            Assert.AreEqual(HttpStatusCode.OK, projectResponse.StatusCode);
            var dto = JsonConvert.DeserializeObject<GetProjectDTO<ProjectEditViewModel>>(projectResponse.Content.ReadAsStringAsync().Result);
            var updatedProject = dto.Project;

            var unmatched = project.GetUnequalProperties(updatedProject,
                nameof(project.id),
                nameof(project.timestamp),
                nameof(project.min_time),
                nameof(project.max_time),
                nameof(project.pgroup),
                nameof(project.new_flag),
                // TODO: remove these ignores when implemented
                nameof(project.oddlead)
                );
            unmatched.RemoveAll(un => un.Item1 == nameof(project.update) && un.Item2 == "\"\"" && un.Item3 == "null");
            unmatched.RemoveAll(un => un.Item1 == nameof(project.key_contact1) && un.Item2 == "\"\"" && un.Item3 == "null");
            unmatched.RemoveAll(un => un.Item1 == nameof(project.key_contact2) && un.Item2 == "\"\"" && un.Item3 == "null");
            unmatched.RemoveAll(un => un.Item1 == nameof(project.key_contact3) && un.Item2 == "\"\"" && un.Item3 == "null");
            unmatched.RemoveAll(un => un.Item1 == nameof(project.supplier) && un.Item2 == "\"\"" && un.Item3 == "null");

            Assert.AreEqual(0, unmatched.Count, $"Unmatched properties = [{string.Join(", ", unmatched.Select(u => $"{{{u.Item1}:[{u.Item2},{u.Item3}]}}"))}]");
        }

        private void CompareProperties(object expected, object actual)
        {
            var expectedProperties = expected.GetType().GetProperties();
            var actualProperties = actual.GetType().GetProperties().ToDictionary(p => p.Name);
            foreach (var expectedProperty in expectedProperties)
            {
                var expectedValue = expectedProperty.GetValue(expected);
                var actualValue = actualProperties[expectedProperty.Name].GetValue(actual);
                switch(expectedProperty.PropertyType.Name)
                {
                    case nameof(String):
                        Assert.AreEqual(expectedValue, actualValue);
                        break;
                }
            }
        }

        private static void TestGetOk(string url)
        {
            var response = client.GetAsync(url).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"GET {url} resulted in {response.StatusCode}");
        }
    }
}
