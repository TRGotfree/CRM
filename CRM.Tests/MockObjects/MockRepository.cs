using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CRM.Interfaces;
using CRM.Models;

namespace CRM.Tests.MockObjects
{
    public class MockRepository : IRepository
    {
        public IEnumerable<UserTask> GetOrderedAndFilteredTasks(int from, int to, string orderBy, string sortBy, string filterBy, string filterValue)
        {
            return new List<UserTask>(0);
        }

        public User GetUser(string login, string password)
        {
            return new User();
        }

        public UserTask[] GetUserTasks(int amountOfTasks)
        {
            return new UserTask[] { };
        }

        public Task<UserTask> SaveNewUserTask(UserTask userTask)
        {
            TaskCompletionSource<UserTask> taskCompletionSource = new TaskCompletionSource<UserTask>();
            taskCompletionSource.SetResult(new UserTask());
            return taskCompletionSource.Task;
        }

        public Task UpdateUserTask(UserTask userTask)
        {
            return Task.CompletedTask;
        }
    }
}
