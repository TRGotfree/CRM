using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Models;

namespace CRM.Interfaces
{
    public interface IRepository
    {
        User GetUser(string login, string password);

        UserTask[] GetUserTasks(int amountOfTasks);

        IEnumerable<UserTask> GetOrderedAndFilteredTasks(int from, int to,
            string orderBy, string sortBy,
            string filterBy, string filterValue);

        Task<UserTask> SaveNewUserTask(UserTask userTask);

        Task UpdateUserTask(UserTask userTask);

        IEnumerable<UserTaskState> GetUserTaskStates();

        IEnumerable<UserTaskType> GetUserTaskTypes();

        IEnumerable<Priority> GetPriorities();

        IEnumerable<ExecutorUser> GetExecutorUsers();
    }
}
