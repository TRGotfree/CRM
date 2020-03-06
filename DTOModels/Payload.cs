using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.DTOModels
{
    public class Payload
    {
        public int Id { get; set; }
        
        [Required]
        public byte[] Data { get; set; }

        [Required]
        [MaxLength(50)]
        public string DataType { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
