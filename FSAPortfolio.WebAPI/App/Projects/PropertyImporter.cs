using AutoMapper;
using CsvHelper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.App.Mapping;
using FSAPortfolio.WebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Projects
{
    internal class PropertyImporter
    {
        internal async Task<List<ProjectUpdateModel>> ImportProjectsAsync(MultipartFormDataStreamProvider files, PortfolioConfiguration config, ProjectEditOptionsModel options)
        {
            var projects = new List<ProjectUpdateModel>();
            var optionsProperties = typeof(ProjectEditOptionsModel).GetProperties().Select(p => new _optionsPropertyMap(p, options)).ToList(); ;
            var projectProperties = typeof(ProjectUpdateModel).GetProperties().Select(p => new _projectPropertyMap(p, config)).ToList();

            foreach (var file in files.FileData)
            {
                using (var reader = new StreamReader(file.LocalFileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    if (await csv.ReadAsync())
                    {
                        if (csv.ReadHeader())
                        {
                            // Go through the header and get the properties used in the import
                            List<_projectPropertyMap> headerProperties = new List<_projectPropertyMap>();
                            foreach (var h in csv.HeaderRecord)
                            {
                                var property = projectProperties.SingleOrDefault(p => h == p.Name);
                                if (property != null)
                                {
                                    property.optionsProperty = optionsProperties.SingleOrDefault(op => op.JsonPropertyName == property.JsonPropertyName);
                                    headerProperties.Add(property);
                                }
                            }

                            // Read in the project records
                            while (await csv.ReadAsync())
                            {
                                var project = new ProjectUpdateModel();
                                foreach (var property in headerProperties)
                                {
                                    MapProperty(csv, project, property);
                                }
                                projects.Add(project);
                            }
                        }
                    }
                }
            }
            return projects;
        }

        private static void MapProperty(CsvReader csv, ProjectUpdateModel project, _projectPropertyMap property)
        {
            // Get the property value and translate it if necessary (using the edit options)
            var value = property.Translate(csv.GetField(property.Name));
            try
            {
                object objValue;

                // For some reason, have to treat floats differently in automapper. This might be a bug in automapper.
                if (property.property.PropertyType.IsAssignableFrom(typeof(float?)))
                {
                    objValue = PortfolioMapper.ExportMapper.Map<float?>(value);
                }
                else
                {
                    objValue = PortfolioMapper.ExportMapper.Map(value, typeof(string), property.property.PropertyType);
                }

                // Set the property value on the project
                property.property.SetValue(project, objValue);
            }
            catch(AutoMapperMappingException ame)
            {
                string msg = $"Error mapping property [${property.property.Name}] of type [${property.property.PropertyType.Name}], value is [${value}]";
                throw new Exception(msg, ame);
            }
        }
    }

    internal class _propertyMap
    {
        internal PropertyInfo property;
        internal JsonPropertyAttribute json;

        internal _propertyMap(PropertyInfo property)
        {
            this.property = property;
            this.json = property.GetCustomAttribute<JsonPropertyAttribute>();
        }
        internal string JsonPropertyName => json?.PropertyName ?? property.Name; // If there is no label, use the json attribute, or property name
        internal virtual string Name => JsonPropertyName;
        internal virtual string Translate(string value) => value;
    }

    internal class _projectPropertyMap : _propertyMap
    {
        internal PortfolioLabelConfig label;
        internal _optionsPropertyMap optionsProperty;
        internal _projectPropertyMap(PropertyInfo property, PortfolioConfiguration config) : base(property)
        {
            this.property = property;
            this.json = property.GetCustomAttribute<JsonPropertyAttribute>();
            this.label = config.Labels.SingleOrDefault(l => l.FieldName == (json?.PropertyName ?? property.Name));
        }

        internal override string Name => label == null ? base.Name : (label.Label ?? label.FieldTitle); // If there is a label - use the label, or field title
        internal override string Translate(string value) => optionsProperty?.Translate(value) ?? value;

    }
    internal class _optionsPropertyMap : _propertyMap
    {
        internal object optionValue;
        private List<DropDownItemModel> dropDownItems;
        private SelectPickerModel selectPicker;

        internal _optionsPropertyMap(PropertyInfo property, ProjectEditOptionsModel options) : base(property)
        {
            this.property = property;
            this.json = property.GetCustomAttribute<JsonPropertyAttribute>();
            this.optionValue = this.property.GetValue(options);
            this.dropDownItems = this.optionValue as List<DropDownItemModel>;
            this.selectPicker = this.optionValue as SelectPickerModel;

        }

        internal override string Translate(string value)
        {
            if (dropDownItems != null)
            {
                value = dropDownItems.SingleOrDefault(i => i.Display == value)?.Value;
            }
            else if (selectPicker != null)
            {
                value = selectPicker.Items.SingleOrDefault(i => i.Display == value)?.Value;
            }

            return value;
        }
    }


}