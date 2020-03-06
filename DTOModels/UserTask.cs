using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.DTOModels
{
    public class UserTask
    {
        public ulong Id { get; set; }

        public string UserTaskType { get; set; }

        [Required]
        public int UserTaskTypeId { get; set; }

        public string Description { get; set; }

        public string UserTaskState { get; set; }

        [Required]
        public int UserTaskStateId { get; set; }

        public string Priority { get; set; }

        [Required]
        public int PriorityId { get; set; }

        public string ExecutorUser { get; set; }

        public int? ExecutorUserId { get; set; }

        public string TaskManagerUser { get; set; }

        [Required]
        public int TaskManagerUserId { get; set; }

        public DateTime ExecuteTaskUntilDate { get; set; }

        public int? PayloadId { get; set; }
    }
}
