using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public partial class SyncMaps
    {
        public static readonly Dictionary<string, string> serdProjectTypeMaps = new Dictionary<string, string>()
        {
            { "o", "Project type" },
            { "i", "Internal" },
            { "f", "Fellowship" },
            { "s", "Studentship" },
            { "c", "Commission" },
            { "p", "Partnership" }
        };

    }
}