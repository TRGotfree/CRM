﻿using System;
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
        public int UserTaskTypeId { get; set; }

        public string Description { get; set; }

        [Display(Name = "Статус")]
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

        [Display(Name = "Постановщик")]
        public string TaskManagerUser { get; set; }

        [Required]
        public int TaskManagerUserId { get; set; }

        [Display(Name = "Срок выполнения")]
        public DateTime ExecuteTaskUntilDate { get; set; }

        public int? PayloadId { get; set; }

        public bool IsPayloadExists { get; set; }
    }
}
