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

        public async Task<IEnumerable<UserTask>> GetOrderedAndFilteredTasks(User user, int from, int to, string orderBy, string sortBy, string filterBy, string filterValue)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
                var propertyToOrder = new Models.UserTask().GetType()
                       .GetProperty(sortBy, bindingFlags);

                if (propertyToOrder == null)
                    throw new InvalidOperationException($"Data couldn't be ordered by this parameter! Parameter name: {sortBy}");

                if (!string.IsNullOrWhiteSpace(filterBy) && !string.IsNullOrWhiteSpace(filterValue))
                {
                    PropertyInfo propertyToFilter = new Models.UserTask().GetType()
                              .GetProperty(filterBy, bindingFlags);

                    if (propertyToFilter == null)
                        throw new InvalidOperationException($"Data couldn't be filtered by this parameter! Parameter name: {filterBy}");

                    var tasks = await repository.UserTask
                                             .Include(u => u.Payload)
                                             .Include(u => u.Priority)
                                             .Include(u => u.TaskManagerUser)
                                             .Include(u => u.UserTaskState)
                                             .Include(u => u.UserTaskType)
                                             .Include(u => u.ExecutorUser).Where(u => u.ExecutorUserId == user.Id).ToListAsync();

                    tasks = tasks.Where(u => propertyToFilter.GetValue(u, null) != null &&
                                                   propertyToFilter.GetValue(u, null).ToString().Contains(filterValue)).ToList();

                    if (orderBy == OrderBy.ASCENDING)
                        tasks = tasks.OrderBy(t => propertyToOrder.GetValue(t, null)).Skip(from).Take(to - from).ToList();
                    else
                        tasks = tasks.OrderByDescending(t => propertyToOrder.GetValue(t, null)).Skip(from).Take(to - from).ToList();

                    return tasks;
                }

                if (orderBy == OrderBy.ASCENDING)
                {
                    var tasks = await repository.UserTask
                                          .Include(u => u.Payload)
                                          .Include(u => u.Priority)
                                          .Include(u => u.TaskManagerUser)
                                          .Include(u => u.UserTaskState)
                                          .Include(u => u.UserTaskType)
                                          .Include(u => u.ExecutorUser).Where(u => u.ExecutorUserId == user.Id).ToListAsync();
                    tasks = tasks
                            .Where(u => propertyToOrder.GetValue(u, null) != null)
                            .OrderBy(u => propertyToOrder.GetValue(u, null)).Skip(from).Take(to - from).ToList();

                    return tasks;
                }
                else
                {
                    var tasks = await repository.UserTask
                                          .Include(u => u.Payload)
                                          .Include(u => u.Priority)
                                          .Include(u => u.TaskManagerUser)
                                          .Include(u => u.UserTaskState)
                                          .Include(u => u.UserTaskType)
                                          .Include(u => u.ExecutorUser).Where(u => u.ExecutorUserId == user.Id).ToListAsync();
                    tasks = tasks
                    .Where(u => propertyToOrder.GetValue(u, null) != null)
                    .OrderByDescending(u => propertyToOrder.GetValue(u, null)).Skip(from).Take(to - from).ToList();

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

        public async Task<User> GetUser(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(password);

            return await repository.User.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        }

        public async Task<User> GetUser(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));

            return await repository.User.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<UserTask[]> GetUserTasks(User user, int amountOfTasks)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return await repository.UserTask
                      .Include(u => u.Payload)
                      .Include(u => u.Priority)
                      .Include(u => u.TaskManagerUser)
                      .Include(u => u.UserTaskState)
                      .Include(u => u.UserTaskType)
                      .Include(u => u.ExecutorUser)
                      .Where(ut => ut.ExecutorUserId == user.Id && ut.UserTaskStateId == (int)UserTaskStates.New ||
                             ut.UserTaskStateId == (int)UserTaskStates.Proceed).Take(amountOfTasks).ToArrayAsync();
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

            await repository.AddAsync(userTask);
            await repository.SaveChangesAsync();
            return userTask;
        }

        public async Task<UserTaskType> SaveUserTaskType(UserTaskType userTaskType)
        {
            if (userTaskType == null)
                throw new ArgumentNullException(nameof(userTaskType));

            if (userTaskType.Id <= 0)
            {
                userTaskType = new UserTaskType { Name = userTaskType.Name };
                await repository.UserTaskType.AddAsync(userTaskType);
            }
            else
                repository.UserTaskType.Update(userTaskType);

            await repository.SaveChangesAsync();
            return userTaskType;
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
