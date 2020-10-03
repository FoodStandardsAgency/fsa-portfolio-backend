using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class ProjectRAGStatus
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string ViewKey { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
        public int Order { get; set; }
        public virtual PortfolioConfiguration Configuration { get; set; }
        public int Configuration_Id { get; set; }

    }

    public class ProjectOnHoldStatus
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string ViewKey { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public int Order { get; set; }
        public virtual PortfolioConfiguration Configuration { get; set; }
        public int Configuration_Id { get; set; }
    }
    public class ProjectPhase
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string ViewKey { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
        public int Order { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }
        public int Configuration_Id { get; set; }
    }
}
