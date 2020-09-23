using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class UserRequestModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("passwordHash")]
        public string PasswordHash { get; set; }
    }

    public class UserModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [JsonProperty("accessGroup")]
        public string AccessGroup { get; set; }
    }
}