using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAPortfolio.Entities.Users
{
    public class User
    {
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(300)]
        public string PasswordHash { get; set; }

        public int AccessGroupId { get; set; }
        public virtual AccessGroup AccessGroup { get; set; }
    }
}
