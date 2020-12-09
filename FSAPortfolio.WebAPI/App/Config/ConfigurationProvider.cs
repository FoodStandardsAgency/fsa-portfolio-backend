using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Config
{
    public class ConfigurationProvider
    {
        private PortfolioContext context;

        public ConfigurationProvider(PortfolioContext context)
        {
            this.context = context;
        }

        internal async Task UpdateCollections(PortfolioConfiguration config)
        {
            await UpdateRAGStatusOptions(config);

            UpdateProjectOptions(
                config,
                nameof(ProjectModel.category),
                context.Projects.Select(p => p.Category).Union(context.Projects.SelectMany(p => p.Subcategories)),
                config.Categories,
                context.ProjectCategories,
                "categories",
                "project category or subcategory",
                ViewKeyPrefix.Category,
                CategoryConstants.MaxCount
                );

            UpdateProjectOptions(
                config,
                nameof(ProjectModel.phase),
                context.ProjectUpdates.Select(p => p.Phase),
                config.Phases,
                context.ProjectPhases,
                "phases",
                "project phase",
                ViewKeyPrefix.Phase,
                PhaseConstants.MaxCount
                );

            UpdateProjectOptions(
                config,
                nameof(ProjectModel.onhold),
                context.ProjectUpdates.Select(p => p.OnHoldStatus),
                config.OnHoldStatuses,
                context.ProjectOnHoldStatuses,
                "statuses",
                "project status",
                ViewKeyPrefix.Status,
                OnHoldConstants.MaxCount
                );

            UpdateProjectOptions(
                config,
                nameof(ProjectModel.project_size),
                context.Projects.Select(p => p.Size),
                config.ProjectSizes,
                context.ProjectSizes,
                "sizes",
                "project size",
                ViewKeyPrefix.ProjectSize,
                ProjectSizeConstants.MaxCount
                );

            UpdateProjectOptions(
                config,
                nameof(ProjectModel.budgettype),
                context.Projects.Select(p => p.BudgetType),
                config.BudgetTypes,
                context.BudgetTypes,
                "budget types",
                "project budget type",
                ViewKeyPrefix.BudgetType,
                BudgetTypeConstants.MaxCount
                );


            await context.SaveChangesAsync();

        }

        private async Task UpdateRAGStatusOptions(PortfolioConfiguration config)
        {
            var labelConfig = config.Labels.Single(l => l.FieldName == nameof(ProjectModel.rag));
            int options = int.Parse(labelConfig.FieldOptions);
            if (config.RAGStatuses.Count != options)
            {
                ProjectRAGStatus redAmber, amberGreen;
                switch (options)
                {
                    case 5:
                        // Add Red/Amber and Amber/Green and set the order
                        var redAmberMap = SyncMaps.ragMap[RagConstants.RedAmberViewKey];
                        var amberGreenMap = SyncMaps.ragMap[RagConstants.AmberGreenViewKey];
                        redAmber = new ProjectRAGStatus() { ViewKey = RagConstants.RedAmberViewKey, Name = redAmberMap.Item1, Order = redAmberMap.Item2 };
                        amberGreen = new ProjectRAGStatus() { ViewKey = RagConstants.AmberGreenViewKey, Name = amberGreenMap.Item1, Order = amberGreenMap.Item2 };
                        config.RAGStatuses.Add(redAmber);
                        config.RAGStatuses.Add(amberGreen);
                        break;
                    case 3:
                        // Remove Red/Amber and Amber/Green, map any projects with those RAGs
                        var red = config.RAGStatuses.Single(rag => rag.ViewKey == RagConstants.RedViewKey);
                        var amber = config.RAGStatuses.Single(rag => rag.ViewKey == RagConstants.AmberViewKey);
                        redAmber = config.RAGStatuses.Single(rag => rag.ViewKey == RagConstants.RedAmberViewKey);
                        amberGreen = config.RAGStatuses.Single(rag => rag.ViewKey == RagConstants.AmberGreenViewKey);

                        var redAmberUpdates = await context.ProjectUpdates.Where(u => u.RAGStatus.Id == redAmber.Id).ToListAsync();
                        foreach (var update in redAmberUpdates) update.RAGStatus = red;

                        var amberGreenUpdates = await context.ProjectUpdates.Where(u => u.RAGStatus.Id == amberGreen.Id).ToListAsync();
                        foreach (var update in amberGreenUpdates) update.RAGStatus = amber;

                        context.ProjectRAGStatuses.Remove(redAmber);
                        context.ProjectRAGStatuses.Remove(amberGreen);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"The label configuration for fieldname=[{nameof(ProjectModel.rag)}] has an unrecognised value. Should be 3 or 5.");
                }

            }

        }

        private void UpdateProjectOptions<T>(
            PortfolioConfiguration config,
            string fieldName,
            IQueryable<T> existingQuery,
            ICollection<T> optionCollection,
            DbSet<T> dbSet,
            string collectionDescription,
            string optionDescription,
            string viewKeyPrefix,
            int? maxOptionCount = null
            ) where T : class, IProjectOption, new()
        {
            var labelConfig = config.Labels.Single(l => l.FieldName == fieldName);
            var optionNames = labelConfig.FieldOptions
                .Split(',')
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select((n, i) => new { index = i, value = n.Trim() })
                .ToArray();

            if (maxOptionCount.HasValue && optionNames.Length > maxOptionCount.Value)
            {
                throw new PortfolioConfigurationException($"Can't update {collectionDescription}: you entered {optionNames.Length} {collectionDescription}; the {optionDescription} limit is {maxOptionCount.Value}.");
            }

            // If the option has no matching name, check the options has no existing assignments and then delete it
            var unmatchedOptionsQuery =
                // Get options that don't have a match in the new list
                from existingOption in optionCollection
                join name in optionNames on existingOption.Name equals name.value into names
                from name in names.DefaultIfEmpty()
                where name == null
                select existingOption;

            existingQuery = existingQuery.Where(e => e != null);

            var unableToDeleteQuery =
                // Join this to all uses of the option
                from option in unmatchedOptionsQuery
                join existingOption in existingQuery
                on option.Id equals existingOption.Id into usedOptions
                from usedOption in usedOptions
                group usedOption by option into options
                select new { category = options.Key, projectCount = options.Count() };

            var unableToDelete = unableToDeleteQuery.ToList();
            if (unableToDelete.Count > 0)
            {
                // Can't do this update while the categories are assigned to projects
                Func<string, int, string> categoryError = (n, c) => {
                    return c == 1 ?
                    $"[{n}] is used as a {optionDescription} ({c} occurrence)" :
                    $"[{n}] is used as a {optionDescription} ({c} occurrences)";
                };
                throw new PortfolioConfigurationException($"Can't update {collectionDescription}: {string.Join("; ", unableToDelete.Select(c => categoryError(c.category.Name, c.projectCount)))}");
            }
            else
            {
                var unmatchedOptions = unmatchedOptionsQuery.ToList();
                foreach (var option in unmatchedOptions)
                {
                    optionCollection.Remove(option);
                    dbSet.Remove(option);
                }
            }

            // If name has no matching option, add a option
            var matchedNamesQuery =
                from name in optionNames
                join option in optionCollection on name.value equals option.Name into options
                from option in options.DefaultIfEmpty()
                orderby option.Order
                select new { name, option };
            var matchedNames = matchedNamesQuery.ToList();
            int viewKeyIndex = 0;

            if (fieldName == nameof(ProjectModel.phase))
            {
                // Write over the existing elements - add any new ones
                for (int i = 0; i < matchedNames.Count(); i++)
                {
                    var match = matchedNames.ElementAt(i);
                    T option = optionCollection.ElementAtOrDefault(i);
                    if (match.option == null)
                    {
                        option = new T();
                        optionCollection.Add(option);
                    }
                    option.Name = match.name.value;
                    option.Order = i;
                    option.ViewKey = $"{viewKeyPrefix}{i}";
                    optionCollection.Add(option);
                }
                var lastOption = optionCollection.LastOrDefault();
                if(lastOption != null && maxOptionCount.HasValue) lastOption.ViewKey = $"{viewKeyPrefix}{maxOptionCount.Value - 1}";
            }
            else
            {
                optionCollection.Clear();
                for (int i = 0; i < matchedNames.Count(); i++)
                {
                    var match = matchedNames.ElementAt(i);
                    T option = match.option;
                    if (match.option == null)
                    {
                        option = new T() { Name = match.name.value };
                    }

                    // Assign next viewkey
                    if (option.ViewKey == null)
                    {
                        do
                        {
                            option.ViewKey = $"{viewKeyPrefix}{viewKeyIndex++}";
                        }
                        while (matchedNames.Any(m => m.option != null && m.option != option && m.option.ViewKey == option.ViewKey));
                    }

                    // Assign order and add to collection
                    option.Order = match.name.index;
                    optionCollection.Add(option);

                }
            }
        }

    }
}