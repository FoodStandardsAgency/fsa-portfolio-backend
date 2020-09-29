using FSAPortfolio.Entites.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entites.Projects
{
    public class ProjectUpdateItem
    {
        public int Id { get; set; }

        public virtual Person Person { get; set; }
        public DateTime Timestamp { get; set; }

        public string Text { get; set; }
        public virtual ProjectRAGStatus RAGStatus { get; set; }
        public virtual ProjectOnHoldStatus OnHoldStatus { get; set; }
        public virtual ProjectPhase Phase { get; set; }

        /// <summary>
        /// The update key in the source postgres database.
        /// </summary>
        public int SyncId { get; set; }
    }
}
