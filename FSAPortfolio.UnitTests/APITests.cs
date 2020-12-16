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
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FSAPortfolio.UnitTests
{
    [TestClass]
    public class APITests
    {
        private static HttpClient client => BackendAPIClient.client;

        private static void UpdateAndVerify(string formBody)
        {
            var projectJson = File.ReadAllText(formBody);
            var project = JsonConvert.DeserializeObject<ProjectUpdateModel>(projectJson);

            // Post the project
            var content = new StringContent(projectJson, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/Projects", content).Result;
            var response1Content = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            // Get the project json and compare with what was sent as an update
            var projectResponse = client.GetAsync($"api/Projects/{project.project_id}/edit").Result;
            var response2Content = projectResponse.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(HttpStatusCode.OK, projectResponse.StatusCode);
            var dto = JsonConvert.DeserializeObject<GetProjectDTO<ProjectEditViewModel>>(response2Content);
            var updatedProject = dto.Project;

            // Equalise null display names because api fills these if null or empty
            if (project.oddlead != null && project.oddlead.DisplayName == null) project.oddlead.DisplayName = project.oddlead.Value;
            if (project.key_contact1 != null && project.key_contact1.DisplayName == null) project.key_contact1.DisplayName = project.key_contact1.Value;
            if (project.key_contact2 != null && project.key_contact2.DisplayName == null) project.key_contact2.DisplayName = project.key_contact2.Value;
            if (project.key_contact3 != null && project.key_contact3.DisplayName == null) project.key_contact3.DisplayName = project.key_contact3.Value;


            var unmatched = project.GetUnequalProperties(updatedProject,
                nameof(project.id),
                nameof(project.timestamp),
                nameof(project.min_time),
                nameof(project.max_time),
                nameof(project.pgroup),
                nameof(project.new_flag),
                nameof(project.team)
                );
            unmatched.RemoveAll(un => un.Item1 == nameof(project.update) && un.Item2 == "\"\"" && un.Item3 == "null");
            unmatched.RemoveAll(un => un.Item1 == nameof(project.key_contact1) && un.Item2 == "\"\"" && un.Item3 == "null");
            unmatched.RemoveAll(un => un.Item1 == nameof(project.key_contact2) && un.Item2 == "\"\"" && un.Item3 == "null");
            unmatched.RemoveAll(un => un.Item1 == nameof(project.key_contact3) && un.Item2 == "\"\"" && un.Item3 == "null");
            unmatched.RemoveAll(un => un.Item1 == nameof(project.supplier) && un.Item2 == "\"\"" && un.Item3 == "null");

            Assert.AreEqual(0, unmatched.Count, $"Unmatched properties = [{string.Join(", ", unmatched.Select(u => $"{{{u.Item1}:[{u.Item2},{u.Item3}]}}"))}]");

            // Have to compare these manually
            if (project.team == null || project.team.Length == 0) Assert.IsTrue(updatedProject.team == null || updatedProject.team.Length == 0);
            else
            {
                foreach (var team in updatedProject.team)
                {
                    Assert.IsTrue(project.team.Contains(team.Value));
                }
            }
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
