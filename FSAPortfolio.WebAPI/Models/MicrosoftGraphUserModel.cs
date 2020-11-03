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

        public List<string> businessPhones { get; set; }
        public string displayName { get; set; }
        public string givenName { get; set; }
        public string jobTitle { get; set; }
        public string mail { get; set; }
        public string mobilePhone { get; set; }
        public string officeLocation { get; set; }
        public string preferredLanguage { get; set; }
        public string surname { get; set; }
        public string userPrincipalName { get; set; }
        public string id { get; set; }
    }

    public class MicrosoftGraphUserListResponse
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context { get; set; }

        public List<MicrosoftGraphUserModel> value { get; set; }
    }
}