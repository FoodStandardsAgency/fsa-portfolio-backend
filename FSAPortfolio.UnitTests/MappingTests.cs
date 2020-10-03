using System;
using FSAPortfolio.WebAPI.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FSAPortfolio.UnitTests
{
    [TestClass]
    public class MappingTests
    {
        [TestMethod]
        public void MappingProfilesAreValid()
        {
            PortfolioMapper.Configure();
            PortfolioMapper.config.AssertConfigurationIsValid();
        }
    }
}
