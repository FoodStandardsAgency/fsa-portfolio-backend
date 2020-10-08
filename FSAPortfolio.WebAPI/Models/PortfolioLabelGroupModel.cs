using FSAPortfolio.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class PortfolioLabelGroupModel
    {
        [JsonProperty("labels")]
        public IEnumerable<PortfolioLabelModel> Labels { get; set; }
    }

    public class PortfolioLabelModel
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

        [JsonProperty("readonly")]
        public bool ReadOnly { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("inputtype")]
        public string FieldType { get; set; }

        [JsonProperty("inputtype_locked")]
        public bool FieldTypeLocked { get; set; }

    }
}