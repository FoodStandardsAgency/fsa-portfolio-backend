﻿using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace FSAPortfolio.WebAPI.App.Mapping.Organisation.Resolvers
{
    public class OutboundLabelInputValueResolver : IValueResolver<PortfolioLabelConfig, PortfolioLabelModel, string>
    {
        public string Resolve(PortfolioLabelConfig source, PortfolioLabelModel destination, string destMember, ResolutionContext context)
        {
            string result;
            switch(source.FieldName)
            {
                case ProjectPropertyConstants.category:
                    result = GetValue(source.Configuration.Categories);
                    break;
                case ProjectPropertyConstants.onhold:
                    result = GetValue(source.Configuration.OnHoldStatuses);
                    break;
                case ProjectPropertyConstants.phase:
                    result = GetValue(source.Configuration.Phases);
                    break;
                case nameof(ProjectModel.rag):
                    result = source.Configuration.RAGStatuses.Count.ToString();
                    break;
                case ProjectPropertyConstants.project_size:
                    result = GetValue(source.Configuration.ProjectSizes);
                    break;
                case ProjectPropertyConstants.budgettype:
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