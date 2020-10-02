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

        [StringLength(50)]
        public string G6team { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
