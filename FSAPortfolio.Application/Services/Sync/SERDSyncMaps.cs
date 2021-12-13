using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Services.Sync
{
    public partial class SyncMaps
    {
        public static readonly Dictionary<string, string> serdRnDMaps = new Dictionary<string, string>()
        {
            { "o", "None" },
            { "y", "Yes" },
            { "n", "No" }
        };

        public static readonly Dictionary<string, string> serdLeadTeamMaps = new Dictionary<string, string>()
        {
            { "role", "None" },
            { "au", "SERD-AU" },
            { "rau", "SERD-RAU" },
            { "sscr", "SERD-SSCR" },
            { "oth", "Other" }
        };

    }
}