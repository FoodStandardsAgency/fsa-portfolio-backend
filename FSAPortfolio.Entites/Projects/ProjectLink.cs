using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class ProjectLink
    {

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Link { get; set; }

        public string ExportText => Name;

    }
}
