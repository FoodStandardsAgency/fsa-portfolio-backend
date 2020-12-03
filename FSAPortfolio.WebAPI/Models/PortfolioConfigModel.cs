using FSAPortfolio.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class PortfolioConfigModel
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

        [JsonProperty("included_lock")]
        public bool IncludedLock { get; set; }

        [JsonProperty("adminonly_lock")]
        public bool AdminOnlyLock { get; set; }

        [JsonProperty("fsaonly")]
        public bool FSAOnly { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("inputtype")]
        public string FieldType { get; set; }

        [JsonProperty("filterable")]
        public bool Filterable { get; set; }

        [JsonProperty("filterproject")]
        public bool FilterProject { get; set; }

        [JsonProperty("inputdesc")]
        public string FieldTypeDescription { get; set; }

        [JsonProperty("inputvalue")]
        public string InputValue { get; set; }

        [JsonProperty("inputtype_locked")]
        public bool FieldTypeLocked { get; set; }

        [JsonProperty("masterfield")]
        public string MasterField { get; set; }

    }
}