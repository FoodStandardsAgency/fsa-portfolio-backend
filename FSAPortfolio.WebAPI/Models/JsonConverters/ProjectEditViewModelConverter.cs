using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models.JsonConverters
{
    public class ProjectEditViewModelConverter : NonReentrantJsonConverter<ProjectEditViewModel>
    {
        public override ProjectEditViewModel ReadJson(JsonReader reader, Type objectType, ProjectEditViewModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            ProjectEditViewModel target = new ProjectEditViewModel();

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, ProjectEditViewModel value, JsonSerializer serializer)
        {
            JObject model = (JObject)FromObject(value, serializer);

            writer.WriteStartObject();
            WriteJson(writer, model);

            if (value.Properties != null)
            {
                foreach (var property in value.Properties)
                {
                    writer.WritePropertyName(property.FieldName);
                    writer.WriteValue(property.ProjectDataValue);
                }
            }
            writer.WriteEndObject();
        }

        private void WriteJson(JsonWriter writer, JObject value)
        {
            foreach (var p in value.Properties())
            {
                if (p.Value is JObject)
                    WriteJson(writer, (JObject)p.Value);
                else
                    p.WriteTo(writer);
            }
        }
    }





}