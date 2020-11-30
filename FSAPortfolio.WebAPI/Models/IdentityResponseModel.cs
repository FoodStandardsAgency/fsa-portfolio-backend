using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class IdentityResponseModel
    {
        [JsonProperty(PropertyName = "roles")]
        public string[] Roles { get; internal set; }

        [JsonProperty(PropertyName = "accessGroup")]
        public string AccessGroup { get; internal set; }
    }
}