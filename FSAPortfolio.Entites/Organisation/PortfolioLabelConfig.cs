﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FSAPortfolio.Entities.Projects;

namespace FSAPortfolio.Entities.Organisation
{
    public enum PortfolioFieldType
    {
        None = 0,
        Auto = 1,

        Date = 10, // Native data types
        Percentage = 11,
        Budget = 12,
        Month = 13,

        FreeText = 20, // Text input
        SmallFreeTextArea = 21,
        MediumFreeTextArea = 22,
        LargeFreeTextArea = 23,

        OptionList = 30, // Editable option lists
        MultiOptionList = 31,

        PredefinedList = 41, // Predefined option lists
        PredefinedMultiList = 42,
        PredefinedSearchableList = 43,
        PredefinedField = 44,

        ProjectMultiSelect = 50, // Data specific inputs
        PhaseChoice = 51,
        RAGChoice = 52,
        NamedLink = 53,
        LinkedItemList = 54,
        ProjectUpdateText = 55

    }

    /// <summary>
    /// ProjectData set = field data will be stored as a <see cref="ProjectDataItem"/>
    /// </summary>

    [Flags]
    public enum PortfolioFieldFlags
    {
        Create = 1,
        Read = 1 << 1,
        Update = 1 << 2,
        Delete = 1 << 3,
        ProjectData = 1 << 4,
        EditorCanView = 1 << 5,
        Default = Create | Read | Update | Delete,
        UpdateOnly = Read | Update,
        DefaultProjectData = Default | ProjectData
    }

    public class PortfolioFieldTypeDescriptions : Dictionary<PortfolioFieldType, string>
    {
        public static PortfolioFieldTypeDescriptions Map = new PortfolioFieldTypeDescriptions();
        private PortfolioFieldTypeDescriptions()
        {
            this[PortfolioFieldType.None] = "None";
            this[PortfolioFieldType.Auto] = "Auto-generated by the system";

            this[PortfolioFieldType.Date] = "Select date";
            this[PortfolioFieldType.Month] = "Select month and year";
            this[PortfolioFieldType.Percentage] = "Percentage";
            this[PortfolioFieldType.Budget] = "Budget amount";


            this[PortfolioFieldType.FreeText] = "Free text input";
            this[PortfolioFieldType.SmallFreeTextArea] = "Small multi-line text input";
            this[PortfolioFieldType.MediumFreeTextArea] = "Medium multi-line text input";
            this[PortfolioFieldType.LargeFreeTextArea] = "Large multi-line text input";

            this[PortfolioFieldType.OptionList] = "List of options for the drop-down";
            this[PortfolioFieldType.MultiOptionList] = "List of options for multiple choice";

            this[PortfolioFieldType.PredefinedList] = "Pre-defined list";
            this[PortfolioFieldType.PredefinedSearchableList] = "Pre-defined searchable list";
            this[PortfolioFieldType.PredefinedMultiList] = "Pre-defined multiple choice";
            this[PortfolioFieldType.PredefinedField] = "Pre-defined field";

            this[PortfolioFieldType.ProjectMultiSelect] = "Select multiple projects from searchable list";
            this[PortfolioFieldType.PhaseChoice] = "List of options for the drop-down";
            this[PortfolioFieldType.RAGChoice] = "RAG choice";
            this[PortfolioFieldType.NamedLink] = "Named hyperlink";
            this[PortfolioFieldType.LinkedItemList] = "List of items with links";
            this[PortfolioFieldType.ProjectUpdateText] = "Project update text";
        }
    }

    public class PortfolioLabelConfig
    {
        public int Id { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }
        public int Configuration_Id { get; set; }

        public PortfolioFieldFlags Flags { get; set; }

        [StringLength(50)]
        public string FieldName { get; set; }

        [StringLength(50)]
        public string FieldTitle { get; set; }
        public int FieldOrder { get; set; }

        public bool Included { get; set; }
        public bool IncludedLock { get; set; }
        public bool AdminOnly { get; set; }
        public bool AdminOnlyLock { get; set; }


        [StringLength(50)]
        public string Label { get; set; }

        public PortfolioFieldType FieldType { get; set; }
        public bool FieldTypeLocked { get; set; }

        /// <summary>
        /// The options for list type fields stored as comma separated values
        /// </summary>
        public string FieldOptions { get; set; }

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