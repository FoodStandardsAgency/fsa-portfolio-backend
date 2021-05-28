using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class ODDLead
    {
        [JsonProperty("oddlead")]
        public string Name { get; set; }

        [JsonProperty("oddlead_email")]
        public string Email { get; set; }
    }

}