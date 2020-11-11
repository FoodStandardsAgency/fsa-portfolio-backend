using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Organisation
{
    public class Portfolio
    {
        public int Id { get; set; }

        [StringLength(10)]
        public string ViewKey { get; set; }

        [StringLength(20)]
        public string ShortName{ get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(10)]
        public string IDPrefix { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Team> Teams { get; set; }

    }
}
