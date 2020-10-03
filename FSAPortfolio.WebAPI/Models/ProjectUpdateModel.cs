using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectUpdateModel
    {
        public string project_id { get; set; }
        public string timestamp { get; set; }
        public string max_timestamp { get; set; }
        public string date { get; set; }
        public string update { get; set; }
    }
}