using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.SummaryTests
{
    [TestClass]
    public class ProjectSummaryTests
    {

        [TestMethod]
        public void NoLeadSummaryTest()
        {
            // Configure test data
            var l1 = new Person() { Id = 1, ActiveDirectoryDisplayName = "Person1" };
            var ph1 = new ProjectPhase() { Id = 1, Name = "Phase1", ViewKey = "phase1", Order = 0 };
            var ph2 = new ProjectPhase() { Id = 2, Name = "Phase2", ViewKey = "phase2", Order = 1 };
            var ph5 = new ProjectPhase() { Id = 5, Name = "Phase5", ViewKey = "phase5", Order = 2 };

            PortfolioConfiguration config = new PortfolioConfiguration() { Phases = new List<ProjectPhase>() { ph1, ph2, ph5 } };
            ph1.Configuration = config;
            ph2.Configuration = config;
            ph5.Configuration = config;

            var up1 = new ProjectUpdateItem() { Phase = ph1 };
            var up2 = new ProjectUpdateItem() { Phase = ph1 };
            Portfolio portfolio = new Portfolio() {
                Configuration = config,
                Projects = new List<Project>()
                {
                    new Project()
                    {
                        ProjectReservation_Id = 1,
                        Name = "Project1",
                        Updates = new List<ProjectUpdateItem>() { up1 },
                        LatestUpdate = up1
                    },
                    new Project()
                    {
                        ProjectReservation_Id = 2,
                        Name = "Project2",
                        Updates = new List<ProjectUpdateItem>() { up2 },
                        LatestUpdate = up2,
                        Lead = l1,
                        Lead_Id = l1.Id
                    }
                }
            };
            portfolio.Configuration.Portfolio = portfolio;
            portfolio.Configuration.CompletedPhase = ph5;
            var summaryType = PortfolioSummaryModel.ByLead;

            // Initialise mapping configuration
            PortfolioMapper.Configure();

            // Map the test data
            var result = PortfolioMapper.ConfigMapper.Map<PortfolioSummaryModel>(
            portfolio,
            opt =>
            {
                opt.Items[nameof(PortfolioConfiguration)] = portfolio.Configuration;
                opt.Items[nameof(PortfolioSummaryModel)] = summaryType;
            });

            // Check result
            // result
            // -- Phases
            //  -- Phases[0]
            //      -- ViewKey: phase1 
            //      -- Count: 2
            //  -- Phases[1]
            //      -- ViewKey: phase2
            //      -- Count: 0
            // -- Summaries
            //  -- Summaries[0]
            //      -- Name: "None set"
            //      -- PhaseProjects
            //          -- Count: 2
            //          -- PhaseProjects[0]
            //              -- ViewKey: "phase1"
            //              -- Projects
            //                  -- Count: 1
            //                  -- Projects[0]
            //                      -- Name: "Project1"
            //          -- PhaseProjects[1]
            //              -- ViewKey: "phase2"
            //              -- Projects
            //                  -- Count: 0
            //  -- Summaries[1]
            //      -- Name: "Person1"
            //      -- PhaseProjects
            //          -- PhaseProjects[0]
            //              -- ViewKey: "phase1"
            //              -- Projects
            //                  -- Count: 1
            //                  -- Projects[0]
            //                      -- Name: "Project2"
            //          -- PhaseProjects[1]
            //              -- ViewKey: "phase2"
            //              -- Projects
            //                  -- Count: 0
            var phases = result.Phases.ToList();
            Assert.AreEqual(2, phases.Count());
            Assert.AreEqual("phase1", phases[0].ViewKey);
            Assert.AreEqual("phase2", phases[1].ViewKey);
            Assert.AreEqual(2, phases[0].Count); // Projects in phase 1
            Assert.AreEqual(0, phases[1].Count); // This is phase 2 (completed phase is hidden)

            var summaries = result.Summaries.ToList();

            // ... counts
            Assert.AreEqual(2, summaries.Count());

            // ... Summary 0 ("None set") should have 1 project in phase1
            Assert.AreEqual(ProjectTeamConstants.NotSetName, summaries[0].Name);
            Assert.AreEqual(2, summaries[0].PhaseProjects.ToList().Count());
            Assert.AreEqual("phase1", summaries[0].PhaseProjects.ToList()[0].ViewKey);
            Assert.AreEqual(1, summaries[0].PhaseProjects.ToList()[0].Projects.Count());
            Assert.AreEqual("Project1", summaries[0].PhaseProjects.ToList()[0].Projects.First().Name);

            Assert.AreEqual("phase2", summaries[0].PhaseProjects.ToList()[1].ViewKey);
            Assert.AreEqual(0, summaries[0].PhaseProjects.ToList()[1].Projects.Count());

            // ... Summary 1 ("Person1") should have 1 project in phase1
            Assert.AreEqual("Person1", summaries[1].Name);
            Assert.AreEqual(2, summaries[1].PhaseProjects.ToList().Count());
            Assert.AreEqual("phase1", summaries[1].PhaseProjects.ToList()[0].ViewKey);
            Assert.AreEqual(1, summaries[1].PhaseProjects.ToList()[0].Projects.Count());
            Assert.AreEqual("Project2", summaries[1].PhaseProjects.ToList()[0].Projects.First().Name);

            Assert.AreEqual("phase2", summaries[1].PhaseProjects.ToList()[1].ViewKey);
            Assert.AreEqual(0, summaries[1].PhaseProjects.ToList()[1].Projects.Count());
        }

    }
}
