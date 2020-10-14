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

        [JsonProperty("phase")]
        public IEnumerable<DropDownItemModel> PhaseItems { get; set; }

        [JsonProperty("rag")]
        public IEnumerable<DropDownItemModel> RAGStatusItems { get; set; }

        [JsonProperty("onhold")]
        public IEnumerable<DropDownItemModel> OnHoldStatusItems { get; set; }

        [JsonProperty("project_size")]
        public IEnumerable<DropDownItemModel> ProjectSizeItems { get; set; }

        [JsonProperty("budgettype")]
        public IEnumerable<DropDownItemModel> BudgetTypeItems { get; set; }

        [JsonProperty("category")]
        public IEnumerable<DropDownItemModel> CategoryItems { get; set; }

        [JsonProperty("subcat")]
        public IEnumerable<DropDownItemModel> SubCategoryItems { get; set; }

    }

    public class DropDownItemModel
    {
        [JsonProperty("display")]
        public string Display { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("order")]
        public string Order { get; set; }
    }

}