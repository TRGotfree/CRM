using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.DTOModels
{
    public class User
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string Name { get; set; }
    }
}
