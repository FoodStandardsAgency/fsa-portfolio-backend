using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class IdentityResponseModel
    {
        [JsonProperty(PropertyName = "roles")]
        public string[] Roles { get; set; }

        [JsonProperty(PropertyName = "accessGroup")]
        public string AccessGroup { get; set; }
    }
}