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
    public class PriorityController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ICustomLogger logger;

        public PriorityController(IRepository repository, ICustomLogger logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(new { data = repository.GetPriorities().ToArray() });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode(500, Constants.ServerMessage.INTERNAL_SERVER_ERROR);
            }
        }
    }
}