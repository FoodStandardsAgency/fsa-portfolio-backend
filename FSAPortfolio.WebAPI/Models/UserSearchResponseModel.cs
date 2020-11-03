using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class UserSearchResponseModel
    {
        [JsonProperty("searchresults")]
        public IEnumerable<UserSearchModel> SearchResults { get; set; }
    }

    public class UserSearchModel
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

    }
}