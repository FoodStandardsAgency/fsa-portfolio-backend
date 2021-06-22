using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Application.Models;
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

            // Get the OptionList labels...
            var optionListLabels = config.Labels
                .Where(l => l.FieldType == PortfolioFieldType.OptionList || l.FieldType == PortfolioFieldType.MultiOptionList || l.FieldType == PortfolioFieldType.PredefinedMultiList)
                .ToList();

            // Get the edit option properties that have a json property name in the OptionList labels...
            var editOptionProperties =
                typeof(ProjectEditOptionsModel)
                .GetProperties()
                .Select(p => new _jsonProperty() { property = p, json = p.GetCustomAttribute<JsonPropertyAttribute>() })
                .Where(p => p.json != null)
                .ToList();

            if (project != null)
            {
                RemoveUnusedEditOptions(options, project, optionListLabels, editOptionProperties);
            }
            else
            {
                RemoveUnusedNewOptions(options, optionListLabels, editOptionProperties);
            }
        }

        private static void RemoveUnusedEditOptions(ProjectEditOptionsModel options, ProjectEditViewModel project, List<PortfolioLabelConfig> optionListLabels, List<_jsonProperty> editOptionProperties)
        {
            // Have to add the existing options for project fields where:
            // - field is a string
            // - field is of type OptionList or MultiOptionList

            // Get all members of the <Project, ProjectEditViewModel> where:
            // - json property of field in ProjectEditViewModel

            var projectEditProperites =
                typeof(ProjectEditViewModel)
                .GetProperties()
                .Select(p => new { property = p, json = p.GetCustomAttribute<JsonPropertyAttribute>() })
                .Where(p => p.json != null)
                .ToList();

            var query = from l in optionListLabels
                        join eop in editOptionProperties on l.FieldName equals eop.json.PropertyName
                        join pep in projectEditProperites on l.FieldName equals pep.json.PropertyName
                        select new _projectProperty() { label = l, editOptionProperty = eop.property, projectEditProperty = pep.property }
                        ;

            foreach (var property in query)
            {
                // Get value from ProjectEditViewModel
                // Work from array of strings so can accommodate subcat
                if (property.projectEditProperty.PropertyType == typeof(string))
                {
                    var projectValue = property.projectEditProperty.GetValue(project) as string;
                    List<DropDownItemModel> propertyOptions = property.editOptionProperty.GetValue(options) as List<DropDownItemModel>;
                    if (propertyOptions != null)
                    {
                        // Might need to add the value to the list 
                        // Note: this could be skipped for IProjectOption collections, but there's no way to tell if this is the case.
                        if (projectValue != null)
                        {
                            if (propertyOptions != null && !propertyOptions.Any(o => o.Value == projectValue))
                            {
                                propertyOptions.Insert(0, LabelDropDownResolver.NewDropDownItem(0, projectValue, projectValue));
                            }
                        }

                        // Now clear out unused hidden options (this does apply to IProjectOption collections!)
                        propertyOptions.RemoveAll(o => o.Order == ProjectOptionConstants.HideOrderValue && projectValue != o.Value);
                    }
                }
                else if (property.projectEditProperty.PropertyType == typeof(string[]))
                {
                    var values = property.projectEditProperty.GetValue(project) as string[];
                    SelectPickerModel propertyOptions = property.editOptionProperty.GetValue(options) as SelectPickerModel;
                    if (propertyOptions != null)
                    {
                        // Note that we don't have to add properties here as this occurs with subcat only: which is an IProjectOption collection.

                        // Now clear out unused hidden options
                        propertyOptions.Items.RemoveAll(o => o.Order == ProjectOptionConstants.HideOrderValue && (values == null || !values.Contains(o.Value)));
                    }
                }

            }
        }
        private static void RemoveUnusedNewOptions(ProjectEditOptionsModel options, List<PortfolioLabelConfig> optionListLabels, List<_jsonProperty> editOptionProperties)
        {
            var query = from l in optionListLabels
                        join eop in editOptionProperties on l.FieldName equals eop.json.PropertyName
                        select new _projectProperty() { label = l, editOptionProperty = eop.property }
                        ;

            foreach (var property in query)
            {
                // Work from array of strings so can accommodate subcat
                List<DropDownItemModel> dropDownOptions = property.editOptionProperty.GetValue(options) as List<DropDownItemModel>;
                if (dropDownOptions != null)
                {
                    dropDownOptions.RemoveAll(o => o.Order == ProjectOptionConstants.HideOrderValue);
                }
                else
                {
                    SelectPickerModel selectPickerOptions = property.editOptionProperty.GetValue(options) as SelectPickerModel;
                    if (selectPickerOptions != null)
                    {
                        selectPickerOptions.Items.RemoveAll(o => o.Order == ProjectOptionConstants.HideOrderValue);
                    }
                }

            }
        }
    }

    class _projectProperty
    {
        internal PropertyInfo editOptionProperty;
        internal PropertyInfo projectEditProperty;
        internal PortfolioLabelConfig label;
    }

    class _jsonProperty
    {
        internal PropertyInfo property;
        internal JsonPropertyAttribute json;
    }
}