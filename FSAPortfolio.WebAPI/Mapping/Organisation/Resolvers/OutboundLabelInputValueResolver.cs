using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace FSAPortfolio.WebAPI.Mapping.Organisation.Resolvers
{
    public class OutboundLabelInputValueResolver : IValueResolver<PortfolioLabelConfig, PortfolioLabelModel, string>
    {
        public string Resolve(PortfolioLabelConfig source, PortfolioLabelModel destination, string destMember, ResolutionContext context)
        {
            string result = null;
            switch(source.FieldName)
            {
                case nameof(ProjectModel.category):
                    result = GetValue(source.Configuration.Categories);
                    break;
                case nameof(ProjectModel.onhold):
                    result = GetValue(source.Configuration.OnHoldStatuses);
                    break;
                case nameof(ProjectModel.phase):
                    result = GetValue(source.Configuration.Phases);
                    break;
                case nameof(ProjectModel.rag):
                    result = source.Configuration.RAGStatuses.Count.ToString();
                    break;
                case nameof(ProjectModel.project_size):
                    result = GetValue(source.Configuration.ProjectSizes);
                    break;
                case nameof(ProjectModel.budgettype):
                    result = GetValue(source.Configuration.BudgetTypes);
                    break;
                default:
                    result = source.FieldOptions;
                    break;
            }
            return result;
        }

        private string GetValue<T>(ICollection<T> collection) where T : IProjectOption
        {
            return string.Join(", ", collection.OrderBy(o => o.Order).Select(o => o.Name));
        }

    }
}