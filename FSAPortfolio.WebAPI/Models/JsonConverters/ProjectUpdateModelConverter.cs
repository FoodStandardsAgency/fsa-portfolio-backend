using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FSAPortfolio.WebAPI.Models.JsonConverters
{
    public class ProjectUpdateModelConverter : JsonConverter<ProjectUpdateModel>
    {
        private static readonly string[] modelProperties;
        static ProjectUpdateModelConverter()
        {
            modelProperties = typeof(ProjectUpdateModel).GetProperties().Select(p => p.Name.ToLower()).ToArray();
        }

        public override bool CanWrite => false;

        public override ProjectUpdateModel ReadJson(JsonReader reader, Type objectType, ProjectUpdateModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Create target object based on JObject
            ProjectUpdateModel target = new ProjectUpdateModel();
            Dictionary<string, ProjectPropertyModel> projectProperties = new Dictionary<string, ProjectPropertyModel>();

            // TODO: clean this up
            //// Load JObject from stream
            //JObject jObject = JObject.Load(reader);

            //// Populate the object properties
            //serializer.Populate(jObject.CreateReader(), target);

            //return base.ReadJson(reader, objectType, existingValue, serializer);
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    string jsonPropertyName = reader.Value.ToString().ToLower();
                    if (reader.Read())
                    {
                        if (modelProperties.Contains(jsonPropertyName))
                        {
                            PropertyInfo pinfo = target.GetType().GetProperty(jsonPropertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                            var convertedValue = Convert.ChangeType(reader.Value, pinfo.PropertyType);
                            pinfo.SetValue(target, convertedValue, null);
                        }
                        else
                        {
                            projectProperties.Add(jsonPropertyName, new ProjectPropertyModel() { FieldName = jsonPropertyName, ProjectDataValue = reader.Value.ToString() });
                        }
                    }
                }
            }
            target.Properties = projectProperties;
            return target;
        }

        public override void WriteJson(JsonWriter writer, ProjectUpdateModel value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }





}