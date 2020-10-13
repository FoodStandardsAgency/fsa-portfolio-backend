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
        public IEnumerable<PortfolioLabelModel> Labels { get; set; }
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


        [JsonProperty("admin")]
        public bool AdminOnly { get; set; }


        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("inputtype")]
        public string FieldType { get; set; }

        [JsonProperty("inputvalue")]
        public string InputValue { get; set; }

    }

}