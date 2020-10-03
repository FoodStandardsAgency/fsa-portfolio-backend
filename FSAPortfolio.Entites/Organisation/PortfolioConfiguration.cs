using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FSAPortfolio.Entities
{
    public class PortfolioConfiguration
    {
        private static Lazy<PortfolioConfiguration> context;
        static PortfolioConfiguration()
        {
            context = new Lazy<PortfolioConfiguration>(() => {
                using (var db = new PortfolioContext())
                {
                    var ctx = new PortfolioConfiguration()
                    {
                        CompletedPhase = db.ProjectPhases.Single(ph => ph.Name == PhaseConstants.CompletedName)
                    };
                    return ctx;
                }
            });
        }
        public static PortfolioConfiguration Current => context.Value;

        public int Id { get; set; }

        public virtual ICollection<ProjectPhase> Phases { get; set; }
        public virtual ProjectPhase CompletedPhase { get; set; }

        public virtual ICollection<PortfolioLabelConfig> Labels { get; set; }
        public PortfolioLabelConfig ProjectIdLabel { get; set; }
        public PortfolioLabelConfig BCNumberLabel { get; set; }
    }

    public class PortfolioLabelConfig
    {
        public int Id { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }
        public bool Included { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; }

        [StringLength(50)]
        public string Label { get; set; }
    }
}