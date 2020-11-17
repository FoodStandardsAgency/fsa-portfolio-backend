using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class Document
    {
        public int Id { get; set; }


        [StringLength(150)]
        public string Name { get; set; }

        public string Link { get; set; }
        public int Order { get; set; }

        public string ExportText => Name;
    }
}
