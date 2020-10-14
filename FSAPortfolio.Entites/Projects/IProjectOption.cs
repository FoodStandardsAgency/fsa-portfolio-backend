namespace FSAPortfolio.Entities.Projects
{
    public interface IProjectOption
    {
        string Name { get; set; }
        int Order { get; set; }
        string ViewKey { get; set; }
    }
}