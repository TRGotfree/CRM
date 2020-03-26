using CRM.DTOModels;
using CRM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.Services
{
    public class ModelTransformer : ITransformer
    {
        public Models.UserTask UserTaskDTOModelToModel(UserTask userTask)
        {
            if (userTask == null)
                throw new ArgumentNullException(nameof(userTask));

            Models.UserTask userTaskModel = new Models.UserTask
            {
                ExecutorUserId = userTask.ExecutorUserId,
                Description = userTask.Description,
                ExecuteTaskUntilDate = userTask.ExecuteTaskUntilDate,
                Id = (long)userTask.Id,
                PayloadId = userTask.PayloadId,
                PriorityId = userTask.PriorityId,
                TaskManagerUserId = userTask.TaskManagerUserId,
                UserTaskStateId = userTask.UserTaskStateId,
                UserTaskTypeId = userTask.UserTaskTypeId
            };

            return userTaskModel;
        }

        public IEnumerable<UserTask> UserTasksToDTOModels(IEnumerable<Models.UserTask> userTasks)
        {
            if (userTasks == null)
                throw new ArgumentNullException(nameof(userTasks));

            List<UserTask> dtoUserTaskList = new List<UserTask>();
            var listOfTasks = userTasks.ToList();

            for (int i = 0; i < listOfTasks.Count; i++)
                dtoUserTaskList.Add(UserTaskToDTOModel(listOfTasks[i]));

            return dtoUserTaskList;
        }

        public UserTask UserTaskToDTOModel(Models.UserTask userTaskModel)
        {
            if (userTaskModel == null)
                throw new ArgumentNullException(nameof(userTaskModel));

            return new UserTask
            {
                Id = (ulong)userTaskModel.Id,
                Description = userTaskModel.Description,
                ExecuteTaskUntilDate = userTaskModel.ExecuteTaskUntilDate,
                ExecuteTaskUntilDateString = userTaskModel.ExecuteTaskUntilDate.ToString("dd.MM.yyyy"),
                ExecutorUser = userTaskModel.ExecutorUser?.Name,
                ExecutorUserId = userTaskModel.ExecutorUserId,
                PayloadId = userTaskModel.PayloadId,
                Priority = userTaskModel.Priority?.Name,
                PriorityId = userTaskModel.PriorityId,
                TaskManagerUser = userTaskModel.TaskManagerUser?.Name,
                TaskManagerUserId = userTaskModel.TaskManagerUserId,
                UserTaskState = userTaskModel.UserTaskState?.Name,
                UserTaskStateId = userTaskModel.UserTaskStateId,
                UserTaskType = userTaskModel.UserTaskType?.Name,
                UserTaskTypeId = userTaskModel.UserTaskTypeId
            };
        }
    }
}
