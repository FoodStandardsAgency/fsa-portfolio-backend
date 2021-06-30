using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.App.Mapping.Projects;
using FSAPortfolio.Application.Models.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using FSAPortfolio.Common;

namespace FSAPortfolio.Application.Models
{
    [JsonConverter(typeof(ProjectEditOptionsModelConverter))]
    public class ProjectEditOptionsModel
    {
        private static List<DropDownItemModel> strategicObjectiveOptions = ProjectViewModelProfile.StragicObjectivesMap.Keys.Select((k, i) => 
            new DropDownItemModel() { Display = ProjectViewModelProfile.StragicObjectivesMap[k], Value = k, Order = i }
            ).ToList();

        [JsonProperty(ProjectPropertyConstants.phase)]
        public List<DropDownItemModel> PhaseItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.rag)]
        public List<DropDownItemModel> RAGStatusItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.onhold)]
        public List<DropDownItemModel> OnHoldStatusItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_size)]
        public List<DropDownItemModel> ProjectSizeItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.budgettype)]
        public List<DropDownItemModel> BudgetTypeItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.category)]
        public List<DropDownItemModel> CategoryItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.subcat)]
        public SelectPickerModel SubCategoryItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.direct)]
        public List<DropDownItemModel> Directorates { get; set; }

        [JsonProperty(ProjectPropertyConstants.strategic_objectives)]
        public List<DropDownItemModel> StrategicObjectives => strategicObjectiveOptions; 

        [JsonProperty(ProjectPropertyConstants.programme)]
        public List<DropDownItemModel> Programme { get; set; }


        [JsonProperty(ProjectPropertyConstants.risk_rating)]
        public List<DropDownItemModel> RiskRating { get; set; }

        [JsonProperty(ProjectPropertyConstants.theme)]
        public List<DropDownItemModel> Theme { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_type)]
        public List<DropDownItemModel> ProjectType { get; set; }

        // People
        [JsonProperty(ProjectPropertyConstants.oddlead)]
        public ActiveDirectoryUserSelectModel ODDLead { get; set; }

        [JsonProperty(ProjectPropertyConstants.oddlead_role)]
        public List<DropDownItemModel> ODDLeadRole { get; set; }

        [JsonProperty(FilterFieldConstants.LeadTeamFilter)]
        public List<DropDownItemModel> G6Team { get; set; }

        [JsonProperty(ProjectPropertyConstants.key_contact1)]
        public ActiveDirectoryUserSelectModel KeyContact1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.key_contact2)]
        public ActiveDirectoryUserSelectModel KeyContact2 { get; set; }

        [JsonProperty(ProjectPropertyConstants.key_contact3)]
        public ActiveDirectoryUserSelectModel KeyContact3 { get; set; }



        [JsonProperty(ProjectPropertyConstants.priority_main)]
        public List<DropDownItemModel> PriorityItems { get; set; }

        [JsonProperty(FilterFieldConstants.PriorityGroupFilter)]
        public List<DropDownItemModel> PriorityGroupItems { get; set; }


        [JsonProperty(ProjectPropertyConstants.funded)]
        public List<DropDownItemModel> FundedItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.confidence)]
        public List<DropDownItemModel> ConfidenceItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.priorities)]
        public List<DropDownItemModel> PrioritiesItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.benefits)]
        public List<DropDownItemModel> BenefitItems { get; set; }

        [JsonProperty(ProjectPropertyConstants.criticality)]
        public List<DropDownItemModel> CriticalityItems { get; set; }


        [JsonProperty(ProjectPropertyConstants.project_team_option1)]
        public List<DropDownItemModel> project_team_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_team_option2)]
        public List<DropDownItemModel> project_team_option2 { get; set; }


        [JsonProperty(ProjectPropertyConstants.project_plan_option1)]
        public List<DropDownItemModel> project_plan_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.project_plan_option2)]
        public List<DropDownItemModel> project_plan_option2 { get; set; }


        [JsonProperty(ProjectPropertyConstants.progress_option1)]
        public List<DropDownItemModel> progress_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.progress_option2)]
        public List<DropDownItemModel> progress_option2 { get; set; }


        [JsonProperty(ProjectPropertyConstants.budget_option1)]
        public List<DropDownItemModel> budget_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.budget_option2)]
        public List<DropDownItemModel> budget_option2 { get; set; }


        [JsonProperty(ProjectPropertyConstants.processes_option1)]
        public List<DropDownItemModel> processes_option1 { get; set; }

        [JsonProperty(ProjectPropertyConstants.processes_option2)]
        public List<DropDownItemModel> processes_option2 { get; set; }



        [JsonIgnore]
        public IEnumerable<ProjectDataOptionsModel> ProjectDataOptions { get; set; }
    }

    public class DropDownItemModel
    {
        [JsonProperty("display")]
        public string Display { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }
    }

    public class SelectPickerModel
    {
        [JsonProperty("header")]
        public string Header { get; set; }

        [JsonProperty("items")]
        public List<SelectPickerItemModel> Items { get; set; }
    }

    public class SelectPickerItemModel : DropDownItemModel
    {
        [JsonProperty("tokens")]
        public string SearchTokens { get; set; }
    }

    // TODO: this is obsolete
    public class ProjectDataOptionsModel
    {
        public string FieldName { get; set; }
        public object Options { get; set; }
    }

    public class ActiveDirectoryUserSelectModel
    {
        [JsonProperty("nouseroption")]
        public bool NoneOption { get; set; }
    }
}