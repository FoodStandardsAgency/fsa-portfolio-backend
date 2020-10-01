using FSAPortfolio.Entites;
using FSAPortfolio.Entites.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSAPortfolio.WebAPI.App
{
    public class DirectorateContext
    {
        private static Lazy<DirectorateContext> context;
        static DirectorateContext()
        {
            context = new Lazy<DirectorateContext>(() => {
                using (var db = new PortfolioContext())
                {
                    var ctx = new DirectorateContext()
                    {
                        CompletedPhase = db.ProjectPhases.Single(ph => ph.Name == PhaseConstants.CompletedName)
                    };
                    return ctx;
                }
            });
        }
        public static DirectorateContext Current => context.Value;

        public ProjectPhase CompletedPhase { get; set; }

        private DirectorateContext() { }

    }
}