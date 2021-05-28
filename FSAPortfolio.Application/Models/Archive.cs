using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class ArchiveResponse
    {
        [JsonProperty("projectIds")]
        public string[] ArchivedProjectIds { get; set; }
    }
}