using FSAPortfolio.WebAPI.Models.JsonConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectUpdateModel : ProjectModel
    {
        public string[] rels { get; set; }
        public string[] dependencies { get; set; }
        [JsonIgnore]
        public IDictionary<string, ProjectPropertyModel> Properties { get; set; }
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