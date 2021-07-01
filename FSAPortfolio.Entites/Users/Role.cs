using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Users
{
    public class Role
    {
        public string Portfolio { get; }
        public string Permission { get; }
        public Role(string portfolio, string permission)
        {
            this.Portfolio = portfolio.ToLower();
            this.Permission = permission.ToLower();
            this.ViewKey = $"{this.Portfolio}.{this.Permission}";
        }
        public Role(string viewKey)
        {
            this.ViewKey = viewKey.Trim().ToLower();
            var parts = this.ViewKey.Split('.');
            if(parts.Length == 2)
            {
                this.Portfolio = parts[0];
                this.Permission = parts[1];
            }
            else
            {
                this.Permission = this.ViewKey;
            }
        }

        public string ViewKey { get; set; }

        public override bool Equals(Object obj)
        {
            Role role = obj as Role;
            if (role == null)
                return false;
            else
                return ViewKey.Equals(role.ViewKey);
        }

        public override int GetHashCode()
        {
            return this.ViewKey.GetHashCode();
        }

    }
}
