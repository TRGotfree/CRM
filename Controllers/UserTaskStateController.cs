﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRM.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskStateController : ControllerBase
    {
        private readonly IRepository repository;
        private readonly ICustomLogger logger;

        public UserTaskStateController(IRepository repository, ICustomLogger logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(new { data = repository.GetUserTaskStates().ToArray() });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode(500, Constants.ServerMessage.INTERNAL_SERVER_ERROR);
            }
        }
    }
}