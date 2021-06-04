using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Models
{
    public class ProjectCollectionModel
    {
        public IEnumerable<ProjectExportModel> Projects { get; set; }
    }

    public class ProjectUpdateCollectionModel
    {
        public IEnumerable<ProjectUpdateExportModel> Updates { get; set; }
    }
    public class ProjectChangeCollectionModel
    {
        public IEnumerable<ProjectUpdateChangeModel> Changes { get; set; }
    }
}
