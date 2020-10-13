using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FSAPortfolio.WebAPI.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        private static void TestGetOk(string url)
        {
            var response = client.GetAsync(url).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"GET {url} resulted in {response.StatusCode}");
        }
    }
}
