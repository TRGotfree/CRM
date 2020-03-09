using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.DTOModels
{
    public class UserTask
    {
        [Display(Name = "Id задачи")]
        public ulong Id { get; set; }

        [Display(Name = "Тип задачи")]
        public string UserTaskType { get; set; }

        [Required]
        public int UserTaskTypeId { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Статус задачи")]
        public string UserTaskState { get; set; }

        [Required]
        public int UserTaskStateId { get; set; }

        [Display(Name = "Приоритет")]
        public string Priority { get; set; }

        [Required]
        public int PriorityId { get; set; }

        [Display(Name = "Исполнитель")]
        public string ExecutorUser { get; set; }

        public int? ExecutorUserId { get; set; }

        [Display(Name = "Постановщик задачи")]
        public string TaskManagerUser { get; set; }

        [Required]
        public int TaskManagerUserId { get; set; }

        [Display(Name = "Срок выполнения задачи")]
        public DateTime ExecuteTaskUntilDate { get; set; }

        public int? PayloadId { get; set; }

        [Display(Name = "Файл")]
        public bool IsPayloadExists { get; set; }
    }
}
