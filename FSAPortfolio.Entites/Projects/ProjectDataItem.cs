using FSAPortfolio.Entities.Organisation;

namespace FSAPortfolio.Entities.Projects
{
    public class ProjectDataItem
    {
        public int Id { get; set; }

        public virtual Project Project { get; set; }
        public int Project_Id { get; set; }

        public virtual PortfolioLabelConfig Label { get; set; }
        public int Label_Id { get; set; }
        public string Value { get; set; }
    }
}