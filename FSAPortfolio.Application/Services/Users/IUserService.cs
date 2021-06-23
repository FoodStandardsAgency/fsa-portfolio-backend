using System.Threading.Tasks;

namespace FSAPortfolio.WebAPI.App.Users
{
    public interface IUserService
    {
        Task CreateUser(string userName, string passwordHash, string accessGroupViewKey);
        Task SeedAccessGroups();
    }
}