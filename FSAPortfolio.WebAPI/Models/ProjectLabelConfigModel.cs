using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    /// <summary>
    /// Need a separate model for labels when viewing a project so can handle master labels
    /// </summary>
    public class ProjectLabelConfigModel
    {
        [JsonProperty("labels")]
        public IEnumerable<ProjectLabelModel> Labels { get; set; }
    }

    public class ProjectLabelModel
    {
        [JsonProperty("field")]
        public string FieldName { get; set; }

        [JsonProperty("fieldgroup")]
        public string FieldGroup { get; set; }

        [JsonProperty("grouporder")]
        public int? GroupOrder { get; set; }

        [JsonProperty("fieldorder")]
        public int FieldOrder { get; set; }

        [JsonProperty("fieldtitle")]
        public string FieldTitle { get; set; }


        [JsonProperty("included")]
        public bool Included { get; set; }

        [JsonProperty("admin")]
        public bool AdminOnly { get; set; }


        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("inputtype")]
        public string FieldType { get; set; }

        [JsonProperty("inputvalue")]
        public string InputValue { get; set; }

    }

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
        public IEnumerable<DropDownItemModel> SubCategoryItems { get; set; }

        [JsonProperty(nameof(ProjectModel.direct))]
        public IEnumerable<DropDownItemModel> Directorates = new DropDownItemModel[] {
            new DropDownItemModel(){ Display = "None", Value = "0", Order = 0 },
            new DropDownItemModel(){ Display = "Communications", Value = "1", Order = 2 },
            new DropDownItemModel(){ Display = "Incidents & Resilience", Value = "2", Order = 3 },
            new DropDownItemModel(){ Display = "Field Operations", Value = "3", Order = 4 },
            new DropDownItemModel(){ Display = "Finance & Performance", Value = "4", Order = 5 },
            new DropDownItemModel(){ Display = "Food Safety Policy", Value = "5", Order = 6 },
            new DropDownItemModel(){ Display = "FSA wide", Value = "6", Order = 1 },
            new DropDownItemModel(){ Display = "National Food Crime Unit", Value = "7", Order = 7 },
            new DropDownItemModel(){ Display = "Northern Ireland", Value = "8", Order = 8 },
            new DropDownItemModel(){ Display = "Openness, Data & Digital", Value = "9", Order = 9 },
            new DropDownItemModel(){ Display = "People", Value = "10", Order = 10 },
            new DropDownItemModel(){ Display = "Regulatory Compliance", Value = "11", Order = 11 },
            new DropDownItemModel(){ Display = "Science, Evidence & Research", Value = "12", Order = 12 },
            new DropDownItemModel(){ Display = "Strategy, Legal & Governance", Value = "13", Order = 13 },
            new DropDownItemModel(){ Display = "Wales", Value = "14", Order = 14 }
        };

        [JsonProperty(nameof(ProjectModel.strategic_objectives))]
        public IEnumerable<DropDownItemModel> StrategicObjectives = new DropDownItemModel[] {
            new DropDownItemModel(){ Display = "None", Value = "none", Order = 0 },
            new DropDownItemModel(){ Display = "FSA wide", Value = "fsa", Order = 1 },
            new DropDownItemModel(){ Display = "Communications", Value = "communcations", Order = 2 }
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

}