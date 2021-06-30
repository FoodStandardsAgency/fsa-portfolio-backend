using FSAPortfolio.Common;
using FSAPortfolio.WebAPI.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Application.Models
{
    public class ProjectUpdateExportModel
    {
        public DateTime? timestamp { get; set; }

        [JsonProperty(ProjectPropertyConstants.ProjectId)]
        public string project_id { get; set; }

        public string phase { get; set; }
        public string rag { get; set; }
        public string onhold { get; set; }
        public string update { get; set; }
        public string budget { get; set; }
        public string spent { get; set; }
        public string expendp { get; set; }
        public string p_comp { get; set; }
    }
    public class ProjectUpdateChangePrecursor : ProjectUpdateExportModel
    {
        public ProjectUpdateExportModel PreviousUpdate { get; set; }

        public IEnumerable<ProjectUpdateChangeModel> Changes
        {
            get
            {
                List<ProjectUpdateChangeModel> changes = new List<ProjectUpdateChangeModel>();
                addChange(changes, ProjectPropertyConstants.phase, phase, PreviousUpdate?.phase);
                addChange(changes, ProjectPropertyConstants.rag, rag, PreviousUpdate?.rag);
                addChange(changes, ProjectPropertyConstants.onhold, onhold, PreviousUpdate?.onhold);
                addChange(changes, ProjectPropertyConstants.update, update, PreviousUpdate?.update);
                addChange(changes, ProjectPropertyConstants.budget, budget, PreviousUpdate?.budget);
                addChange(changes, ProjectPropertyConstants.spent, spent, PreviousUpdate?.spent);
                addChange(changes, ProjectPropertyConstants.ExpectedCurrentPhaseEndDate, expendp, PreviousUpdate?.expendp);
                addChange(changes, ProjectPropertyConstants.p_comp, p_comp, PreviousUpdate?.p_comp);
                return changes;
            }
        }

        private void addChange(List<ProjectUpdateChangeModel> changes, string tag, string value, string previousValue)
        {
            if ((PreviousUpdate != null && value != previousValue) || (PreviousUpdate == null) && !string.IsNullOrEmpty(value))
            {
                var change = new ProjectUpdateChangeModel()
                {
                    project_id = this.project_id,
                    timestamp = this.timestamp,
                    changeType = tag,
                    newValue = value,
                    previousvalue = previousValue
                };
                changes.Add(change);
            }
        }

    }

    public class ProjectUpdateChangeModel
    {
        public DateTime? timestamp { get; set; }

        [JsonProperty(ProjectPropertyConstants.ProjectId)]
        public string project_id { get; set; }

        public string changeType { get; set; }
        public string previousvalue { get; set; }
        public string newValue { get; set; }
    }
}