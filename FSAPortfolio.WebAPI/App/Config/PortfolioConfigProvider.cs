using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Config
{
    public class PortfolioConfigProvider
    {
        private PortfolioConfiguration config;
        IEnumerable<PortfolioLabelModel> labels;

        public PortfolioConfigProvider(PortfolioConfiguration config, IEnumerable<PortfolioLabelModel> labels)
        {
            this.config = config;
            this.labels = labels;
        }

        internal void PopulateLabelValues()
        {
            PopulateLabel(nameof(ProjectModel.category), c => config.Categories.Select(oh => oh.Name));
            PopulateLabel(nameof(ProjectModel.onhold), c => config.OnHoldStatuses.Select(oh => oh.Name));
            PopulateLabel(nameof(ProjectModel.phase), c => config.Phases.Select(oh => oh.Name));
            PopulateLabel(nameof(ProjectModel.rag), c => config.RAGStatuses.Select(oh => oh.Name));
            PopulateLabel(nameof(ProjectModel.project_size), c => config.ProjectSizes.Select(oh => oh.Name));
            PopulateLabel(nameof(ProjectModel.budgettype), c => config.BudgetTypes.Select(oh => oh.Name));
        }

        private void PopulateLabel(string fieldName, Func<PortfolioConfiguration, IEnumerable<string>> factory)
        {
            var label = labels.SingleOrDefault(l => l.FieldName == fieldName);
            label.InputValue = string.Join(", ", factory(config));
        }
    }
}