using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public class DefaultFieldLabels
    {
        public static IEnumerable<PortfolioLabelConfig> GetDefaultLabels(int configuration_Id)
        {
            return new PortfolioLabelConfig[]
            {
                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "Project IDs", FieldTitle = "Project ID", FieldName = "project_id", Included = true, AdminOnly = false, FieldType = PortfolioFieldType.Auto, ReadOnly = true, FieldTypeLocked = true },
                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "Project IDs", FieldTitle = "Business Case Number", FieldName = "business_case_number", Included = false, AdminOnly = false, FieldType = PortfolioFieldType.FreeText, FieldTypeLocked = true },
                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "Project IDs", FieldTitle = "FS Number", FieldName = "fs_number", Included = false, AdminOnly = false, FieldType = PortfolioFieldType.FreeText, FieldTypeLocked = true },

                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "About the project", FieldTitle = "Project name", FieldName = "project_name", Included = true, AdminOnly = false, FieldType = PortfolioFieldType.FreeText, ReadOnly = true, FieldTypeLocked = true },
                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "About the project", FieldTitle = "Short description", FieldName = "short_desc", Included = false, AdminOnly = false, FieldType = PortfolioFieldType.FreeText, FieldTypeLocked = true },
                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "About the project", FieldTitle = "Risk rating", FieldName = "risk_rating", Included = false, AdminOnly = false, FieldType = PortfolioFieldType.OptionList, FieldTypeLocked = true },
                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "About the project", FieldTitle = "Theme", FieldName = "theme", Included = false, AdminOnly = false, FieldType = PortfolioFieldType.OptionList },
                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "About the project", FieldTitle = "Project type", FieldName = "project_type", Included = false, AdminOnly = false, FieldType = PortfolioFieldType.OptionList },
                new PortfolioLabelConfig(){ Configuration_Id = configuration_Id, FieldGroup = "About the project", FieldTitle = "Project size", FieldName = "project_size", Included = false, AdminOnly = false, FieldType = PortfolioFieldType.OptionList },

            };
        }
    }
}