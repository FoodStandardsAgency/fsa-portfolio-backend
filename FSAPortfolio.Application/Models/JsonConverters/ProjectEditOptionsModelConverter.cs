using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FSAPortfolio.WebAPI.Models.JsonConverters
{
    public class ProjectEditOptionsModelConverter : NonReentrantJsonConverter<ProjectEditOptionsModel>
    {
        public override bool CanRead => false;

        public override ProjectEditOptionsModel ReadJson(JsonReader reader, Type objectType, ProjectEditOptionsModel existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, ProjectEditOptionsModel value, JsonSerializer serializer)
        {
            JObject model = (JObject)FromObject(value, serializer);

            writer.WriteStartObject();
            WriteJson(writer, model);

            if (value.ProjectDataOptions != null)
            {
                foreach (var property in value.ProjectDataOptions)
                {
                    if (property.Options != null)
                    {
                        writer.WritePropertyName(property.FieldName);
                        JToken options = JToken.FromObject(property.Options);
                        options.WriteTo(writer);
                    }
                }
            }
            writer.WriteEndObject();
        }

        private void WriteJson(JsonWriter writer, JObject value)
        {
            foreach (var p in value.Properties())
            {
                p.WriteTo(writer);
            }
        }
    }





}