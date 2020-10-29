namespace FSAPortfolio.Entities.Projects
{
    public interface IProjectOption
    {
        int Id { get; set; }
        string Name { get; set; }
        int Order { get; set; }
        string ViewKey { get; set; }
    }
}