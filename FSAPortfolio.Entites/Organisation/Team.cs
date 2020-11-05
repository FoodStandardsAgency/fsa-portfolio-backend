using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Organisation
{
    /// <summary>
    /// TODO: This is a hack - need to look at teams again!
    /// </summary>
    public class Team
    {
        public string ViewKey { get; set; }
        public string Name => ViewKey;
        public int Order { get; set; } = 0;
        public PortfolioConfiguration Config { get; set; } // TODO: This is a hack!!!
    }
}
