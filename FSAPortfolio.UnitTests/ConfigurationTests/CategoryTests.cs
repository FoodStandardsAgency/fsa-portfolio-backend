using FSAPortfolio.UnitTests.APIClients;
using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.DTO;
using FSAPortfolio.Application.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.UnitTests.ConfigurationTests
{
    [TestClass]
    public class CategoryTests
    {
        private const string CategoryBackup = "Backend Feature, Backend Bug, Frontend Feature, Frontend Bug, Database, HTML";
        private const string TestStartCategories = "Food hypersensitivity, Chemical contaminants, Foodborne disease, Antimicrobial resistance, Nutrition and health, Consumer and business behaviour, Food crime, Novel food and processes, Data and digital, Best regulator, Emerging risks and opportunities, Not a current ARI, Unknown";
        private const string TestChangedCategories = "Food hypersensitivity, Chemical hazards in food and feed, Foodborne disease, Antimicrobial resistance, Nutrition and Health, Behaviour and perception, Data and digital innovations, Cutting edge regulator, Emerging challenges and opportunities, Novel and non-traditional foods and processes, Food crime, Other";



        [ClassCleanup]
        public static async Task CleanupTest()
        {
            await ProjectClient.DeleteTestProjectsAsync();
            await PortfolioConfigClient.UpdateCategoriesAsync(TestSettings.TestPortfolio, CategoryBackup);
        }

        [TestMethod]
        public async Task ReconfigureCategoriesTest()
        {
            // Ensure the projects are there
            await ProjectClient.EnsureTestProjects();

            // DATA SET UP -------------------------------
            // Reconfigure the categories
            var categoriesBackup = await PortfolioConfigClient.UpdateCategoriesAsync(TestSettings.TestPortfolio, TestStartCategories);
            Assert.AreEqual(CategoryBackup, categoriesBackup);

            GetProjectQueryDTO options = await PortfolioConfigClient.GetFilterOptionsAsync(TestSettings.TestPortfolio);
            var categoryOptions = options.Options.CategoryItems;

            List<ProjectUpdateModel> updates = new List<ProjectUpdateModel>();
            Action<ProjectUpdateModel, string> ensureCategory = (p, c) => {
                if (p.category != c)
                {
                    p.category = c;
                    updates.Add(p);
                }
            };
            Action<ProjectUpdateModel, string> ensureSubCategory = (p, c) =>
            {
                var _cat = categoryOptions.Single(_c => _c.Display == c);
                if (p.subcat == null || !p.subcat.Contains(_cat.Value))
                {
                    var subCats = new List<string>();
                    if (p.subcat != null) subCats.AddRange(p.subcat); // Add the existing values
                    subCats.Add(_cat.Value); // Add the new value
                    updates.Add(p);
                }
            };

            var testProjects = await ProjectClient.GetTestProjectsAsync();

            int ci = 0;
            for (int i = 0; i < ProjectClient.TestProjectCount; i++)
            {
                ensureCategory(testProjects[i], categoryOptions[ci++].Value);
                if (ci >= categoryOptions.Count) ci = 0;
            }

            ensureSubCategory(testProjects[0], "Best regulator");
            ensureSubCategory(testProjects[1], "Chemical contaminants");
            ensureSubCategory(testProjects[1], "Nutrition and health");
            ensureSubCategory(testProjects[1], "Consumer and business behaviour");
            ensureSubCategory(testProjects[2], "Novel food and processes");
            ensureSubCategory(testProjects[2], "Data and digital");
            ensureSubCategory(testProjects[2], "Best regulator");
            ensureSubCategory(testProjects[3], "Emerging risks and opportunities");
            ensureSubCategory(testProjects[3], "Best regulator");
            ensureSubCategory(testProjects[3], "Chemical contaminants");
            ensureSubCategory(testProjects[4], "Nutrition and health");
            ensureSubCategory(testProjects[4], "Consumer and business behaviour");
            ensureSubCategory(testProjects[5], "Novel food and processes");
            ensureSubCategory(testProjects[6], "Data and digital");
            ensureSubCategory(testProjects[7], "Best regulator");
            ensureSubCategory(testProjects[8], "Emerging risks and opportunities");

            await ProjectClient.UpdateProjectsAsync(updates);
            updates.Clear();

            // CHANGE CATEGORIES  -------------------------------
            categoriesBackup = await PortfolioConfigClient.UpdateCategoriesAsync(TestSettings.TestPortfolio, TestChangedCategories);
            Assert.AreEqual(TestStartCategories, categoriesBackup);

            // Restore the categories
            categoriesBackup = await PortfolioConfigClient.UpdateCategoriesAsync(TestSettings.TestPortfolio, categoriesBackup);
            Assert.AreEqual(TestChangedCategories, categoriesBackup);
        }

        [TestMethod]
        public async Task UpdateCategoryWithHistoryCheck()
        {
            var projectId = TestSettings.TestProjectId;
            var testCategory = "EXTRA CATEGORY";
            string originalCategoryViewKey = null;
            Func<Task> updateCategory = async () => {
                originalCategoryViewKey = await UpdateProjectCategory(projectId, testCategory);
            };

            // Test procedure: 
            // 1. Update config to add the category
            // - at the midpoint, update the project to use the test category
            // - config then resets - but category is in use so is only hidden
            await ConfigurationTestUtils.TestCollectionLabel(ProjectPropertyConstants.category, testCategory, midpointTest: updateCategory);

            // 2. Update the project to not use the label
            var project = await ProjectTestData.LoadAsync(projectId);
            project.ProjectUpdate.category = originalCategoryViewKey;
            await project.UpdateAsync();

            // 3. Check the option has disappeared.
            await project.LoadAsync();
            var categoryItem = project.DTO.Options.CategoryItems.SingleOrDefault(i => i.Display == testCategory);
            Assert.IsNull(categoryItem);

            // 4. Do a no-change update on config (hidden option should be removed - but no way to verify!)
            var config = await ConfigTestData.LoadAsync(TestSettings.TestPortfolio);
            await config.UpdateAsync();

        }

        private async Task<string> UpdateProjectCategory(string projectId, string categoryName)
        {
            // 1. GET PROJECT
            var initial = await ProjectTestData.LoadAsync(projectId);
            var originalCategoryViewKey = initial.DTO.Project.category;
            var categoryItem = initial.DTO.Options.CategoryItems.Single(i => i.Display == categoryName);

            // 2. UPDATE PROJECT category
            var update = initial.Clone();
            update.ProjectUpdate.category = categoryItem.Value;
            await update.UpdateAsync();

            // 3. Check the option is present
            await update.LoadAsync();
            Assert.AreEqual(categoryItem.Value, update.DTO.Project.category);

            return originalCategoryViewKey;
        }


    }
}
