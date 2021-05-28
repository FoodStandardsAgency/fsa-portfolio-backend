using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
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

        [JsonProperty(PropertyName = "companyName")]
        public string companyName { get; set; }
    }

    public class MicrosoftGraphUserListResponse
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context { get; set; }

        public List<MicrosoftGraphUserModel> value { get; set; }
    }

    public class MicrosoftGraphGroupMember
    {

        [JsonProperty(PropertyName = "@odata.type")]
        public string MembershipType { get; set; }
        
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public class MicrosoftGraphGroupMemberListResponse
    {
        [JsonProperty(PropertyName = "@odata.context")]
        public string context { get; set; }

        public List<MicrosoftGraphGroupMember> value { get; set; }
    }
}