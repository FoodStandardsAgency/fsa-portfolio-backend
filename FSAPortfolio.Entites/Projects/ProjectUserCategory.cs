using FSAPortfolio.Common;
using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Projects
{
    public enum ProjectUserCategoryType
    {
        None = 0,
        Lead = 1,
        Team = 2,
        Contact1 = 3,
        Contact2 = 4,
        Contact3 = 5,
    }

    public class ProjectUserCategory
    {
        static Dictionary<ProjectUserCategoryType, string> labelFieldMap = new Dictionary<ProjectUserCategoryType, string>() {
            { ProjectUserCategoryType.Lead, ProjectPropertyConstants.ProjectLead },
            { ProjectUserCategoryType.Team, ProjectPropertyConstants.team },
            { ProjectUserCategoryType.Contact1, ProjectPropertyConstants.key_contact1 },
            { ProjectUserCategoryType.Contact2, ProjectPropertyConstants.key_contact2 },
            { ProjectUserCategoryType.Contact3, ProjectPropertyConstants.key_contact3 }
        };

        private ProjectUserCategory(ProjectUserCategoryType type, ICollection<PortfolioLabelConfig> labels)
        {
            this.CategoryType = type;
            if (type != ProjectUserCategoryType.None)
            {
                this.Label = labels.Single(l => l.FieldName == labelFieldMap[type]);
            }
        }

        public ProjectUserCategoryType CategoryType { get; }
        public PortfolioLabelConfig Label { get; }
        public int Order => (int)CategoryType;
        public string ViewKey => $"usercat{Order}";
        public string Name => Label?.Label ?? Label?.FieldTitle ?? "None";
        public static List<ProjectUserCategory> All(ICollection<PortfolioLabelConfig> labels)
        {
            return Enum.GetValues(typeof(ProjectUserCategoryType))
                .Cast<ProjectUserCategoryType>()
                .Select(c => new ProjectUserCategory(c, labels))
                .ToList();
        }
    }
}
