using FSAPortfolio.Entities.Organisation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Users
{
    public class Person
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Surname { get; set; }

        [StringLength(250)]
        public string Firstname { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public Team Team { get; set; }
        public int? Team_Id { get; set; }

        [StringLength(150)]
        public string ActiveDirectoryPrincipalName { get; set; }

        [StringLength(150)]
        public string ActiveDirectoryId { get; set; }

        [StringLength(500)]
        public string ActiveDirectoryDisplayName { get; set; }


        [StringLength(150)]
        public string Department { get; set; }


        public DateTime Timestamp { get; set; }


        public string DisplayName
        {
            get
            {
                string result = null;
                if(!string.IsNullOrWhiteSpace(ActiveDirectoryDisplayName))
                {
                    result = DisplayName;
                }
                else if (!string.IsNullOrWhiteSpace(Firstname) && !string.IsNullOrWhiteSpace(Surname))
                {
                    result = $"{Firstname} {Surname}";
                }
                else if (!string.IsNullOrWhiteSpace(Email))
                {
                    result = Email;
                }
                else if (!string.IsNullOrWhiteSpace(ActiveDirectoryPrincipalName))
                {
                    result = ActiveDirectoryPrincipalName;
                }
                return result;
            }
        }

        public string ViewKey => ActiveDirectoryPrincipalName ?? Email;
    }
}
