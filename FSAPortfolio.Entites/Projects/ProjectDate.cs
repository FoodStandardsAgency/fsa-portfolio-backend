using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    [Flags]
    public enum ProjectDateFlags
    {
        Day = 1,
        Month = 1 << 1,
        Year = 1 << 2
    }
    public class ProjectDate
    {
        public DateTime? Date { get; set; }
        public ProjectDateFlags Flags { get; set; }
    }
}
