using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.PostgreSQL.Projects
{
    interface IPostgresProject<T> where T: class, new()
    {
        bool IsDuplicate(T p);
    }
}
