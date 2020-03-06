using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using CRM.Constants;
using CRM.Interfaces;
using CRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserTaskController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly Repository.RepositoryContext repository;
        private readonly ITransformer modelTransformer;

        public UserTaskController(ICustomLogger logger, Repository.RepositoryContext repository, ITransformer modelTransformer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.modelTransformer = modelTransformer;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int numberOfRows)
        {
            try
            {
                if (numberOfRows > 100)
                    return BadRequest(new { message = "Couldn't return this count of tasks!" });

                var newTasksInProcess = repository.UserTask
                     .Include(u => u.Payload)
                     .Include(u => u.Priority)
                     .Include(u => u.TaskManagerUser)
                     .Include(u => u.UserTaskState)
                     .Include(u => u.UserTaskType)
                     .Include(u => u.ExecutorUser)
                     .Where(ut => ut.UserTaskStateId == (int)UserTaskStates.New ||
                            ut.UserTaskStateId == (int)UserTaskStates.Proceed).Take(numberOfRows).ToArray();

                if (newTasksInProcess == null || newTasksInProcess.Length == 0)
                    return Ok(new { data = newTasksInProcess });

                return Ok(new { data = modelTransformer.UserTasksToDTOModels(newTasksInProcess).ToArray() });

            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ServerMessage.INTERNAL_SERVER_ERROR });
            }
        }

        [HttpGet()]
        public IActionResult Get(
            [FromQuery]int from, [FromQuery]int to,
            [FromQuery]string orderBy, [FromQuery]string sortBy,
            [FromQuery]string filterBy, [FromQuery]string filterValue)
        {
            try
            {
                if ((to - from) > 20)
                    return BadRequest(new { message = "number of rows couldn't be larger than 20" });

                if (string.IsNullOrEmpty(orderBy))
                    return BadRequest(new { message = "orderBy parameter not specified!" });

                if (orderBy != OrderBy.ASCENDING && orderBy != OrderBy.DESCENDING)
                    return BadRequest(new { message = "This type of orderBy parameter not supported!" });

                if (string.IsNullOrEmpty(sortBy))
                    return BadRequest(new { message = "sortBy parameter not specified!" });

                var bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
                var propertyToOrder = new DTOModels.UserTask().GetType()
                       .GetProperty(sortBy, bindingFlags);

                if (propertyToOrder == null)
                    return StatusCode(400, new { message = $"Data couldn't be ordered by this parameter! Parameter name: {sortBy}" });

                if (!string.IsNullOrWhiteSpace(filterBy) && !string.IsNullOrWhiteSpace(filterValue))
                {
                    PropertyInfo propertyToFilter = new DTOModels.UserTask().GetType()
                              .GetProperty(filterBy, bindingFlags);

                    if (propertyToFilter == null)
                        return StatusCode(400, new { message = $"Data couldn't be filtered by this parameter! Parameter name: {filterBy}" });
                    
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

                    return Ok(new { data = modelTransformer.UserTasksToDTOModels(tasks).ToArray() });
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

                    return Ok(new { data = modelTransformer.UserTasksToDTOModels(tasks).ToArray() });
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

                    return Ok(new { data = modelTransformer.UserTasksToDTOModels(tasks).ToArray() });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ServerMessage.INTERNAL_SERVER_ERROR });
            }
        }

        // POST: api/UserTask
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTOModels.UserTask userTask)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Data is not valid" });

                var userTaskModel = modelTransformer.UserTaskDTOModelToModel(userTask);

                if (userTask.Id <= 0)
                    await repository.AddAsync(userTaskModel);
                else
                    repository.Update(userTaskModel);

                await repository.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ServerMessage.INTERNAL_SERVER_ERROR });
            }

        }
 
    }
}
