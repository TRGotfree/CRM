using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Models;

namespace CRM.Interfaces
{
    public interface IRepository
    {
        Task<User> GetUser(string login, string password);

        Task<User> GetUser(string login);

        Task<UserTask[]> GetUserTasks(User user, int amountOfTasks);

        Task<IEnumerable<UserTask>> GetOrderedAndFilteredTasks(User user, int from, int to,
            string orderBy, string sortBy,
            string filterBy, string filterValue);

        Task<UserTask> SaveNewUserTask(UserTask userTask);

        Task UpdateUserTask(UserTask userTask);

        IEnumerable<UserTaskState> GetUserTaskStates();

        IEnumerable<UserTaskType> GetUserTaskTypes();

        Task<UserTaskType> SaveUserTaskType(UserTaskType userTaskType);

        IEnumerable<Priority> GetPriorities();

        IEnumerable<ExecutorUser> GetExecutorUsers();
    }
}
