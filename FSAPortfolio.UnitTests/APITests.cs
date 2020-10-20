using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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
            TestGetOk($"api/Projects/{testProjectId}/updates");
            TestGetOk($"api/Projects/{testProjectId}/related");
            TestGetOk($"api/Projects/{testProjectId}/dependant");
        }

        [TestMethod]
        public void UpdateProject()
        {
            string testProjectId = ConfigurationManager.AppSettings["TestProjectId"];

            ProjectModel project = new ProjectModel()
            {
                project_id = testProjectId,
                phase = "backlog",
                rag = "nor",
                onhold = "n",
                fsaproc_assurance_gatecompleted = DateTime.Now
            };
            var projectJson = JsonConvert.SerializeObject(project);
            var content = new StringContent(projectJson, Encoding.UTF8, "application/json");
            var response = client.PostAsync("api/Projects", content).Result;
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        private static void TestGetOk(string url)
        {
            var response = client.GetAsync(url).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"GET {url} resulted in {response.StatusCode}");
        }
    }
}
