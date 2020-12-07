using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects
{
    public class ProjectEditOptionsManualMaps
    {
        internal static async Task MapAsync(PortfolioContext context, PortfolioConfiguration config, ProjectEditOptionsModel options, ProjectEditViewModel project = null)
        {
            var directorates = await context.Directorates.OrderBy(d => d.Order).ToListAsync();
            var directorateItems = PortfolioMapper.ProjectMapper.Map<List<DropDownItemModel>>(directorates);
            directorateItems.Insert(0, new DropDownItemModel() { Display = "None", Value = "", Order = 0 });
            options.Directorates = directorateItems;

            var teams = PortfolioMapper.ProjectMapper.Map<List<DropDownItemModel>>(config.Portfolio.Teams.OrderBy(d => d.Order));
            options.G6Team = teams;

            if (project != null)
            {
                // Have to add the existing options for project fields where:
                // - field is a string
                // - field is of type OptionList or MultiOptionList

                // Get all members of the <Project, ProjectEditViewModel> where:
                // - json property of field in ProjectEditViewModel

                // Get the OptionList labels...
                var optionListLabels = config.Labels
                    .Where(l => l.FieldType == PortfolioFieldType.OptionList || l.FieldType == PortfolioFieldType.MultiOptionList)
                    .ToList();

                // Get the edit option properties that have a json property name in the OptionList labels...
                var editOptionProperties =
                    typeof(ProjectEditOptionsModel)
                    .GetProperties()
                    .Select(p => new { property = p, json = p.GetCustomAttribute<JsonPropertyAttribute>() })
                    .Where(p => p.json != null)
                    .ToList();

                var projectEditProperites =
                    typeof(ProjectEditViewModel)
                    .GetProperties()
                    .Select(p => new { property = p, json = p.GetCustomAttribute<JsonPropertyAttribute>() })
                    .Where(p => p.json != null)
                    .ToList();

                var query = from l in optionListLabels
                            join eop in editOptionProperties on l.FieldName equals eop.json.PropertyName
                            join pep in projectEditProperites on l.FieldName equals pep.json.PropertyName
                            select new Stuff() { label = l, editOptionProperty = eop.property, projectEditProperty = pep.property }
                            ;

                foreach (var property in query)
                {
                    // Get value from ProjectEditViewModel
                    string projectValue = property.projectEditProperty.GetValue(project) as string;
                    if (!string.IsNullOrWhiteSpace(projectValue))
                    {
                        List<DropDownItemModel> propertyOptions = property.editOptionProperty.GetValue(options) as List<DropDownItemModel>;
                        if (propertyOptions != null && !propertyOptions.Any(o => o.Value == projectValue))
                        {
                            propertyOptions.Insert(0, LabelDropDownResolver.NewDropDownItem(0, projectValue, projectValue));
                        }
                    }
                }

            }
        }
    }

    class Stuff
    {
        internal PropertyInfo editOptionProperty;
        internal PropertyInfo projectEditProperty;
        internal PortfolioLabelConfig label;

    }
}