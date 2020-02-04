using CRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Employee
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Position { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
