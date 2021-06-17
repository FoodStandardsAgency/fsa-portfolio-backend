using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class ProjectQueryModel
    {
        [JsonProperty("portfolio")]
        public string PortfolioViewKey { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_name)]
        public string Name { get; set; }

        [JsonProperty(ProjectPropertyConstants.risk_rating)]
        public string[] RiskRatings { get; set; }

        [JsonProperty(FilterFieldConstants.TeamMemberNameFilter)]
        public string TeamMemberName { get; set; }

        [JsonProperty(ProjectPropertyConstants.phase)]
        public string[] Phases { get; set; }

        [JsonProperty(ProjectPropertyConstants.theme)]
        public string[] Themes { get; set; }

        [JsonProperty(PropertyName = ProjectPropertyConstants.project_type)]
        public string[] ProjectTypes { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_size)]
        public string[] ProjectSizes { get; set; }


        [JsonProperty(ProjectPropertyConstants.rag)]
        public string[] RAGStatuses { get; set; }

        [JsonProperty(ProjectPropertyConstants.onhold)]
        public string[] OnHoldStatuses { get; set; }

        [JsonProperty(ProjectPropertyConstants.category)]
        public string[] Categories { get; set; }

        [JsonProperty(ProjectPropertyConstants.direct)]
        public string[] Directorates { get; set; }

        [JsonProperty(ProjectPropertyConstants.strategic_objectives)]
        public string[] StrategicObjectives { get; set; }

        [JsonProperty(ProjectPropertyConstants.programme)]
        public string[] Programmes { get; set; }


        [JsonProperty(ProjectPropertyConstants.budgettype)]
        public string[] BudgetTypes { get; set; }

        [JsonProperty(ProjectPropertyConstants.budget)]
        public BudgetFieldQueryModel Budget { get; set; }

        [JsonProperty(ProjectPropertyConstants.spent)]
        public BudgetFieldQueryModel Spent { get; set; }

        [JsonProperty(ProjectPropertyConstants.forecast_spend)]
        public BudgetFieldQueryModel ForecastSpend { get; set; }

        [JsonProperty(ProjectPropertyConstants.budget_field1)]
        public BudgetFieldQueryModel BudgetField1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.budget_field2)]
        public BudgetFieldQueryModel BudgetField2 { get; set; }


        [JsonProperty(ProjectPropertyConstants.budget_option1)]
        public string[] BudgetOptions1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.budget_option2)]
        public string[] BudgetOptions2 { get; set; }


        [JsonProperty(ProjectPropertyConstants.fsaproc_assurance_gatecompleted)]
        public DateFieldQueryModel AssuranceGateCompleted { get; set; }



        [JsonProperty(ProjectPropertyConstants.processes_option1)]
        public string[] ProcessOptions1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.processes_option2)]
        public string[] ProcessOptions2 { get; set; }


        [JsonProperty(ProjectPropertyConstants.project_plan_option1)]
        public string[] ProjectPlanOptions1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_plan_option2)]
        public string[] ProjectPlanOptions2 { get; set; }


        [JsonProperty(ProjectPropertyConstants.progress_option1)]
        public string[] ProgressOptions1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.progress_option2)]
        public string[] ProgressOptions2 { get; set; }




        [JsonProperty(ProjectPropertyConstants.ProjectLead)]
        public string[] ProjectLeadName { get; set; }

        [JsonProperty(ProjectPropertyConstants.key_contact1)]
        public string[] KeyContact1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.key_contact2)]
        public string[] KeyContact2 { get; set; }

        [JsonProperty(ProjectPropertyConstants.key_contact3)]
        public string[] KeyContact3 { get; set; }



        [JsonProperty(FilterFieldConstants.LeadTeamFilter)]
        public string[] Teams { get; set; }

        [JsonProperty(FilterFieldConstants.PriorityGroupFilter)]
        public string[] PriorityGroups { get; set; }


        [JsonProperty(ProjectPropertyConstants.IntendedStartDate)]
        public DateFieldQueryModel IntendedStartDate { get; set; }

        [JsonProperty(ProjectPropertyConstants.ActualStartDate)]
        public DateFieldQueryModel ActualStartDate { get; set; }

        [JsonProperty(ProjectPropertyConstants.ExpectedCurrentPhaseEndDate)]
        public DateFieldQueryModel ExpectedCurrentPhaseEndDate { get; set; }

        [JsonProperty(ProjectPropertyConstants.ExpectedEndDate)]
        public DateFieldQueryModel ExpectedEndDate { get; set; }

        [JsonProperty(ProjectPropertyConstants.ActualEndDate)]
        public DateFieldQueryModel ActualEndDate { get; set; }

        [JsonProperty(ProjectPropertyConstants.HardDeadline)]
        public DateFieldQueryModel HardDeadline { get; set; }


        [JsonProperty(FilterFieldConstants.LastUpdateFilter)]
        public DateTime? LastUpdateBefore { get; set; }

        [JsonProperty(FilterFieldConstants.NoUpdatesFilter)]
        public bool? NoUpdates { get; set; }

        [JsonProperty(FilterFieldConstants.PastIntendedStartDateFilter)]
        public bool? PastStartDate { get; set; }

        [JsonProperty(FilterFieldConstants.MissedEndDateFilter)]
        public bool? MissedEndDate { get; set; }

    }

    public class BudgetFieldQueryModel
    {
        [JsonProperty("amount")]
        public decimal? Amount { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

    }

    public class DateFieldQueryModel
    {
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

    }


}