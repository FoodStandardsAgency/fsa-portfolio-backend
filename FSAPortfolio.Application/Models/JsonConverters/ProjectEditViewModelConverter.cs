using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FSAPortfolio.Application.Models.JsonConverters
{
    public class ProjectEditViewModelConverter : NonReentrantJsonConverter<ProjectEditViewModel>
    {
        public override bool CanRead => false;

        public override ProjectEditViewModel ReadJson(JsonReader reader, Type objectType, ProjectEditViewModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, ProjectEditViewModel value, JsonSerializer serializer)
        {
            JObject model = (JObject)FromObject(value, serializer);

            writer.WriteStartObject();
            WritePropertiesJson(writer, model);

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

        private void WritePropertiesJson(JsonWriter writer, JObject value)
        {
            foreach (var p in value.Properties())
            {
                p.WriteTo(writer);
            }
        }
    }





}