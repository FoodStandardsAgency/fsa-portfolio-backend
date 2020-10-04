using System.ComponentModel.DataAnnotations;

namespace FSAPortfolio.Entities
{
    public class PortfolioLabelConfig
    {
        public int Id { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }
        public bool Included { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; }

        [StringLength(50)]
        public string Label { get; set; }
        public int Configuration_Id { get; set; }
    }
}