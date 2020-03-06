using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class Payload
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string DataType { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
