using FSAPortfolio.Common;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Services.Index.Models
{
    public class ProjectSearchIndexModel
    {
        [JsonProperty(ProjectPropertyConstants.ProjectId)]
        public string project_id { get; set; }

        /// <summary>
        /// The concise name of the project, limited to a maximum of 250 characters.
        /// </summary>
        [JsonProperty(ProjectPropertyConstants.project_name)]
        public string project_name { get; set; }


        [JsonProperty(ProjectPropertyConstants.short_desc)]
        public string short_desc { get; set; }


        [JsonProperty(ProjectPropertyConstants.forward_look)]
        public string forward_look { get; set; }

        [JsonProperty(ProjectPropertyConstants.rag)]
        public string rag { get; set; }


    }


}