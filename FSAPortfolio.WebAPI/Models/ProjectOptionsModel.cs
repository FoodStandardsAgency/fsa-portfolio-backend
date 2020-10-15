using Newtonsoft.Json;
using System.Collections.Generic;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectOptionsModel
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
            new DropDownItemModel(){ Display = "Communications", Value = "communcations", Order = 2 }
        };

        [JsonProperty(nameof(ProjectModel.rels))]
        public SelectPickerModel RelatedProjects { get; set; }

        [JsonProperty(nameof(ProjectModel.dependencies))]
        public SelectPickerModel DependantProjects { get; set; }

        // TODO: these options are multi selects that come from the user input in the config screen.
        // Need to store the input (like RAG Statuses etc are stored - but generic way for ad-hoc fields).
        [JsonProperty(nameof(ProjectModel.programme))]
        public readonly SelectPickerModel Programme = new SelectPickerModel()
        {
            Header = "Select the programmes...",
            Items = new SelectPickerItemModel[] {
                new SelectPickerItemModel(){ Display = "Test1", Value = "prog1", Order = 0 },
                new SelectPickerItemModel(){ Display = "Test2", Value = "prog2", Order = 1 },
                new SelectPickerItemModel(){ Display = "Test3", Value = "prog3", Order = 2 }
            }
        };

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

}