using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.DTOModels
{
    public class UserTask
    {
        [Display(Name = "Id")]
        public ulong Id { get; set; }

        [Display(Name = "Тип")]
        public string UserTaskType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int UserTaskTypeId { get; set; }

        public string Description { get; set; }

        [Display(Name = "Статус")]
        public string UserTaskState { get; set; }

        
        public int UserTaskStateId { get; set; }

        [Display(Name = "Приоритет")]
        public string Priority { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PriorityId { get; set; }

        [Display(Name = "Исполнитель")]
        public string ExecutorUser { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? ExecutorUserId { get; set; }

        [Display(Name = "Постановщик")]
        public string TaskManagerUser { get; set; }

        [Required]
        public string TaskManagerUserLogin { get; set; }

        public int TaskManagerUserId { get; set; }

        
        public DateTime ExecuteTaskUntilDate { get; set; }

        [Display(Name = "Срок выполнения")]
        public string ExecuteTaskUntilDateString { get; set; }

        public int? PayloadId { get; set; }

        public bool IsPayloadExists { get; set; }
    }
}
