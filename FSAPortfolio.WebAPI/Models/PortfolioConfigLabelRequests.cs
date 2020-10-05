using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace FSAPortfolio.WebAPI.Models
{
    [ModelBinder(typeof(PortfolioConfigLabelRequestModelBinder))]
    public class PortfolioConfigAddLabelRequest
    {
        [JsonProperty("portfolio")]
        public string ViewKey { get; set; }
        [JsonProperty("included")]
        public bool Included { get; set; }
        [JsonProperty("field")]
        public string FieldName { get; set; }
        [JsonProperty("label")]
        public string FieldLabel { get; set; }

    }
    [ModelBinder(typeof(PortfolioConfigLabelRequestModelBinder))]
    public class PortfolioConfigDeleteLabelRequest
    {
        [JsonProperty("portfolio")]
        public string ViewKey { get; set; }
        [JsonProperty("field")]
        public string FieldName { get; set; }
    }

    // TODO: pull the model field names out of the JsonProperty attributes
    public class PortfolioConfigLabelRequestModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var result = false;
            if (bindingContext.ModelType == typeof(PortfolioConfigAddLabelRequest))
            {
                result = BindAddRequest(bindingContext);
            }
            else if (bindingContext.ModelType == typeof(PortfolioConfigDeleteLabelRequest))
            {
                result = BindDeleteRequest(bindingContext);
            }
            return result;
        }

        private static bool BindDeleteRequest(ModelBindingContext bindingContext)
        {
            bool result = false;
            var viewKey = GetViewKey(bindingContext);
            var fieldName = GetFieldName(bindingContext);
            if (viewKey != null && fieldName != null)
            {
                var model = new PortfolioConfigDeleteLabelRequest()
                {
                    ViewKey = viewKey,
                    FieldName = fieldName
                };
                bindingContext.Model = model;
                result = true;
            }
            return result;
        }

        private static bool BindAddRequest(ModelBindingContext bindingContext)
        {
            bool result = false;
            var viewKey = GetViewKey(bindingContext);
            var fieldName = GetFieldName(bindingContext);
            var fieldLabel = GetFieldLabel(bindingContext);
            if (viewKey != null && fieldName != null && fieldLabel != null)
            {
                var included = GetIncluded(bindingContext);
                var model = new PortfolioConfigAddLabelRequest()
                {
                    ViewKey = viewKey,
                    FieldName = fieldName,
                    FieldLabel = fieldLabel,
                    Included = included
                };
                bindingContext.Model = model;
                result = true;
            }
            return result;
        }

        private static string GetViewKey(ModelBindingContext bindingContext) => bindingContext.ValueProvider.GetValue("portfolio")?.RawValue as string;
        private static string GetFieldName(ModelBindingContext bindingContext) => bindingContext.ValueProvider.GetValue("field")?.RawValue as string;
        private static bool GetIncluded(ModelBindingContext bindingContext) => Convert.ToBoolean(bindingContext.ValueProvider.GetValue("included")?.RawValue ?? "true");
        private static string GetFieldLabel(ModelBindingContext bindingContext) => bindingContext.ValueProvider.GetValue("label")?.RawValue as string;

    }
}