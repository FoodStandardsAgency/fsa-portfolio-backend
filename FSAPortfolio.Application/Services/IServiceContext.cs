using FSAPortfolio.Entities;
using FSAPortfolio.Entities.Organisation;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace FSAPortfolio.Application.Services
{
    public interface IServiceContext
    {
        PortfolioContext PortfolioContext { get; }
        string CurrentUserName { get; }
        ClaimsIdentity Identity { get; }
        string ActiveDirectoryId { get; }
        void AssertSuperuser();
        void AssertAdmin();
        void AssertAdmin(Portfolio portfolio);
        void AssertEditor(Portfolio portfolio);
        void AssertPermission(Portfolio portfolio);
        void AssertPermission(Portfolio portfolio, params string[] roles);
        bool HasPermission(Portfolio portfolio);
        bool HasPermission(Portfolio portfolio, params string[] roles);
        bool UserHasClaim(string[] claims);
        bool UserHasFSAClaim();
        bool UserHasSupplierClaim();
    }
}