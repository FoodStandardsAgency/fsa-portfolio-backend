using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Application.Models
{
    public interface IJsonProperties
    {
        IEnumerable<ProjectPropertyModel> Properties { get; set; }
    }
}
