using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int UserRoleId { get; set; }

        public virtual UserRole UserRole { get; set; }
    }
}
