using CRM.Constants;
using CRM.Interfaces;
using CRM.Models;
using CRM.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CRM.Services
{
    public class RepositoryProvider : IRepository
    {

        private readonly RepositoryContext repository;

        public RepositoryProvider(RepositoryContext repositoryContext)
        {
            this.repository = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
        }

        public IEnumerable<ExecutorUser> GetExecutorUsers()
        {
            return repository.User
               .Where(us => us.IsActive)
               .Select(u => new ExecutorUser { Id = u.Id, Name = u.Name });
        }

        public IEnumerable<UserTask> GetOrderedAndFilteredTasks(int from, int to, string orderBy, string sortBy, string filterBy, string filterValue)
        {
            try
            {
                var bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
                var propertyToOrder = new DTOModels.UserTask().GetType()
                       .GetProperty(sortBy, bindingFlags);

                if (propertyToOrder == null)
                    throw new InvalidOperationException($"Data couldn't be ordered by this parameter! Parameter name: {sortBy}");

                if (!string.IsNullOrWhiteSpace(filterBy) && !string.IsNullOrWhiteSpace(filterValue))
                {
                    PropertyInfo propertyToFilter = new DTOModels.UserTask().GetType()
                              .GetProperty(filterBy, bindingFlags);

                    if (propertyToFilter == null)
                        throw new InvalidOperationException($"Data couldn't be filtered by this parameter! Parameter name: {filterBy}");

                    var tasks = repository.UserTask
                                             .Include(u => u.Payload)
                                             .Include(u => u.Priority)
                                             .Include(u => u.TaskManagerUser)
                                             .Include(u => u.UserTaskState)
                                             .Include(u => u.UserTaskType)
                                             .Include(u => u.ExecutorUser)
                                             .Where(u => propertyToFilter.GetValue(u, null) != null &&
                                                   propertyToFilter.GetValue(u, null).ToString().Contains(filterValue)).ToList();

                    if (orderBy == OrderBy.ASCENDING)
                        tasks = tasks.OrderBy(t => propertyToOrder.GetValue(t, null)).Skip(from).Take(to - from).ToList();
                    else
                        tasks = tasks.OrderByDescending(t => propertyToOrder.GetValue(t, null)).Skip(from).Take(to - from).ToList();

                    return tasks;
                }

                if (orderBy == OrderBy.ASCENDING)
                {
                    var tasks = repository.UserTask
                                          .Include(u => u.Payload)
                                          .Include(u => u.Priority)
                                          .Include(u => u.TaskManagerUser)
                                          .Include(u => u.UserTaskState)
                                          .Include(u => u.UserTaskType)
                                          .Include(u => u.ExecutorUser)
                                          .OrderBy(t => propertyToOrder.GetValue(t, null)).Skip(from).Take(to - from).ToList();

                    return tasks;
                }
                else
                {
                    var tasks = repository.UserTask
                                          .Include(u => u.Payload)
                                          .Include(u => u.Priority)
                                          .Include(u => u.TaskManagerUser)
                                          .Include(u => u.UserTaskState)
                                          .Include(u => u.UserTaskType)
                                          .Include(u => u.ExecutorUser)
                                          .OrderByDescending(t => propertyToOrder.GetValue(t, null)).Skip(from).Take(to - from).ToList();

                    return tasks;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Models.Priority> GetPriorities()
        {
            return repository.Priority.ToList();
        }

        public User GetUser(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(password);

            return repository.User.Include(u => u.UserRole).FirstOrDefault(u => u.Login == login && u.Password == password);
        }

        public UserTask[] GetUserTasks(int amountOfTasks)
        {
            return repository.UserTask
                      .Include(u => u.Payload)
                      .Include(u => u.Priority)
                      .Include(u => u.TaskManagerUser)
                      .Include(u => u.UserTaskState)
                      .Include(u => u.UserTaskType)
                      .Include(u => u.ExecutorUser)
                      .Where(ut => ut.UserTaskStateId == (int)UserTaskStates.New ||
                             ut.UserTaskStateId == (int)UserTaskStates.Proceed).Take(amountOfTasks).ToArray();
        }

        public IEnumerable<UserTaskState> GetUserTaskStates()
        {
            return repository.UserTaskState.ToList();
        }

        public IEnumerable<UserTaskType> GetUserTaskTypes()
        {
            return repository.UserTaskType.Where(ut => !ut.IsDeleted).ToList();
        }

        public async Task<UserTask> SaveNewUserTask(UserTask userTask)
        {
            if (userTask == null)
                throw new ArgumentNullException(nameof(userTask));

            var user = await repository.AddAsync(userTask);
            await repository.SaveChangesAsync();
            return user.Entity;
        }

        public async Task UpdateUserTask(UserTask userTask)
        {
            if (userTask == null)
                throw new ArgumentNullException(nameof(userTask));

            repository.Update(userTask);
            await repository.SaveChangesAsync();
        }
    }
}
