using CRM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Client
    {

        [Required]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(2000)]
        public string Comment { get; set; }

        [Required]
        public int ClientId { get; set; }

        public virtual ClientType ClientType { get; set; }

        [Required]
        public int CityId { get; set; }

        public virtual City City { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsActive { get; set; }

    }
}
