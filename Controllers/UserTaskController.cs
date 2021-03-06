﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserTaskController : ControllerBase
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;
        private readonly ITransformer modelTransformer;

        public UserTaskController(ICustomLogger logger, IRepository repository, ITransformer modelTransformer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.modelTransformer = modelTransformer;
        }

        [HttpGet]
        [Route("new")]
        public async Task<IActionResult> Get([FromQuery]string login, [FromQuery]int numberOfRows)
        {
            try
            {
                if (numberOfRows > 100)
                    return BadRequest(new { message = "Couldn't return this count of tasks!" });

                if (string.IsNullOrWhiteSpace(login))
                    return BadRequest(new { message = "login not specified!" });

                var user = await repository.GetUser(login);

                if (user == null)
                    return BadRequest(new { message = "User with certain login not found!" });
         
                var newTasksInProcess = await repository.GetUserTasks(user, numberOfRows);

                if (newTasksInProcess == null || newTasksInProcess.Length == 0)
                    return Ok(new { data = newTasksInProcess });

                return Ok(new { data = modelTransformer.UserTasksToDTOModels(newTasksInProcess).ToArray(), total = newTasksInProcess.Length });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ServerMessage.INTERNAL_SERVER_ERROR });
            }
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery]string login,
            [FromQuery]int from, [FromQuery]int to,
            [FromQuery]string orderBy, [FromQuery]string sortBy,
            [FromQuery]string filterBy, [FromQuery]string filterValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login))
                    return BadRequest(new { message = "login not specified!" });

                if ((to - from) > 30)
                    return BadRequest(new { message = "number of rows couldn't be larger than 20" });

                if (string.IsNullOrEmpty(orderBy))
                    return BadRequest(new { message = "orderBy parameter not specified!" });

                if (orderBy != OrderBy.ASCENDING && orderBy != OrderBy.DESCENDING)
                    return BadRequest(new { message = "This type of orderBy parameter not supported!" });

                if (string.IsNullOrEmpty(sortBy))
                    return BadRequest(new { message = "sortBy parameter not specified!" });
                
                var user = await repository.GetUser(login);
                if (user == null)
                    return BadRequest(new { message = "User with certain login not found!" });

                var tasks = await repository.GetOrderedAndFilteredTasks(user, from, to, orderBy, sortBy, filterBy, filterValue);
                
                return Ok(new { data = modelTransformer.UserTasksToDTOModels(tasks).ToArray(), total = tasks.Count() });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ServerMessage.INTERNAL_SERVER_ERROR });
            }
        }

        [HttpGet()]
        [Route("meta")]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                Dictionary<string, string> propsAndDisplayNames = new Dictionary<string, string>();
                var bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                var userTaskProperties = typeof(DTOModels.UserTask).GetProperties(bindingFlags);
                userTaskProperties = userTaskProperties.Where(ut => ut.CustomAttributes != null).ToArray();

                if (userTaskProperties == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, new { message = "MetaData for user tasks not found!" });

                foreach (var prop in userTaskProperties)
                {
                    var displayAttribute = prop.GetCustomAttributes(typeof(DisplayAttribute), true).Select(attr => (DisplayAttribute)attr).FirstOrDefault() as DisplayAttribute;
                    if (displayAttribute == null)
                        continue;

                    propsAndDisplayNames.Add(string.Format("{0}{1}", prop.Name.Substring(0, 1).ToLower(), prop.Name.Substring(1)), displayAttribute.Name);
                }

                return Ok(new { data = propsAndDisplayNames.ToHashSet() });
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
                var user = await repository.GetUser(userTask.TaskManagerUserLogin);

                if (user == null)
                    return BadRequest(new { message = "User with certain login not found!" });

                userTaskModel.TaskManagerUser = user;

                if (userTask.Id <= 0)
                   userTaskModel = await repository.SaveNewUserTask(userTaskModel);
                else
                    await repository.UpdateUserTask(userTaskModel);

                return Ok(new { data = modelTransformer.UserTaskToDTOModel(userTaskModel) });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ServerMessage.INTERNAL_SERVER_ERROR });
            }
        }

    }
}
