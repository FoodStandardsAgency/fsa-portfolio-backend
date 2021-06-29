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
        Contact = 3
    }

    public class ProjectUserCategory
    {
        public ProjectUserCategoryType CategoryType { get; set; }
        public int Order => (int)CategoryType;
        public string ViewKey => $"usercat{Order}";
        public string Name
        {
            get
            {
                string result = "None";
                switch (CategoryType)
                {
                    case ProjectUserCategoryType.Lead:
                        result = "Project lead";
                        break;
                    case ProjectUserCategoryType.Team:
                        result = "Team member";
                        break;
                    case ProjectUserCategoryType.Contact:
                        result = "Key role";
                        break;
                }
                return result;
            }
        }
        public static List<ProjectUserCategory> All()
        {
            return Enum.GetValues(typeof(ProjectUserCategoryType))
                .Cast<ProjectUserCategoryType>()
                .Select(c => new ProjectUserCategory() { CategoryType = c })
                .ToList()
                ;
        }
    }
}
