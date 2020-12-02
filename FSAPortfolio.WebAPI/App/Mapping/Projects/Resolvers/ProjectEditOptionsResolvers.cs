using AutoMapper;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Mapping.Projects.Resolvers
{
    public class ProjectDataOptionsOutboundMapper : IMappingAction<PortfolioConfiguration, ProjectEditOptionsModel>
    {
        public void Process(PortfolioConfiguration source, ProjectEditOptionsModel destination, ResolutionContext context)
        {
            List<ProjectDataOptionsModel> options = new List<ProjectDataOptionsModel>();

            // Unmodelled properties
            var unmodelledPropertiesQuery = from l in source.Labels
                                            where l.Included && l.Flags.HasFlag(PortfolioFieldFlags.NotModelled)
                                            select l;

            foreach (var label in unmodelledPropertiesQuery)
            {
                switch (label.FieldType)
                {
                    case PortfolioFieldType.OptionList:
                        var dropDownOptions = new ProjectDataOptionsModel() { FieldName = label.FieldName };
                        dropDownOptions.Options = new LabelDropDownResolver(label.FieldName).Resolve(source, destination, null, context);
                        options.Add(dropDownOptions);
                        break;
                    case PortfolioFieldType.MultiOptionList:
                        var selectPickerOptions = new ProjectDataOptionsModel() { FieldName = label.FieldName }; 
                        selectPickerOptions.Options = new SelectPickerResolver(label.FieldName, $"Select from the list...");
                        options.Add(selectPickerOptions);
                        break;
                }
            }
            destination.ProjectDataOptions = options;
        }
    }
}