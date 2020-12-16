using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FSAPortfolio.UnitTests.TestMappings;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    [TestClass]
    public class APIProjectTests
    {


        [TestMethod]
        public async Task UpdateWithNoChange()
        {
            var project = await ProjectTestData.LoadAsync("ODD2009001");
            var projectUpdate = project.Clone();
            await projectUpdate.UpdateAsync();
            var afterproject = await ProjectTestData.LoadAsync("ODD2009001");
            CompareUtil.Compare(project.ProjectEditView, afterproject.ProjectEditView);
        }



    }
}
