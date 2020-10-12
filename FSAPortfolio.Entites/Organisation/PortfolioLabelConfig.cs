using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FSAPortfolio.Entities.Organisation
{
    public enum PortfolioFieldType
    {
        None = 0,
        Auto = 1,
        FreeText = 2,
        OptionList = 3,
        PredefinedList = 4,
        PredefinedField = 5,
        Date = 6,
        Percentage = 7,
        RAGChoice = 8,
        Budget = 9
    }

    public class PortfolioLabelConfig
    {
        public int Id { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }
        public int Configuration_Id { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; }

        [StringLength(50)]
        public string FieldTitle { get; set; }
        public int FieldOrder { get; set; }

        public bool Included { get; set; }
        public bool AdminOnly { get; set; }
        public bool IncludedLock { get; set; }
        public bool AdminOnlyLock { get; set; }


        [StringLength(50)]
        public string Label { get; set; }

        public PortfolioFieldType FieldType { get; set; }
        public bool FieldTypeLocked { get; set; }

        public virtual PortfolioLabelGroup Group { get; set; }

        /// <summary>
        /// This label can only be configured if the master label is included.
        /// </summary>
        public virtual PortfolioLabelConfig MasterLabel { get; set; }
        public int? MasterLabel_Id { get; set; }
    }

    public class PortfolioLabelGroup
    {
        public int Id { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }
        public int Configuration_Id { get; set; }


        [StringLength(50)]
        public string Name { get; set; }

        public int Order { get; set; }

        public virtual ICollection<PortfolioLabelConfig> Labels { get; set; }
    }
}