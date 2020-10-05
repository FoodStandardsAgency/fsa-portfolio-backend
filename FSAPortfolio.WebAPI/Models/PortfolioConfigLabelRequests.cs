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

    public class PortfolioConfigLabelRequestModelBinder : IModelBinder
    {
        static Dictionary<string, string> addFieldMappings;
        static Dictionary<string, string> deleteFieldMappings;
        static PortfolioConfigLabelRequestModelBinder()
        {
            // Pull the field mappings out of JsonProperty attributes
            var tjpa = typeof(JsonPropertyAttribute);

            addFieldMappings = typeof(PortfolioConfigAddLabelRequest).GetProperties()
                .ToDictionary(p => p.Name, p => ((JsonPropertyAttribute)Attribute.GetCustomAttribute(p, tjpa)).PropertyName);

            deleteFieldMappings = typeof(PortfolioConfigDeleteLabelRequest).GetProperties()
                .ToDictionary(p => p.Name, p => ((JsonPropertyAttribute)Attribute.GetCustomAttribute(p, tjpa)).PropertyName);
        }

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
            var viewKey = GetString(bindingContext, deleteFieldMappings, nameof(PortfolioConfigDeleteLabelRequest.ViewKey));
            var fieldName = GetString(bindingContext, deleteFieldMappings, nameof(PortfolioConfigDeleteLabelRequest.FieldName));
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
            var viewKey = GetString(bindingContext, addFieldMappings, nameof(PortfolioConfigAddLabelRequest.ViewKey));
            var fieldName = GetString(bindingContext, addFieldMappings, nameof(PortfolioConfigAddLabelRequest.FieldName));
            var fieldLabel = GetString(bindingContext, addFieldMappings, nameof(PortfolioConfigAddLabelRequest.FieldLabel));
            if (viewKey != null && fieldName != null && fieldLabel != null)
            {
                var included = GetBool(bindingContext, addFieldMappings, nameof(PortfolioConfigAddLabelRequest.Included));
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

        private static string GetString(ModelBindingContext bindingContext, Dictionary<string, string> map, string propertyName) => bindingContext.ValueProvider.GetValue(map[propertyName])?.RawValue as string;
        private static bool GetBool(ModelBindingContext bindingContext, Dictionary<string, string> map, string propertyName) => Convert.ToBoolean(bindingContext.ValueProvider.GetValue(map[propertyName])?.RawValue ?? "true");

    }
}