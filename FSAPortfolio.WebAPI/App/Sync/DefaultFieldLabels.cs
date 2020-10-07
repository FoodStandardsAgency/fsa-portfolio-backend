using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.PostgreSQL.Projects;
using FSAPortfolio.WebAPI.Models;
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
            Func<string, string, string, bool, PortfolioFieldType, bool, bool, PortfolioLabelConfig> factory = (fg, ft, fn, inc, ftyp, ro, ftypl) => new PortfolioLabelConfig() { Configuration_Id = configuration_Id, FieldGroup = fg, FieldTitle = ft, FieldName = fn, Included = inc, FieldType = ftyp, ReadOnly = ro, FieldTypeLocked = ftypl };
            return new PortfolioLabelConfig[]
            {
                //      FieldGroup      FieldTitle                      FieldName     Included FieldType            Readonly FieldTypeLocked
                factory("Project IDs", "Project ID", nameof(ProjectModel.project_id), true, PortfolioFieldType.Auto, true, true),
                factory("Project IDs", "Business Case Number", nameof(ProjectModel.business_case_number), false, PortfolioFieldType.FreeText, true, false),
                factory("Project IDs", "FS Number", nameof(ProjectModel.fs_number), false, PortfolioFieldType.FreeText, true, false),

                factory("About the project", "Project name", nameof(ProjectModel.project_name), true, PortfolioFieldType.FreeText, true, true),
                factory("About the project", "Short description", nameof(ProjectModel.short_desc), false, PortfolioFieldType.FreeText, true, false),
                factory("About the project", "Risk rating", nameof(ProjectModel.risk_rating), false, PortfolioFieldType.OptionList, true, false),
                factory("About the project", "Theme", nameof(ProjectModel.theme), false, PortfolioFieldType.OptionList, false, false),
                factory("About the project", "Project type", nameof(ProjectModel.project_type), false, PortfolioFieldType.OptionList, false, false),
                factory("About the project", "Project size", nameof(ProjectModel.project_size), false, PortfolioFieldType.OptionList, false, false),
                factory("About the project", "Category", nameof(ProjectModel.category), false, PortfolioFieldType.OptionList, false, false),
                factory("About the project", "Secondary category", nameof(ProjectModel.subcat), false, PortfolioFieldType.OptionList, false, false),
                factory("About the project", "Directorate", nameof(ProjectModel.direct), false, PortfolioFieldType.PredefinedList, false, true),
                factory("About the project", "Strategic objectives", nameof(ProjectModel.strategic_objectives), false, PortfolioFieldType.PredefinedList, false, true),
                factory("About the project", "Programme", nameof(ProjectModel.programme), false, PortfolioFieldType.OptionList, false, false),
                factory("About the project", "Programme description", nameof(ProjectModel.programme_description), false, PortfolioFieldType.FreeText, false, true),
                factory("About the project", "Project channel (link)", nameof(ProjectModel.link), false, PortfolioFieldType.FreeText, false, true),
                factory("About the project", "Related projects", nameof(ProjectModel.rels), false, PortfolioFieldType.PredefinedList, false, true),
                factory("About the project", "Dependencies", nameof(ProjectModel.dependencies), false, PortfolioFieldType.PredefinedList, false, true),
                factory("About the project", "Key documents", nameof(ProjectModel.documents), false, PortfolioFieldType.PredefinedList, false, true),

                factory("Project team", "Project lead", nameof(ProjectModel.oddlead), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Project team", "Lead role", nameof(ProjectModel.oddlead_role), false, PortfolioFieldType.OptionList, false, true),
                factory("Project team", "Lead team", nameof(ProjectModel.lead_team), false, PortfolioFieldType.OptionList, false, true),
                factory("Project team", "Key contact 1", nameof(ProjectModel.key_contact1), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Project team", "Key contact 2", nameof(ProjectModel.key_contact2), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Project team", "Key contact 3", nameof(ProjectModel.key_contact3), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Project team", "Supplier", nameof(ProjectModel.supplier), false, PortfolioFieldType.FreeText, false, true),
                factory("Project team", "Project team", nameof(ProjectModel.project_team), false, PortfolioFieldType.PredefinedField, false, true),

                factory("Project plan", "Intended start date", nameof(ProjectModel.start_date), false, PortfolioFieldType.Date, false, true),
                factory("Project plan", "Actual start date", nameof(ProjectModel.actstart), false, PortfolioFieldType.Date, false, true),
                factory("Project plan", "Expected current phase end date", nameof(ProjectModel.expendp), false, PortfolioFieldType.Date, false, true),
                factory("Project plan", "Expected end date", nameof(ProjectModel.expend), false, PortfolioFieldType.Date, false, true),
                factory("Project plan", "Actual end date", nameof(ProjectModel.actual_end_date), false, PortfolioFieldType.Date, false, true),
                factory("Project plan", "Hard deadline", nameof(ProjectModel.hardend), false, PortfolioFieldType.Date, false, true),
                factory("Project plan", "Percentage completed", nameof(ProjectModel.p_comp), false, PortfolioFieldType.Percentage, false, true),
                factory("Project plan", "Milesones", nameof(ProjectModel.milestones), false, PortfolioFieldType.PredefinedField, false, true),

                factory("Progress indicators", "Phase", nameof(ProjectModel.phase), true, PortfolioFieldType.OptionList, true, false),
                factory("Progress indicators", "RAG", nameof(ProjectModel.rag), true, PortfolioFieldType.RAGChoice, true, false),
                factory("Progress indicators", "How to get to green", nameof(ProjectModel.how_get_green), false, PortfolioFieldType.FreeText, false, true),
                factory("Progress indicators", "Status", nameof(ProjectModel.progress_status), false, PortfolioFieldType.OptionList, false, false),

                //      FieldGroup      FieldTitle                      FieldName     Included FieldType            Readonly FieldTypeLocked
                factory("Updates", "Updates", nameof(ProjectModel.update), true, PortfolioFieldType.FreeText, true, true),
                factory("Updates", "Forward look", nameof(ProjectModel.forward_look), false, PortfolioFieldType.FreeText, false, true),
                factory("Updates", "Emerging issues, risks", nameof(ProjectModel.emerging_issues), false, PortfolioFieldType.FreeText, false, true),

                factory("Prioritisation", "Priority score", nameof(ProjectModel.priority_main), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Prioritisation", "Funded", nameof(ProjectModel.funded), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Prioritisation", "Confidence in delivery", nameof(ProjectModel.confidence), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Prioritisation", "Priorities impacted", nameof(ProjectModel.priorities), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Prioritisation", "Benefits", nameof(ProjectModel.benefits), false, PortfolioFieldType.PredefinedField, false, true),
                factory("Prioritisation", "Criticality", nameof(ProjectModel.criticality), false, PortfolioFieldType.PredefinedField, false, true),

                factory("Budget", "Budget category", nameof(ProjectModel.budgettype), false, PortfolioFieldType.OptionList, false, true),
                factory("Budget", "Budget amount", nameof(ProjectModel.budget), false, PortfolioFieldType.Budget, false, true),
                factory("Budget", "Amount spent", nameof(ProjectModel.spent), false, PortfolioFieldType.Budget, false, true),
                factory("Budget", "Forecast spend at completion", nameof(ProjectModel.forecast_spend), false, PortfolioFieldType.Budget, false, true),
                factory("Budget", "Budget field 1", nameof(ProjectModel.budget_field1), false, PortfolioFieldType.Budget, false, true),
                factory("Budget", "Cost centre", nameof(ProjectModel.cost_centre), false, PortfolioFieldType.FreeText, false, true),

                factory("FSA Processes", "Assurance gate number", nameof(ProjectModel.fsa_proc_assurance_gate_number), false, PortfolioFieldType.FreeText, false, true),
                factory("FSA Processes", "Assurance gate completed", nameof(ProjectModel.fsa_proc_assurance_gate_completed), false, PortfolioFieldType.Date, false, true),
                factory("FSA Processes", "Next gate", nameof(ProjectModel.fsa_proc_assurance_next_gate), false, PortfolioFieldType.FreeText, false, true),
            };
        }
    }
}