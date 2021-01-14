using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App.Sync;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

            UpdatePhaseOptions(config);

            UpdateProjectOptions(
                config,
                ProjectPropertyConstants.category,
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
                ProjectPropertyConstants.onhold,
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
                ProjectPropertyConstants.project_size,
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
                ProjectPropertyConstants.budgettype,
                context.Projects.Select(p => p.BudgetType),
                config.BudgetTypes,
                context.BudgetTypes,
                "budget types",
                "project budget type",
                ViewKeyPrefix.BudgetType,
                BudgetTypeConstants.MaxCount
                );

            try
            {
                // Allow phases to be reordered with same names: 
                using (var transaction = context.Database.BeginTransaction())
                {
                    // - give the phase names a temporary prefix and save
                    foreach (var phase in config.Phases)
                    {
                        phase.Name = $"__{phase.Name}";
                    }
                    await context.SaveChangesAsync();

                    // - remove the prefix
                    foreach (var phase in config.Phases)
                    {
                        phase.Name = Regex.Match(phase.Name, "^__(.*)").Groups[1].Value;
                    }
                    await context.SaveChangesAsync();

                    transaction.Commit();
                }
            }
            catch (DbUpdateException e)
            {
                var builder = new StringBuilder();
                foreach(var entry in e.Entries)
                {
                    if(entry.Entity is ProjectPhase && entry.State == EntityState.Deleted)
                    {
                        var phase = entry.Entity as ProjectPhase;
                        builder.Append($"Phase [{phase.Name}] can't be removed because it has projects assigned to it. This is likely occurring because you are trying to reduce the number of phases but there are projects assigned to the phase to be removed.");
                    }
                    else
                    {
                        builder.Append($"Please send this message to system support: Error updating project collections Entity type = {entry.Entity.GetType().Name}, Entity State = {entry.State}");
                    }
                }
                throw new PortfolioConfigurationException(builder.ToString(), e);
            }
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
                    case 6:
                        // Add Red/Amber and Amber/Green and set the order
                        var redAmberMap = SyncMaps.ragMap[RagConstants.RedAmberViewKey];
                        var amberGreenMap = SyncMaps.ragMap[RagConstants.AmberGreenViewKey];
                        redAmber = new ProjectRAGStatus() { ViewKey = RagConstants.RedAmberViewKey, Name = redAmberMap.Item1, Order = redAmberMap.Item2 };
                        amberGreen = new ProjectRAGStatus() { ViewKey = RagConstants.AmberGreenViewKey, Name = amberGreenMap.Item1, Order = amberGreenMap.Item2 };
                        config.RAGStatuses.Add(redAmber);
                        config.RAGStatuses.Add(amberGreen);
                        break;
                    case 4:
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

        private void UpdatePhaseOptions(PortfolioConfiguration config)
        {
            var labelConfig = config.Labels.Single(l => l.FieldName == ProjectPropertyConstants.phase);
            var optionNames = labelConfig.FieldOptions
                .Split(',')
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select((n, i) => new { index = i, value = n.Trim(), lowervalue = n.Trim().ToLower() })
                .ToArray();

            if (optionNames.Length > PhaseConstants.MaxCount)
            {
                throw new PortfolioConfigurationException($"Can't update phases: you entered {optionNames.Length} phases; the maximum number of phases is {PhaseConstants.MaxCount}.");
            }
            else if (optionNames.Length < 2)
            {
                throw new PortfolioConfigurationException($"Can't update phases: you entered {optionNames.Length} phases; the minimum number of phases is 2.");
            }

            var optionCollection = config.Phases;
            var dbSet = context.ProjectPhases;
            var viewKeyPrefix = ViewKeyPrefix.Phase;

            // If name has no matching option, add a option
            var matchedNamesQuery =
                from name in optionNames
                join option in optionCollection on name.lowervalue equals option.Name.ToLower() into options
                from option in options.DefaultIfEmpty()
                orderby name.index
                select new { name, option };
            var matchedNames = matchedNamesQuery.ToList();

            int phaseIndex = 0;
            int matchIndex = 0;
            int lastButOnePhaseIndex = PhaseConstants.MaxCount - 2;
            int lastButOneMatchIndex = matchedNames.Count() - 2;
            int lastMatchIndex = matchedNames.Count() - 1;
            List<ProjectPhase> phasesToRemove = new List<ProjectPhase>();

            // Write over the existing elements - add any new ones
            while (phaseIndex < PhaseConstants.MaxCount)
            {
                var match = matchedNames.ElementAt(matchIndex);
                var phaseViewKey = $"{viewKeyPrefix}{phaseIndex}";
                if (matchIndex == lastButOneMatchIndex)
                {
                    // Removed unrequired phases
                    while (phaseIndex < lastButOnePhaseIndex)
                    {
                        var phase = optionCollection.SingleOrDefault(p => p.ViewKey == phaseViewKey);
                        if (phase != null) phasesToRemove.Add(phase);
                        phaseViewKey = $"{viewKeyPrefix}{++phaseIndex}";
                    }
                }

                ProjectPhase option = optionCollection.SingleOrDefault(p => p.ViewKey == phaseViewKey);
                if (option == null)
                {
                    option = new ProjectPhase() { ViewKey = phaseViewKey, Order = phaseIndex };
                    optionCollection.Add(option);
                }
                option.Name = match.name.value;

                // Set the archive and completed phases
                if (matchIndex == lastButOneMatchIndex) config.ArchivePhase = option;
                if (matchIndex == lastMatchIndex) config.CompletedPhase = option;

                phaseIndex++;
                matchIndex++;
            }

            foreach (var phase in phasesToRemove)
            {
                optionCollection.Remove(phase);
                dbSet.Remove(phase);
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
            int? maxOptionCount = null,
            bool hideHistoric = true) 
            where T : class, IProjectOption, new()
        {
            var labelConfig = config.Labels.Single(l => l.FieldName == fieldName);

            var optionNames = labelConfig.FieldOptions
                .Split(',')
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select((n, i) => (index: i, value: n.Trim(), lowervalue: n.Trim().ToLower() ))
                .ToArray();

            if (maxOptionCount.HasValue && optionNames.Length > maxOptionCount.Value)
            {
                throw new PortfolioConfigurationException($"Can't update {collectionDescription}: you entered {optionNames.Length} {collectionDescription}; the {optionDescription} limit is {maxOptionCount.Value}.");
            }

            var matchedNamesQuery =
                from name in optionNames
                join option in optionCollection on name.lowervalue equals option.Name.ToLower() into options
                from option in options.DefaultIfEmpty()
                orderby name.index
                select (name, option);
            var matchedNames = matchedNamesQuery.ToList();

            // If the option has no matching name, check the options has no existing assignments and then delete it
            var unmatchedOptionsQuery =
                // Get options that don't have a match in the new list
                from existingOption in optionCollection
                join name in optionNames on existingOption.Name.ToLower() equals name.lowervalue into names
                from name in names.DefaultIfEmpty()
                where name == default
                select existingOption;

            existingQuery = existingQuery.Where(e => e != null);

            var unableToDeleteQuery =
                // Join this to all uses of the option
                from option in unmatchedOptionsQuery
                join existingOption in existingQuery
                on option.Id equals existingOption.Id into usedOptions
                from usedOption in usedOptions
                group usedOption by option into options
                select new { option = options.Key, projectCount = options.Count() };

            var unableToDelete = unableToDeleteQuery.ToList();
            if (unableToDelete.Count > 0)
            {
                if (hideHistoric)
                {
                    // Use Order = -1 to hide options
                    foreach(var noDeleteOption in unableToDelete)
                    {
                        noDeleteOption.option.Order = ProjectOptionConstants.HideOrderValue;

                        // Because we are hiding this option, need to add it back into the matched names so it gets added to the collection further down
                        matchedNames.Add(((noDeleteOption.option.Order, noDeleteOption.option.Name, noDeleteOption.option.Name.ToLower()), noDeleteOption.option));
                    }
                }
                else
                {
                    // Can't do this update while the options are assigned to projects
                    Func<string, int, string> categoryError = (n, c) =>
                    {
                        return c == 1 ?
                        $"[{n}] is used as a {optionDescription} ({c} occurrence)" :
                        $"[{n}] is used as a {optionDescription} ({c} occurrences)";
                    };
                    throw new PortfolioConfigurationException($"Can't update {collectionDescription}: {string.Join("; ", unableToDelete.Select(c => categoryError(c.option.Name, c.projectCount)))}");
                }
            }

            var unmatchedOptions = unmatchedOptionsQuery.ToList();
            foreach (var option in unmatchedOptions)
            {
                if (!unableToDelete.Any(o => o.option.ViewKey == option.ViewKey))
                {
                    optionCollection.Remove(option);
                    dbSet.Remove(option);
                }
            }

            // If name has no matching option, add a option
            int viewKeyIndex = 0;

            optionCollection.Clear();
            for (int i = 0; i < matchedNames.Count(); i++)
            {
                var match = matchedNames.ElementAt(i);
                T option = match.option ?? new T();
                option.Name = match.name.value;

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