namespace FSAPortfolio.Entities.Organisation
{
    public class PriorityGroup
    {
        public string ViewKey { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }
        public PortfolioConfiguration Configuration { get; set; }
    }
}