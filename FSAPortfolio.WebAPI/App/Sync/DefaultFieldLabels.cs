﻿using FSAPortfolio.Entities.Organisation;
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

        private PortfolioLabelConfig Factory(string fieldGroup, string fieldTitle, string fieldName, 
                                             bool included, bool includedLock, bool adminLock, 
                                             PortfolioFieldType inputType, bool inputTypeLocked,
                                             PortfolioFieldFlags flags = PortfolioFieldFlags.Default)
        {
            var group = config.LabelGroups.FirstOrDefault(g => g.Name == fieldGroup);
            return new PortfolioLabelConfig() { 
                Configuration_Id = config.Portfolio_Id,
                Group = group,
                FieldOrder = fieldOrder++,
                FieldTitle = fieldTitle,
                FieldName = fieldName,
                Included = included,
                FieldType = inputType,
                IncludedLock = includedLock,
                AdminOnlyLock = adminLock,
                FieldTypeLocked = inputTypeLocked,
                Flags = flags
            };
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
                Factory(FieldGroupName_ProjectIDs, "Project ID", nameof(ProjectModel.project_id), true, true, true, PortfolioFieldType.Auto, true),
                Factory(FieldGroupName_ProjectIDs, "Business Case Number", nameof(ProjectModel.business_case_number), true, false, false, PortfolioFieldType.FreeText, true),
                Factory(FieldGroupName_ProjectIDs, "FS Number", nameof(ProjectModel.fs_number), false, false, false, PortfolioFieldType.FreeText, true),

                Factory(FieldGroupName_AboutTheProject, "Project name", nameof(ProjectModel.project_name), true, true, false, PortfolioFieldType.FreeText, true),
                Factory(FieldGroupName_AboutTheProject, "Short description", nameof(ProjectModel.short_desc), true, false, false, PortfolioFieldType.FreeTextArea, true),
                Factory(FieldGroupName_AboutTheProject, "Risk rating", nameof(ProjectModel.risk_rating), false, false, false, PortfolioFieldType.OptionList, false),
                Factory(FieldGroupName_AboutTheProject, "Theme", nameof(ProjectModel.theme), true, false, false, PortfolioFieldType.OptionList, false),
                Factory(FieldGroupName_AboutTheProject, "Project type", nameof(ProjectModel.project_type), false, false, false, PortfolioFieldType.OptionList, false),
                Factory(FieldGroupName_AboutTheProject, "Project size", nameof(ProjectModel.project_size), false, false, false, PortfolioFieldType.OptionList, false),
                Factory(FieldGroupName_AboutTheProject, "Category", nameof(ProjectModel.category), true, false, false, PortfolioFieldType.OptionList, false),
                Factory(FieldGroupName_AboutTheProject, "Secondary category", nameof(ProjectModel.subcat), true, false, false, PortfolioFieldType.PredefinedMultiList, false), // Uses the same values as category
                Factory(FieldGroupName_AboutTheProject, "Directorate", nameof(ProjectModel.direct), true, false, false, PortfolioFieldType.PredefinedList, true),
                Factory(FieldGroupName_AboutTheProject, "Strategic objectives", nameof(ProjectModel.strategic_objectives), false, false, false, PortfolioFieldType.PredefinedList, true),
                Factory(FieldGroupName_AboutTheProject, "Programme", nameof(ProjectModel.programme), false, false, false, PortfolioFieldType.MultiOptionList, false),
                Factory(FieldGroupName_AboutTheProject, "Programme description", nameof(ProjectModel.programme_description), false, false, false, PortfolioFieldType.FreeText, true),
                Factory(FieldGroupName_AboutTheProject, "Project channel (link)", nameof(ProjectModel.link), true, false, false, PortfolioFieldType.FreeText, true),
                Factory(FieldGroupName_AboutTheProject, "Related projects", nameof(ProjectModel.rels), true, false, false, PortfolioFieldType.SelectPicker, true),
                Factory(FieldGroupName_AboutTheProject, "Dependencies", nameof(ProjectModel.dependencies), false, false, false, PortfolioFieldType.SelectPicker, true),
                Factory(FieldGroupName_AboutTheProject, "Key documents", nameof(ProjectModel.documents), true, false, false, PortfolioFieldType.PredefinedList, true),

                Factory(FieldGroupName_ProjectTeam, "Project lead", nameof(ProjectModel.oddlead), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_ProjectTeam, "Lead role", nameof(ProjectModel.oddlead_role), false, false, false, PortfolioFieldType.OptionList, true),
                Factory(FieldGroupName_ProjectTeam, "Lead team", nameof(ProjectModel.g6team), false, false, false, PortfolioFieldType.OptionList, true),
                Factory(FieldGroupName_ProjectTeam, "Key contact 1", nameof(ProjectModel.key_contact1), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_ProjectTeam, "Key contact 2", nameof(ProjectModel.key_contact2), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_ProjectTeam, "Key contact 3", nameof(ProjectModel.key_contact3), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_ProjectTeam, "Supplier", nameof(ProjectModel.supplier), false, false, false, PortfolioFieldType.FreeText, true),
                Factory(FieldGroupName_ProjectTeam, "Project team", nameof(ProjectModel.team), false, false, false, PortfolioFieldType.PredefinedField, true),

                Factory(FieldGroupName_ProjectPlan, "Intended start date", nameof(ProjectModel.start_date), false, false, false, PortfolioFieldType.Date, true),
                Factory(FieldGroupName_ProjectPlan, "Actual start date", nameof(ProjectModel.actstart), false, false, false, PortfolioFieldType.Date, true),
                Factory(FieldGroupName_ProjectPlan, "Expected current phase end date", nameof(ProjectModel.expendp), false, false, false, PortfolioFieldType.Date, true),
                Factory(FieldGroupName_ProjectPlan, "Expected end date", nameof(ProjectModel.expend), false, false, false, PortfolioFieldType.Date, true),
                Factory(FieldGroupName_ProjectPlan, "Actual end date", nameof(ProjectModel.actual_end_date), false, false, false, PortfolioFieldType.Date, true),
                Factory(FieldGroupName_ProjectPlan, "Hard deadline", nameof(ProjectModel.hardend), false, false, false, PortfolioFieldType.Date, true),
                Factory(FieldGroupName_ProjectPlan, "Percentage completed", nameof(ProjectModel.p_comp), false, false, false, PortfolioFieldType.Percentage, true),
                Factory(FieldGroupName_ProjectPlan, "Milestones", nameof(ProjectModel.milestones), false, false, false, PortfolioFieldType.PredefinedField, true),

                Factory(FieldGroupName_ProgressIndicators, "Phase", nameof(ProjectModel.phase), true, true, false, PortfolioFieldType.OptionList, false),
                Factory(FieldGroupName_ProgressIndicators, "RAG", nameof(ProjectModel.rag), true, false, false, PortfolioFieldType.RAGChoice, false),
                Factory(FieldGroupName_ProgressIndicators, "How to get to green", nameof(ProjectModel.how_get_green), false, false, false, PortfolioFieldType.FreeText, true),
                Factory(FieldGroupName_ProgressIndicators, "Status", nameof(ProjectModel.onhold), false, false, false, PortfolioFieldType.OptionList, false),

                //      FieldGroup      FieldTitle     FieldName                    Included IncludeLock AdmLock FieldType FieldTypeLocked
                Factory(FieldGroupName_Updates, "Update", nameof(ProjectModel.update), true, false, false, PortfolioFieldType.FreeTextArea, true, PortfolioFieldFlags.UpdateOnly),
                Factory(FieldGroupName_Updates, "Forward look", nameof(ProjectModel.forward_look), false, false, false, PortfolioFieldType.FreeTextArea, true),
                Factory(FieldGroupName_Updates, "Emerging issues, risks", nameof(ProjectModel.emerging_issues), false, false, false, PortfolioFieldType.FreeTextArea, true),

                Factory(FieldGroupName_Prioritisation, "Priority score", nameof(ProjectModel.priority_main), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_Prioritisation, "Funded", nameof(ProjectModel.funded), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_Prioritisation, "Confidence in delivery", nameof(ProjectModel.confidence), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_Prioritisation, "Priorities impacted", nameof(ProjectModel.priorities), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_Prioritisation, "Benefits", nameof(ProjectModel.benefits), false, false, false, PortfolioFieldType.PredefinedField, true),
                Factory(FieldGroupName_Prioritisation, "Criticality", nameof(ProjectModel.criticality), false, false, false, PortfolioFieldType.PredefinedField, true),

                Factory(FieldGroupName_Budget, "Budget category", nameof(ProjectModel.budgettype), false, false, false, PortfolioFieldType.OptionList, true),
                Factory(FieldGroupName_Budget, "Budget amount", nameof(ProjectModel.budget), false, false, false, PortfolioFieldType.Budget, true),
                Factory(FieldGroupName_Budget, "Amount spent", nameof(ProjectModel.spent), false, false, false, PortfolioFieldType.Budget, true),
                Factory(FieldGroupName_Budget, "Forecast spend at completion", nameof(ProjectModel.forecast_spend), false, false, false, PortfolioFieldType.Budget, true),
                Factory(FieldGroupName_Budget, "Budget field 1", nameof(ProjectModel.budget_field1), false, false, false, PortfolioFieldType.Budget, true),
                Factory(FieldGroupName_Budget, "Cost centre", nameof(ProjectModel.cost_centre), false, false, false, PortfolioFieldType.FreeText, true),

                Factory(FieldGroupName_FSAProcesses, "Assurance gate number", nameof(ProjectModel.fsaproc_assurance_gatenumber), false, false, false, PortfolioFieldType.FreeText, true),
                Factory(FieldGroupName_FSAProcesses, "Assurance gate completed", nameof(ProjectModel.fsaproc_assurance_gatecompleted), false, false, false, PortfolioFieldType.Date, true),
                Factory(FieldGroupName_FSAProcesses, "Next gate", nameof(ProjectModel.fsaproc_assurance_nextgate), false, false, false, PortfolioFieldType.FreeText, true),
            };

            SetMasterLabel(labels, nameof(ProjectModel.subcat), nameof(ProjectModel.category));
            SetMasterLabel(labels, nameof(ProjectModel.programme_description), nameof(ProjectModel.programme));
            SetMasterLabel(labels, nameof(ProjectModel.oddlead_role), nameof(ProjectModel.oddlead));
            SetMasterLabel(labels, nameof(ProjectModel.g6team), nameof(ProjectModel.oddlead));
            SetMasterLabel(labels, nameof(ProjectModel.how_get_green), nameof(ProjectModel.rag));

            return labels;
        }
    }
}