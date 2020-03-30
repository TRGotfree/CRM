using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class UserTaskType : IComparable<UserTaskType>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public int CompareTo(UserTaskType other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
