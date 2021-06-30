using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.App.Sync
{
    public interface ISyncService
    {
        Portfolio AddPortfolio(PortfolioContext context, string name, string shortName, string viewKey, string requiredRoles = null);
        void SyncAllProjects(string portfolio = "odd");
        void SyncDirectorates();
        Task SyncPeople(string viewKey = "odd", bool forceADSync = false);
        void SyncPortfolios();
        bool SyncProject(string projectId, string portfolioViewKey = null);
        void SyncUsers();
        IEnumerable<string> Messages();
        void ClearLog();
    }
}