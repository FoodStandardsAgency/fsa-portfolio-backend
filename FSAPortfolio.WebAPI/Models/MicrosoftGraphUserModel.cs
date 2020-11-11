using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class MicrosoftGraphUserModel
    {
        [JsonProperty(PropertyName = "@odata.type")]
        public string odataType { get; set; }

        [JsonProperty(PropertyName = "@odata.id")]
        public string odataId { get; set; }

        public string displayName { get; set; }
        public string givenName { get; set; }
        public string mail { get; set; }
        public string surname { get; set; }

        
        [JsonProperty(PropertyName = "userPrincipalName")]
        public string userPrincipalName { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "department")]
        public string department { get; set; }
    }

    public class MicrosoftGraphUserListResponse
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context { get; set; }

        public List<MicrosoftGraphUserModel> value { get; set; }
    }
}