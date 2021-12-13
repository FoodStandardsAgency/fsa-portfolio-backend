using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Mapping;
using FSAPortfolio.Application.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FSAPortfolio.Entities;

namespace FSAPortfolio.UnitTests
{
    [TestClass]
    public class MappingTests
    {
        [TestMethod]
        public void MappingProfilesAreValid()
        {
            PortfolioMapper.Configure();
            PortfolioMapper.projectConfig.AssertConfigurationIsValid();
            PortfolioMapper.configConfig.AssertConfigurationIsValid();
            PortfolioMapper.updateConfig.AssertConfigurationIsValid();
            PortfolioMapper.exportConfig.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void NullableFloatMap()
        {
            PortfolioMapper.Configure();
            float? f = PortfolioMapper.ExportMapper.Map<float?>(string.Empty);
            Assert.IsNull(f);
        }

        [TestMethod]
        public void NewProjectMap()
        {
            PortfolioMapper.Configure();
            var newProject = new Project() {
                Reservation = new ProjectReservation() { ProjectId = "TEST123" }
            };

            var model = ProjectModelFactory.GetProjectEditModel(newProject);

            Assert.IsNotNull(model);

            var minYear = DateTime.Now.Year - PortfolioSettings.ProjectDateMinYearOffset;
            var maxYear = DateTime.Now.Year + PortfolioSettings.ProjectDateMaxYearOffset;
            Assert.AreEqual(minYear, model.MinProjectYear);
            Assert.AreEqual(maxYear, model.MaxProjectYear);
        }

    }
}
