namespace FSAPortfolio.Entities.Projects
{
    public interface IProjectOption
    {
        int Id { get; set; }
        string Name { get; set; }

        // Set order to -1 to hide option from configuration
        int Order { get; set; }
        string ViewKey { get; set; }
    }
}