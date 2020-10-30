using FSAPortfolio.WebAPI.Models.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectUpdateModel : ProjectModel
    {
        public string[] rels { get; set; }
        public string[] dependencies { get; set; }
        [JsonIgnore]
        public IDictionary<string, ProjectPropertyModel> Properties { get; set; }

        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if(_additionalData != null && _additionalData.Count > 0)
            {
                Properties = _additionalData.ToDictionary(k => k.Key, s => new ProjectPropertyModel() { FieldName = s.Key, ProjectDataValue = s.Value.ToString() });
            }
        }
    }

    [JsonConverter(typeof(ProjectEditViewModelConverter))]
    public class ProjectEditViewModel : ProjectModel
    {
        public string[] rels { get; set; }
        public string[] dependencies { get; set; }
        [JsonIgnore]
        public IEnumerable<ProjectPropertyModel> Properties { get; set; }
    }

    public class ProjectPropertyModel
    {
        public string FieldName { get; set; }
        public string ProjectDataValue { get; set; }
    }


}