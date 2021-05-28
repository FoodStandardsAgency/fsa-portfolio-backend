using FSAPortfolio.Entities;

namespace FSAPortfolio.Application.Services
{
    public interface IServiceContext
    {
        PortfolioContext PortfolioContext { get; }
    }
}