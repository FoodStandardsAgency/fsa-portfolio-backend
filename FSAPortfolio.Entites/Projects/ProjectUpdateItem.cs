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
        public int? RAGStatus_Id { get; set; }
        public virtual ProjectOnHoldStatus OnHoldStatus { get; set; }
        public int? OnHoldStatus_Id { get; set; }
        public virtual ProjectPhase Phase { get; set; }
        public int? Phase_Id { get; set; }

        public float? PercentageComplete { get; set; }
        public decimal Budget { get; set; }
        public decimal Spent { get; set; }

        public ProjectDate ExpectedCurrentPhaseEnd { get; set; }

        /// <summary>
        /// The update key in the source postgres database.
        /// </summary>
        public int SyncId { get; set; }

        public bool IsDuplicate(ProjectUpdateItem update)
        {
            return update != null &&
                IsDuplicate(RAGStatus, update.RAGStatus) &&
                IsDuplicate(OnHoldStatus, update.OnHoldStatus) &&
                IsDuplicate(Phase, update.Phase) &&
                ((PercentageComplete == null && update.PercentageComplete == null) || (PercentageComplete.HasValue && PercentageComplete.Equals(update.PercentageComplete))) &&
                Budget.Equals(update.Budget) &&
                Spent.Equals(update.Spent) &&
                ((ExpectedCurrentPhaseEnd == null && update.ExpectedCurrentPhaseEnd.Date == null) || (ExpectedCurrentPhaseEnd != null && ExpectedCurrentPhaseEnd.Equals(update.ExpectedCurrentPhaseEnd.Date))) &&
                (string.IsNullOrWhiteSpace(Text) || string.Equals(Text, update.Text, StringComparison.OrdinalIgnoreCase))
                ;
        }

        private bool IsDuplicate<T>(T option1, T option2) where T : IProjectOption => (option1 == null && option2 == null) || (option1 != null && option1.ViewKey.Equals(option2?.ViewKey));
    }
}
