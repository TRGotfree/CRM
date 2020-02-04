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
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phones { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }

        public string HeadManager { get; set; }

        public string AccountManager { get; set; }

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
