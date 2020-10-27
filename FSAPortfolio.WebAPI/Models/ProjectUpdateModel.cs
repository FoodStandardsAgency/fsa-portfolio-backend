using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.Models
{
    public class ProjectUpdateModel : ProjectModel
    {
        public string[] rels { get; set; }
        public string[] dependencies { get; set; }
    }

    public class ProjectEditViewModel : ProjectModel
    {
        public string[] rels { get; set; }
        public string[] dependencies { get; set; }

    }
}