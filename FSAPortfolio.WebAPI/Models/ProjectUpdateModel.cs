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
        public string oddlead { get; set; }
        public string servicelead { get; set; }


        [JsonIgnore]
        public IDictionary<string, ProjectPropertyModel> Properties { get; set; }

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Never assigned to
        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData;
#pragma warning restore IDE0044 // Add readonly modifier
#pragma warning restore CS0649 // Never assigned to

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
        public string oddlead { get; set; }
        public string servicelead { get; set; }
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