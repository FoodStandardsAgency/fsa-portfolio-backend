using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class PortfolioConfigAddLabelRequest
    {
        [JsonProperty("portfolio")]
        public string ViewKey { get; set; }


        [JsonProperty("field")]
        public string FieldName { get; set; }

        [JsonProperty("fieldgroup")]
        public string FieldGroup { get; set; }

        [JsonProperty("fieldtitle")]
        public string FieldTitle { get; set; }

        [JsonProperty("order")]
        public int FieldOrder { get; set; }

        [JsonProperty("included")]
        public bool Included { get; set; }

        [JsonProperty("admin")]
        public bool AdminOnly { get; set; }

        [JsonProperty("included_lock")]
        public bool IncludedLock { get; set; }

        [JsonProperty("adminonly_lock")]
        public bool AdminOnlyLock { get; set; }


        [JsonProperty("label")]
        public string FieldLabel { get; set; }

        [JsonProperty("inputtype")]
        public PortfolioFieldType FieldType { get; set; }

        [JsonProperty("inputoptions")]
        public string FieldOptions { get; set; }

        [JsonProperty("inputtype_locked")]
        public bool FieldTypeLocked { get; set; }

        [JsonProperty("flags")]
        public PortfolioFieldFlags Flags { get; set; } = PortfolioFieldFlags.DefaultCRUD;

    }

    public class PortfolioConfigDeleteLabelRequest
    {
        [JsonProperty("portfolio")]
        public string ViewKey { get; set; }
        [JsonProperty("field")]
        public string FieldName { get; set; }
    }


}