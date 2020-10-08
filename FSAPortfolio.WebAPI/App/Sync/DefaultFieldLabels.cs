using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.PostgreSQL.Projects;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public class DefaultFieldLabels
    {
        private PortfolioConfiguration config;

        internal const string FieldGroupName_Ungrouped = "Ungrouped fields";
        internal const string FieldGroupName_ProjectIDs = "Project IDs";
        internal const string FieldGroupName_AboutTheProject = "About the project";
        internal const string FieldGroupName_ProjectTeam = "Project team";
        internal const string FieldGroupName_ProjectPlan = "Project plan";
        internal const string FieldGroupName_ProgressIndicators = "Progress indicators";
        internal const string FieldGroupName_Updates = "Updates";
        internal const string FieldGroupName_Prioritisation = "Prioritisation";
        internal const string FieldGroupName_Budget = "Budget";
        internal const string FieldGroupName_FSAProcesses = "FSA Processes";

        private int fieldOrder = 0;

        public DefaultFieldLabels(PortfolioConfiguration config)
        {
            this.config = config;
        }

        private PortfolioLabelConfig factory(string fieldGroup, string fieldTitle, string fieldName, bool included, bool includedAdminLocked, PortfolioFieldType inputType, bool inputTypeLocked)
        {
            var group = config.LabelGroups.FirstOrDefault(g => g.Name == fieldGroup);
            return new PortfolioLabelConfig() { Configuration_Id = config.Id, Group = group, FieldOrder = fieldOrder++, FieldTitle = fieldTitle, FieldName = fieldName, Included = included, FieldType = inputType, ReadOnly = includedAdminLocked, FieldTypeLocked = inputTypeLocked };
        }

        public PortfolioLabelConfig[] GetDefaultLabels()
        {
            return new PortfolioLabelConfig[]
            {
                //      FieldGroup      FieldTitle     FieldName                    Included IncludeLock FieldType FieldTypeLocked
                factory(FieldGroupName_ProjectIDs, "Project ID", nameof(ProjectModel.project_id), true, true, PortfolioFieldType.Auto, true),
                factory(FieldGroupName_ProjectIDs, "Business Case Number", nameof(ProjectModel.business_case_number), false, false, PortfolioFieldType.FreeText, false),
                factory(FieldGroupName_ProjectIDs, "FS Number", nameof(ProjectModel.fs_number), false, false, PortfolioFieldType.FreeText, false),

                factory(FieldGroupName_AboutTheProject, "Project name", nameof(ProjectModel.project_name), true, true, PortfolioFieldType.FreeText, true),
                factory(FieldGroupName_AboutTheProject, "Short description", nameof(ProjectModel.short_desc), false, false, PortfolioFieldType.FreeText, false),
                factory(FieldGroupName_AboutTheProject, "Risk rating", nameof(ProjectModel.risk_rating), false, false, PortfolioFieldType.OptionList, false),
                factory(FieldGroupName_AboutTheProject, "Theme", nameof(ProjectModel.theme), false, false, PortfolioFieldType.OptionList, false),
                factory(FieldGroupName_AboutTheProject, "Project type", nameof(ProjectModel.project_type), false, false, PortfolioFieldType.OptionList, false),
                factory(FieldGroupName_AboutTheProject, "Project size", nameof(ProjectModel.project_size), false, false, PortfolioFieldType.OptionList, false),
                factory(FieldGroupName_AboutTheProject, "Category", nameof(ProjectModel.category), false, false, PortfolioFieldType.OptionList, false),
                factory(FieldGroupName_AboutTheProject, "Secondary category", nameof(ProjectModel.subcat), false, false, PortfolioFieldType.OptionList, false),
                factory(FieldGroupName_AboutTheProject, "Directorate", nameof(ProjectModel.direct), false, false, PortfolioFieldType.PredefinedList, true),
                factory(FieldGroupName_AboutTheProject, "Strategic objectives", nameof(ProjectModel.strategic_objectives), false, false, PortfolioFieldType.PredefinedList, true),
                factory(FieldGroupName_AboutTheProject, "Programme", nameof(ProjectModel.programme), false, false, PortfolioFieldType.OptionList, false),
                factory(FieldGroupName_AboutTheProject, "Programme description", nameof(ProjectModel.programme_description), false, false, PortfolioFieldType.FreeText, true),
                factory(FieldGroupName_AboutTheProject, "Project channel (link)", nameof(ProjectModel.link), false, false, PortfolioFieldType.FreeText, true),
                factory(FieldGroupName_AboutTheProject, "Related projects", nameof(ProjectModel.rels), false, false, PortfolioFieldType.PredefinedList, true),
                factory(FieldGroupName_AboutTheProject, "Dependencies", nameof(ProjectModel.dependencies), false, false, PortfolioFieldType.PredefinedList, true),
                factory(FieldGroupName_AboutTheProject, "Key documents", nameof(ProjectModel.documents), false, false, PortfolioFieldType.PredefinedList, true),

                factory(FieldGroupName_ProjectTeam, "Project lead", nameof(ProjectModel.oddlead), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_ProjectTeam, "Lead role", nameof(ProjectModel.oddlead_role), false, false, PortfolioFieldType.OptionList, true),
                factory(FieldGroupName_ProjectTeam, "Lead team", nameof(ProjectModel.lead_team), false, false, PortfolioFieldType.OptionList, true),
                factory(FieldGroupName_ProjectTeam, "Key contact 1", nameof(ProjectModel.key_contact1), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_ProjectTeam, "Key contact 2", nameof(ProjectModel.key_contact2), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_ProjectTeam, "Key contact 3", nameof(ProjectModel.key_contact3), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_ProjectTeam, "Supplier", nameof(ProjectModel.supplier), false, false, PortfolioFieldType.FreeText, true),
                factory(FieldGroupName_ProjectTeam, "Project team", nameof(ProjectModel.team), false, false, PortfolioFieldType.PredefinedField, true),

                factory(FieldGroupName_ProjectPlan, "Intended start date", nameof(ProjectModel.start_date), false, false, PortfolioFieldType.Date, true),
                factory(FieldGroupName_ProjectPlan, "Actual start date", nameof(ProjectModel.actstart), false, false, PortfolioFieldType.Date, true),
                factory(FieldGroupName_ProjectPlan, "Expected current phase end date", nameof(ProjectModel.expendp), false, false, PortfolioFieldType.Date, true),
                factory(FieldGroupName_ProjectPlan, "Expected end date", nameof(ProjectModel.expend), false, false, PortfolioFieldType.Date, true),
                factory(FieldGroupName_ProjectPlan, "Actual end date", nameof(ProjectModel.actual_end_date), false, false, PortfolioFieldType.Date, true),
                factory(FieldGroupName_ProjectPlan, "Hard deadline", nameof(ProjectModel.hardend), false, false, PortfolioFieldType.Date, true),
                factory(FieldGroupName_ProjectPlan, "Percentage completed", nameof(ProjectModel.p_comp), false, false, PortfolioFieldType.Percentage, true),
                factory(FieldGroupName_ProjectPlan, "Milesones", nameof(ProjectModel.milestones), false, false, PortfolioFieldType.PredefinedField, true),

                factory(FieldGroupName_ProgressIndicators, "Phase", nameof(ProjectModel.phase), true, true, PortfolioFieldType.OptionList, false),
                factory(FieldGroupName_ProgressIndicators, "RAG", nameof(ProjectModel.rag), true, true, PortfolioFieldType.RAGChoice, false),
                factory(FieldGroupName_ProgressIndicators, "How to get to green", nameof(ProjectModel.how_get_green), false, false, PortfolioFieldType.FreeText, true),
                factory(FieldGroupName_ProgressIndicators, "Status", nameof(ProjectModel.progress_status), false, false, PortfolioFieldType.OptionList, false),

                //      FieldGroup      FieldTitle     FieldName                    Included Readonly FieldType FieldTypeLocked
                factory(FieldGroupName_Updates, "Updates", nameof(ProjectModel.update), true, true, PortfolioFieldType.FreeText, true),
                factory(FieldGroupName_Updates, "Forward look", nameof(ProjectModel.forward_look), false, false, PortfolioFieldType.FreeText, true),
                factory(FieldGroupName_Updates, "Emerging issues, risks", nameof(ProjectModel.emerging_issues), false, false, PortfolioFieldType.FreeText, true),

                factory(FieldGroupName_Prioritisation, "Priority score", nameof(ProjectModel.priority_main), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_Prioritisation, "Funded", nameof(ProjectModel.funded), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_Prioritisation, "Confidence in delivery", nameof(ProjectModel.confidence), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_Prioritisation, "Priorities impacted", nameof(ProjectModel.priorities), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_Prioritisation, "Benefits", nameof(ProjectModel.benefits), false, false, PortfolioFieldType.PredefinedField, true),
                factory(FieldGroupName_Prioritisation, "Criticality", nameof(ProjectModel.criticality), false, false, PortfolioFieldType.PredefinedField, true),

                factory(FieldGroupName_Budget, "Budget category", nameof(ProjectModel.budgettype), false, false, PortfolioFieldType.OptionList, true),
                factory(FieldGroupName_Budget, "Budget amount", nameof(ProjectModel.budget), false, false, PortfolioFieldType.Budget, true),
                factory(FieldGroupName_Budget, "Amount spent", nameof(ProjectModel.spent), false, false, PortfolioFieldType.Budget, true),
                factory(FieldGroupName_Budget, "Forecast spend at completion", nameof(ProjectModel.forecast_spend), false, false, PortfolioFieldType.Budget, true),
                factory(FieldGroupName_Budget, "Budget field 1", nameof(ProjectModel.budget_field1), false, false, PortfolioFieldType.Budget, true),
                factory(FieldGroupName_Budget, "Cost centre", nameof(ProjectModel.cost_centre), false, false, PortfolioFieldType.FreeText, true),

                factory(FieldGroupName_FSAProcesses, "Assurance gate number", nameof(ProjectModel.fsaproc_assurance_gatenumber), false, false, PortfolioFieldType.FreeText, true),
                factory(FieldGroupName_FSAProcesses, "Assurance gate completed", nameof(ProjectModel.fsaproc_assurance_gatecompleted), false, false, PortfolioFieldType.Date, true),
                factory(FieldGroupName_FSAProcesses, "Next gate", nameof(ProjectModel.fsaproc_assurance_nextgate), false, false, PortfolioFieldType.FreeText, true),
            };
        }
    }
}