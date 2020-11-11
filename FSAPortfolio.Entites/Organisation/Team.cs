using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Organisation
{
    public class Team : IProjectOption
    {
        public int Id { get; set; }
        public string ViewKey { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public virtual Portfolio Portfolio { get; set; }
    }
}
