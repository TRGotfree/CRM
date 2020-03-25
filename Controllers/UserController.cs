using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ICustomLogger logger;

        public UserController(IRepository repository, ICustomLogger logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("executor")]
        public IActionResult GetExecutor()
        {
            try
            {
                return Ok(new { data = repository.GetExecutorUsers().ToArray() });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode(500, Constants.ServerMessage.INTERNAL_SERVER_ERROR);
            }
        }
    }
}