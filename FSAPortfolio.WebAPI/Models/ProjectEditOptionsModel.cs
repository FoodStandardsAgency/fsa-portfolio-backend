using FSAPortfolio.WebAPI.App;
using FSAPortfolio.WebAPI.Models.JsonConverters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FSAPortfolio.WebAPI.Models
{
    [JsonConverter(typeof(ProjectEditOptionsModelConverter))]
    public class ProjectEditOptionsModel
    {

        [JsonProperty(nameof(ProjectModel.phase))]
        public IEnumerable<DropDownItemModel> PhaseItems { get; set; }

        [JsonProperty(nameof(ProjectModel.rag))]
        public IEnumerable<DropDownItemModel> RAGStatusItems { get; set; }

        [JsonProperty(nameof(ProjectModel.onhold))]
        public IEnumerable<DropDownItemModel> OnHoldStatusItems { get; set; }

        [JsonProperty(nameof(ProjectModel.project_size))]
        public IEnumerable<DropDownItemModel> ProjectSizeItems { get; set; }

        [JsonProperty(nameof(ProjectModel.budgettype))]
        public IEnumerable<DropDownItemModel> BudgetTypeItems { get; set; }

        [JsonProperty(nameof(ProjectModel.category))]
        public IEnumerable<DropDownItemModel> CategoryItems { get; set; }

        [JsonProperty(nameof(ProjectModel.subcat))]
        public SelectPickerModel SubCategoryItems { get; set; }

        [JsonProperty(nameof(ProjectModel.direct))]
        public IEnumerable<DropDownItemModel> Directorates { get; set; }

        [JsonProperty(nameof(ProjectModel.strategic_objectives))]
        public readonly IEnumerable<DropDownItemModel> StrategicObjectives = new DropDownItemModel[] {
            new DropDownItemModel(){ Display = "None", Value = "none", Order = 0 },
            new DropDownItemModel(){ Display = "FSA wide", Value = "fsa", Order = 1 },
            new DropDownItemModel(){ Display = "Communications", Value = "communications", Order = 2 }
        };

        [JsonProperty(nameof(ProjectEditViewModel.rels))]
        public SelectPickerModel RelatedProjects { get; set; }

        [JsonProperty(nameof(ProjectEditViewModel.dependencies))]
        public SelectPickerModel DependantProjects { get; set; }

        [JsonProperty(nameof(ProjectModel.programme))]
        public IEnumerable<DropDownItemModel> Programme { get; set; }


        [JsonProperty(nameof(ProjectModel.risk_rating))]
        public IEnumerable<DropDownItemModel> RiskRating { get; set; }

        [JsonProperty(nameof(ProjectModel.theme))]
        public IEnumerable<DropDownItemModel> Theme { get; set; }

        [JsonProperty(nameof(ProjectModel.project_type))]
        public IEnumerable<DropDownItemModel> ProjectType { get; set; }

        // People
        [JsonProperty(nameof(ProjectUpdateModel.oddlead))]
        public ActiveDirectoryUserSelectModel ODDLead { get; set; }

        [JsonProperty(ProjectPropertyConstants.oddlead_role)]
        public IEnumerable<DropDownItemModel> ODDLeadRole { get; set; }

        [JsonProperty(FilterFieldConstants.LeadTeamFilter)]
        public IEnumerable<DropDownItemModel> G6Team { get; set; }


        [JsonProperty(nameof(ProjectModel.priority_main))]
        public IEnumerable<DropDownItemModel> PriorityItems { get; set; }

        [JsonProperty(FilterFieldConstants.PriorityGroupFilter)]
        public IEnumerable<DropDownItemModel> PriorityGroupItems { get; set; }


        [JsonProperty(nameof(ProjectModel.funded))]
        public IEnumerable<DropDownItemModel> FundedItems { get; set; }

        [JsonProperty(nameof(ProjectModel.confidence))]
        public IEnumerable<DropDownItemModel> ConfidenceItems { get; set; }

        [JsonProperty(nameof(ProjectModel.priorities))]
        public IEnumerable<DropDownItemModel> PrioritiesItems { get; set; }

        [JsonProperty(nameof(ProjectModel.benefits))]
        public IEnumerable<DropDownItemModel> BenefitItems { get; set; }

        [JsonProperty(nameof(ProjectModel.criticality))]
        public IEnumerable<DropDownItemModel> CriticalityItems { get; set; }

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
        public IEnumerable<SelectPickerItemModel> Items { get; set; }
    }

    public class SelectPickerItemModel : DropDownItemModel
    {
        [JsonProperty("tokens")]
        public string SearchTokens { get; set; }
    }

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