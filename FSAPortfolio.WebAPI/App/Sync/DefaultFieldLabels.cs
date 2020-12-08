using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static FSAPortfolio.Entities.Organisation.PortfolioFieldFlags;
using static FSAPortfolio.WebAPI.App.FieldGroupConstants;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public class DefaultFieldLabels
    {
        private const bool IncludeAll = true;

        private PortfolioConfiguration config;

        private static string priorityOptions = string.Join(", ", Enumerable.Range(0, 21));
        private static string fundedOptions = string.Join(", ", Enumerable.Range(0, 5));



        private int fieldOrder = 0;

        public DefaultFieldLabels(PortfolioConfiguration config)
        {
            this.config = config;
        }

        private PortfolioLabelConfig Factory(string fieldGroup, string fieldTitle, string fieldName,
                                             bool included, bool includedLock, bool adminLock,
                                             PortfolioFieldType inputType, PortfolioFieldFlags flags = DefaultCRUD, string options = null)
        {
            bool inputTypeLocked;
            switch(inputType)
            {
                case PortfolioFieldType.None:
                case PortfolioFieldType.Auto:
                case PortfolioFieldType.FreeText:
                case PortfolioFieldType.PredefinedList:
                case PortfolioFieldType.PredefinedSearchableList:
                case PortfolioFieldType.PredefinedField:
                case PortfolioFieldType.Date:
                case PortfolioFieldType.ProjectDate:
                case PortfolioFieldType.NullableBoolean:
                case PortfolioFieldType.Percentage:
                case PortfolioFieldType.Budget:
                case PortfolioFieldType.SmallFreeTextArea:
                case PortfolioFieldType.MediumFreeTextArea:
                case PortfolioFieldType.LargeFreeTextArea:
                case PortfolioFieldType.ProjectMultiSelect:
                case PortfolioFieldType.PredefinedMultiList:
                case PortfolioFieldType.NamedLink:
                case PortfolioFieldType.LinkedItemList:
                case PortfolioFieldType.ProjectUpdateText:
                case PortfolioFieldType.ADUserSearch:
                case PortfolioFieldType.ADUserMultiSearch:
                case PortfolioFieldType.Milestones:
                case PortfolioFieldType.AjaxMultiSearch:
                    inputTypeLocked = true;
                    break;
                case PortfolioFieldType.OptionList:
                case PortfolioFieldType.RAGChoice:
                case PortfolioFieldType.PhaseChoice:
                case PortfolioFieldType.MultiOptionList:
                    inputTypeLocked = false;
                    break;
                default:
                    throw new ArgumentException("Not recognised", nameof(inputType));
            }

            if ((flags & DefaultCRUD) == 0) flags |= DefaultCRUD;
            if (fieldGroup == FieldGroupName_Budget) flags |= FSAOnly;

            var group = config.LabelGroups.FirstOrDefault(g => g.Name == fieldGroup);
            var label = config.Labels.FirstOrDefault(l => l.FieldName == fieldName) ?? new PortfolioLabelConfig();
            label.Configuration_Id = config.Portfolio_Id;
            label.Group = group;
            label.FieldOrder = fieldOrder++;
            label.FieldTitle = fieldTitle;
            label.FieldName = fieldName;
            label.FieldType = inputType;
            label.IncludedLock = includedLock;
            label.AdminOnlyLock = adminLock;
            label.FieldTypeLocked = inputTypeLocked;

            // Clear flags to leave alone
            var flagsToLeaveAsIs = FilterProject;
            flags = (flags & ~FilterProject);
            
            // Set to the current values 
            flags |= (label.Flags & FilterProject);

            label.Flags = flags;

            return label;
        }

        private void SetMasterLabel(PortfolioLabelConfig[] labels, string slaveFieldName, string masterFieldName)
        {
            labels.Single(l => l.FieldName == slaveFieldName).MasterLabel = labels.Single(l => l.FieldName == masterFieldName);
        }

        public PortfolioLabelConfig[] GetDefaultLabels()
        {
            var labels = new PortfolioLabelConfig[]
            {
                //      FieldGroup      FieldTitle     FieldName                    Included IncludeLock AdmLock FieldType FieldTypeLocked
                Factory(FieldGroupName_ProjectIDs, "Project ID", ProjectPropertyConstants.ProjectId, true, true, true, PortfolioFieldType.Auto),
                Factory(FieldGroupName_ProjectIDs, "Business Case Number", ProjectPropertyConstants.business_case_number, true, false, false, PortfolioFieldType.FreeText, flags: EditorCanView),
                Factory(FieldGroupName_ProjectIDs, "FS Number", ProjectPropertyConstants.fs_number, false, false, false, PortfolioFieldType.FreeText, flags: EditorCanView),

                Factory(FieldGroupName_AboutTheProject, "Project title", nameof(ProjectModel.project_name), true, true, false, PortfolioFieldType.FreeText, flags: EditorCanView|Filterable|FilterProject|Required),
                Factory(FieldGroupName_AboutTheProject, "Short description", nameof(ProjectModel.short_desc), true, false, false, PortfolioFieldType.MediumFreeTextArea, flags: EditorCanView),
                Factory(FieldGroupName_AboutTheProject, "Risk rating", ProjectPropertyConstants.risk_rating, false, false, false, PortfolioFieldType.OptionList),
                Factory(FieldGroupName_AboutTheProject, "Theme", nameof(ProjectModel.theme), true, false, false, PortfolioFieldType.OptionList, flags: Filterable|FilterProject, options: "Theme1, Theme2, Theme3"),
                Factory(FieldGroupName_AboutTheProject, "Project type", nameof(ProjectModel.project_type), false, false, false, PortfolioFieldType.OptionList, flags: Filterable|FilterProject),
                Factory(FieldGroupName_AboutTheProject, "Project size", nameof(ProjectModel.project_size), false, false, false, PortfolioFieldType.OptionList),
                Factory(FieldGroupName_AboutTheProject, "Category", nameof(ProjectModel.category), true, false, false, PortfolioFieldType.OptionList, flags: Filterable|FilterProject),
                Factory(FieldGroupName_AboutTheProject, "Secondary category", nameof(ProjectModel.subcat), true, false, false, PortfolioFieldType.PredefinedMultiList), // Uses the same values as category
                Factory(FieldGroupName_AboutTheProject, "Directorate", nameof(ProjectModel.direct), true, false, false, PortfolioFieldType.PredefinedList, flags: Filterable|FilterProject),
                Factory(FieldGroupName_AboutTheProject, "Strategic objectives", nameof(ProjectModel.strategic_objectives), false, false, false, PortfolioFieldType.PredefinedList, flags: Filterable|FilterProject),
                Factory(FieldGroupName_AboutTheProject, "Programme", nameof(ProjectModel.programme), false, false, false, PortfolioFieldType.OptionList, flags: EditorCanView|Filterable|FilterProject),
                Factory(FieldGroupName_AboutTheProject, "Programme description", ProjectPropertyConstants.programme_description, false, false, false, PortfolioFieldType.MediumFreeTextArea, flags: EditorCanView),
                Factory(FieldGroupName_AboutTheProject, "Project channel (link)", ProjectPropertyConstants.link, true, false, false, PortfolioFieldType.NamedLink),
                Factory(FieldGroupName_AboutTheProject, "Related projects", nameof(ProjectEditViewModel.rels), true, false, false, PortfolioFieldType.AjaxMultiSearch),
                Factory(FieldGroupName_AboutTheProject, "Dependencies", nameof(ProjectEditViewModel.dependencies), false, false, false, PortfolioFieldType.AjaxMultiSearch),
                Factory(FieldGroupName_AboutTheProject, "Key documents", nameof(ProjectModel.documents), true, false, false, PortfolioFieldType.LinkedItemList),

                Factory(FieldGroupName_ProjectTeam, "Project lead", ProjectPropertyConstants.ProjectLead, false, false, false, PortfolioFieldType.ADUserSearch, flags: Filterable|FilterProject),
                Factory(FieldGroupName_ProjectTeam, "Lead role", ProjectPropertyConstants.oddlead_role, false, false, false, PortfolioFieldType.OptionList, options: "Role, Support, Implementation, Management"),
                Factory(FieldGroupName_ProjectTeam, "Lead team", ProjectPropertyConstants.g6team, false, false, false, PortfolioFieldType.Auto, flags: Read),
                Factory(FieldGroupName_ProjectTeam, "Key contact 1", nameof(ProjectModel.key_contact1), false, false, false, PortfolioFieldType.ADUserSearch),
                Factory(FieldGroupName_ProjectTeam, "Key contact 2", nameof(ProjectModel.key_contact2), false, false, false, PortfolioFieldType.ADUserSearch),
                Factory(FieldGroupName_ProjectTeam, "Key contact 3", nameof(ProjectModel.key_contact3), false, false, false, PortfolioFieldType.ADUserSearch),
                Factory(FieldGroupName_ProjectTeam, "Supplier", ProjectPropertyConstants.Supplier, false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_ProjectTeam, "Project team", nameof(ProjectEditViewModel.team), false, false, false, PortfolioFieldType.ADUserMultiSearch  ),
                Factory(FieldGroupName_ProjectTeam, $"{FieldGroupName_ProjectTeam} setting 1", nameof(ProjectModel.project_team_setting1), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_ProjectTeam, $"{FieldGroupName_ProjectTeam} setting 2", nameof(ProjectModel.project_team_setting2), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_ProjectTeam, $"{FieldGroupName_ProjectTeam} option 1", nameof(ProjectModel.project_team_option1), false, false, false, PortfolioFieldType.OptionList),
                Factory(FieldGroupName_ProjectTeam, $"{FieldGroupName_ProjectTeam} option 2", nameof(ProjectModel.project_team_option2), false, false, false, PortfolioFieldType.OptionList),

                Factory(FieldGroupName_ProjectPlan, "Intended start date", ProjectPropertyConstants.IntendedStartDate, false, false, false, PortfolioFieldType.ProjectDate),
                Factory(FieldGroupName_ProjectPlan, "Actual start date", ProjectPropertyConstants.ActualStartDate, false, false, false, PortfolioFieldType.ProjectDate),
                Factory(FieldGroupName_ProjectPlan, "Expected current phase end date", ProjectPropertyConstants.ExpectedCurrentPhaseEndDate, false, false, false, PortfolioFieldType.ProjectDate),
                Factory(FieldGroupName_ProjectPlan, "Expected end date", ProjectPropertyConstants.ExpectedEndDate, false, false, false, PortfolioFieldType.ProjectDate),
                Factory(FieldGroupName_ProjectPlan, "Actual end date", ProjectPropertyConstants.ActualEndDate, false, false, false, PortfolioFieldType.ProjectDate),
                Factory(FieldGroupName_ProjectPlan, "Hard deadline", ProjectPropertyConstants.HardDeadline, false, false, false, PortfolioFieldType.ProjectDate),
                Factory(FieldGroupName_ProjectPlan, "Percentage completed", nameof(ProjectModel.p_comp), false, false, false, PortfolioFieldType.Percentage),
                Factory(FieldGroupName_ProjectPlan, "Milestones", ProjectPropertyConstants.milestones, false, false, false, PortfolioFieldType.Milestones),
                Factory(FieldGroupName_ProjectPlan, $"{FieldGroupName_ProjectPlan} setting 1", nameof(ProjectModel.project_plan_setting1), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_ProjectPlan, $"{FieldGroupName_ProjectPlan} setting 2", nameof(ProjectModel.project_plan_setting2), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_ProjectPlan, $"{FieldGroupName_ProjectPlan} option 1", nameof(ProjectModel.project_plan_option1), false, false, false, PortfolioFieldType.OptionList),
                Factory(FieldGroupName_ProjectPlan, $"{FieldGroupName_ProjectPlan} option 2", nameof(ProjectModel.project_plan_option2), false, false, false, PortfolioFieldType.OptionList),

                Factory(FieldGroupName_ProgressIndicators, "Phase", nameof(ProjectModel.phase), true, true, false, PortfolioFieldType.PhaseChoice, flags: Filterable|FilterProject),
                Factory(FieldGroupName_ProgressIndicators, "RAG", nameof(ProjectModel.rag), true, false, false, PortfolioFieldType.RAGChoice, flags: Filterable|FilterProject),
                Factory(FieldGroupName_ProgressIndicators, "How to get to green", nameof(ProjectModel.how_get_green), false, false, false, PortfolioFieldType.SmallFreeTextArea),
                Factory(FieldGroupName_ProgressIndicators, "Status", nameof(ProjectModel.onhold), false, false, false, PortfolioFieldType.OptionList, flags: Filterable|FilterProject),
                Factory(FieldGroupName_ProgressIndicators, $"{FieldGroupName_ProgressIndicators} setting 1", nameof(ProjectModel.progress_setting1), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_ProgressIndicators, $"{FieldGroupName_ProgressIndicators} setting 2", nameof(ProjectModel.progress_setting2), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_ProgressIndicators, $"{FieldGroupName_ProgressIndicators} option 1", nameof(ProjectModel.progress_option1), false, false, false, PortfolioFieldType.OptionList),
                Factory(FieldGroupName_ProgressIndicators, $"{FieldGroupName_ProgressIndicators} option 2", nameof(ProjectModel.progress_option2), false, false, false, PortfolioFieldType.OptionList),

                //      FieldGroup      FieldTitle     FieldName                    Included IncludeLock AdmLock FieldType FieldTypeLocked
                Factory(FieldGroupName_Updates, "Update", nameof(ProjectModel.update), true, false, false, PortfolioFieldType.ProjectUpdateText),
                Factory(FieldGroupName_Updates, "Forward look", nameof(ProjectModel.forward_look), false, false, false, PortfolioFieldType.LargeFreeTextArea),
                Factory(FieldGroupName_Updates, "Emerging issues, risks", nameof(ProjectModel.emerging_issues), false, false, false, PortfolioFieldType.LargeFreeTextArea),

                Factory(FieldGroupName_Prioritisation, "Priority score", nameof(ProjectModel.priority_main), false, false, false, PortfolioFieldType.PredefinedList, flags: FilterRequired, options: priorityOptions),
                Factory(FieldGroupName_Prioritisation, "Priority group", ProjectPropertyConstants.pgroup, false, false, false, PortfolioFieldType.Auto, flags: PortfolioFieldFlags.Read),
                Factory(FieldGroupName_Prioritisation, "Funded", nameof(ProjectModel.funded), false, false, false, PortfolioFieldType.PredefinedList, options: fundedOptions),
                Factory(FieldGroupName_Prioritisation, "Confidence in delivery", nameof(ProjectModel.confidence), false, false, false, PortfolioFieldType.PredefinedList, options: fundedOptions),
                Factory(FieldGroupName_Prioritisation, "Priorities impacted", nameof(ProjectModel.priorities), false, false, false, PortfolioFieldType.PredefinedList, options: fundedOptions),
                Factory(FieldGroupName_Prioritisation, "Benefits", nameof(ProjectModel.benefits), false, false, false, PortfolioFieldType.PredefinedList, options: fundedOptions),
                Factory(FieldGroupName_Prioritisation, "Criticality", nameof(ProjectModel.criticality), false, false, false, PortfolioFieldType.PredefinedList, options: fundedOptions),

                Factory(FieldGroupName_Budget, "Budget category", nameof(ProjectModel.budgettype), false, false, false, PortfolioFieldType.OptionList),
                Factory(FieldGroupName_Budget, "Budget amount", nameof(ProjectModel.budget), false, false, false, PortfolioFieldType.Budget),
                Factory(FieldGroupName_Budget, "Amount spent", nameof(ProjectModel.spent), false, false, false, PortfolioFieldType.Budget),
                Factory(FieldGroupName_Budget, "Forecast spend at completion", nameof(ProjectModel.forecast_spend), false, false, false, PortfolioFieldType.Budget),
                Factory(FieldGroupName_Budget, "Cost centre", nameof(ProjectModel.cost_centre), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_Budget, $"{FieldGroupName_Budget} field 1", nameof(ProjectModel.budget_field1), false, false, false, PortfolioFieldType.Budget),
                Factory(FieldGroupName_Budget, $"{FieldGroupName_Budget} field 2", nameof(ProjectModel.budget_field2), false, false, false, PortfolioFieldType.Budget),
                Factory(FieldGroupName_Budget, $"{FieldGroupName_Budget} option 1", nameof(ProjectModel.budget_option1), false, false, false, PortfolioFieldType.OptionList),
                Factory(FieldGroupName_Budget, $"{FieldGroupName_Budget} option 2", nameof(ProjectModel.budget_option2), false, false, false, PortfolioFieldType.OptionList),

                Factory(FieldGroupName_FSAProcesses, "Assurance gate number", nameof(ProjectModel.fsaproc_assurance_gatenumber), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_FSAProcesses, "Assurance gate completed", ProjectPropertyConstants.fsaproc_assurance_gatecompleted, false, false, false, PortfolioFieldType.ProjectDate),
                Factory(FieldGroupName_FSAProcesses, "Next gate", nameof(ProjectModel.fsaproc_assurance_nextgate), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_FSAProcesses, $"{FieldGroupName_FSAProcesses} setting 1", nameof(ProjectModel.processes_setting1), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_FSAProcesses, $"{FieldGroupName_FSAProcesses} setting 2", nameof(ProjectModel.processes_setting2), false, false, false, PortfolioFieldType.FreeText),
                Factory(FieldGroupName_FSAProcesses, $"{FieldGroupName_FSAProcesses} option 1", nameof(ProjectModel.processes_option1), false, false, false, PortfolioFieldType.OptionList),
                Factory(FieldGroupName_FSAProcesses, $"{FieldGroupName_FSAProcesses} option 2", nameof(ProjectModel.processes_option2), false, false, false, PortfolioFieldType.OptionList),
            };

            SetMasterLabel(labels, nameof(ProjectModel.subcat), nameof(ProjectModel.category));
            SetMasterLabel(labels, nameof(ProjectModel.programme_description), nameof(ProjectModel.programme));
            SetMasterLabel(labels, ProjectPropertyConstants.oddlead_role, ProjectPropertyConstants.ProjectLead);
            SetMasterLabel(labels, ProjectPropertyConstants.g6team, ProjectPropertyConstants.ProjectLead);
            SetMasterLabel(labels, nameof(ProjectModel.how_get_green), nameof(ProjectModel.rag));

            return labels;
        }
    }
}