using FSAPortfolio.Common;
using FSAPortfolio.Entities.Projects;
using FSAPortfolio.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Organisation
{
    public class Portfolio
    {
        public int Id { get; set; }

        [StringLength(10)]
        public string ViewKey { get; set; }

        [StringLength(20)]
        public string ShortName{ get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(10)]
        public string IDPrefix { get; set; }

        public virtual PortfolioConfiguration Configuration { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Team> Teams { get; set; }

        public string RequiredRoleData { get; set; }

        public Role[] RequiredRoles
        {
            get
            {
                switch(RequiredRoleData?.ToLower())
                {
                    case "superuser":
                        return new Role[] { new Role(IDPrefix, AccessGroupConstants.SuperuserViewKey) };
                    case "admin":
                        return new Role[] { new Role(IDPrefix, AccessGroupConstants.SuperuserViewKey), new Role(IDPrefix, AccessGroupConstants.AdminViewKey) };
                    case "editor":
                        return new Role[] { new Role(IDPrefix, AccessGroupConstants.SuperuserViewKey), new Role(IDPrefix, AccessGroupConstants.AdminViewKey), new Role(IDPrefix, AccessGroupConstants.EditorViewKey) };
                    case "fsa":
                        return new Role[] { new Role(IDPrefix, AccessGroupConstants.SuperuserViewKey), new Role(IDPrefix, AccessGroupConstants.AdminViewKey), new Role(IDPrefix, AccessGroupConstants.EditorViewKey), new Role(IDPrefix, AccessGroupConstants.FSAViewKey) };
                    case "read":
                    default:
                        return new Role[] { new Role(IDPrefix, AccessGroupConstants.SuperuserViewKey), new Role(IDPrefix, AccessGroupConstants.AdminViewKey), new Role(IDPrefix, AccessGroupConstants.EditorViewKey), new Role(IDPrefix, AccessGroupConstants.FSAViewKey), new Role(IDPrefix, "Read") };
                }
            }
        }

    }
}
