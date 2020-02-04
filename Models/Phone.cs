using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Phone
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Number { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
