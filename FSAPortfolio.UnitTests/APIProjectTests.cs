using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    [TestClass]
    public class APIProjectTests
    {
        private static string TestProjectId = ConfigurationManager.AppSettings["TestProjectId"];

        [TestMethod]
        public async Task UpdateWithNoChange()
        {
            var project = await ProjectTestData.LoadAsync(TestProjectId);
            var projectUpdate = project.Clone();
            await projectUpdate.UpdateAsync();
            var afterproject = await ProjectTestData.LoadAsync(TestProjectId);
            CompareUtil.Compare(project.ProjectEditView, afterproject.ProjectEditView);
        }



    }
}
