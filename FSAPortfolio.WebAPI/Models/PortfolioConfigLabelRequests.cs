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
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace FSAPortfolio.WebAPI.Models
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

        [JsonProperty("readonly")]
        public bool ReadOnly { get; set; }


        [JsonProperty("label")]
        public string FieldLabel { get; set; }

        [JsonProperty("inputtype")]
        public PortfolioFieldType FieldType { get; set; }

        [JsonProperty("inputtype_locked")]
        public bool FieldTypeLocked { get; set; }
    }

    public class PortfolioConfigDeleteLabelRequest
    {
        [JsonProperty("portfolio")]
        public string ViewKey { get; set; }
        [JsonProperty("field")]
        public string FieldName { get; set; }
    }


}