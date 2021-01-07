using FSAPortfolio.UnitTests.APIClients;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.PortfolioTests
{
    [TestClass]
    public class ArchiveTests
    {
        [TestMethod]
        public async Task ArchiveTest()
        {
            var response = await PortfolioClient.ArchiveProjectsAsync("odd");
        }
    }
}
