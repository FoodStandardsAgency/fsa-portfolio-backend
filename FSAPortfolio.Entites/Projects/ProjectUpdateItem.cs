using FSAPortfolio.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public class ProjectUpdateItem
    {
        public int Id { get; set; }

        public virtual Project Project { get; set; }
        public int Project_Id { get; set; }

        public virtual Person Person { get; set; }
        public DateTime Timestamp { get; set; }

        public string Text { get; set; }
        public virtual ProjectRAGStatus RAGStatus { get; set; }
        public virtual ProjectOnHoldStatus OnHoldStatus { get; set; }
        public virtual ProjectPhase Phase { get; set; }

        public float? PercentageComplete { get; set; }
        public decimal Budget { get; set; }
        public decimal Spent { get; set; }

        public DateTime? ExpectedCurrentPhaseEnd { get; set; }

        /// <summary>
        /// The update key in the source postgres database.
        /// </summary>
        public int SyncId { get; set; }

        public bool IsDuplicate(ProjectUpdateItem update)
        {
            return update != null &&
                RAGStatus.ViewKey.Equals(update.RAGStatus.ViewKey) &&
                OnHoldStatus.ViewKey.Equals(update.OnHoldStatus.ViewKey) &&
                Phase.ViewKey.Equals(update.Phase.ViewKey) &&
                PercentageComplete.Equals(update.PercentageComplete) &&
                Budget.Equals(update.Budget) &&
                Spent.Equals(update.Spent) &&
                ExpectedCurrentPhaseEnd.Equals(update.ExpectedCurrentPhaseEnd) &&
                (string.IsNullOrWhiteSpace(Text) || string.Equals(Text, update.Text, StringComparison.OrdinalIgnoreCase))
                ;
        }
    }
}
