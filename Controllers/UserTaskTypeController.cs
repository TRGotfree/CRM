using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Interfaces;
using CRM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserTaskTypeController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ICustomLogger logger;

        public UserTaskTypeController(IRepository repository, ICustomLogger logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(new { data = repository.GetUserTaskTypes().ToArray() });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode(500, Constants.ServerMessage.INTERNAL_SERVER_ERROR);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserTaskType userTaskType)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(Constants.ServerMessage.BAD_REQUEST);

                return Ok(new { data = await repository.SaveUserTaskType(userTaskType) });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode(500, Constants.ServerMessage.INTERNAL_SERVER_ERROR);
            }
        }
    }
}