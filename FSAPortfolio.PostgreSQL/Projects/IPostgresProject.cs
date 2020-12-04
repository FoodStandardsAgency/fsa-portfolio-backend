using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.PostgreSQL.Projects
{
    public interface IPostgresProject
    {
        string project_id { get; set; }
        DateTime timestamp { get; set; }
        string short_desc { get; set; }
        int id { get; set; }
        string update { get; set; }
        bool IsDuplicate(object project);
    }
}
