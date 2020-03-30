using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Models
{
    public class UserTask : IComparable<UserTask>
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public int UserTaskTypeId { get; set; }

        public UserTaskType UserTaskType { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        public int UserTaskStateId { get; set; }
        
        public UserTaskState UserTaskState { get; set; }

        [Required]
        public int PriorityId { get; set; }

        public Priority Priority { get; set; } 

        public int? ExecutorUserId { get; set; } 

        public User ExecutorUser { get; set; }

        [Required]
        public int TaskManagerUserId { get; set; }

        public User TaskManagerUser { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }

        public DateTime ExecuteTaskUntilDate { get; set; }

        [Required]
        public DateTime ChangeDate { get; set; }

        public DateTime CloseDate { get; set; }

        public int? PayloadId { get; set; }

        public Payload Payload { get; set; }

        public int CompareTo([AllowNull] UserTask other)
        {
            if (other == null || this.UserTaskType == null || other.UserTaskType == null)
                return -1;

            return this.UserTaskType.Name.CompareTo(other.UserTaskType.Name);
        }
    }
}
