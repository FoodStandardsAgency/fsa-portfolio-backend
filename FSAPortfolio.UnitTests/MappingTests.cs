using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Mapping;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using Newtonsoft.Json;

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
        }
    }
}
