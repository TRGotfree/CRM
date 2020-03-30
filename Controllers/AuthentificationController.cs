using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CRM.Constants;
using CRM.DTOModels;
using CRM.Models;
using CRM.Interfaces;
using CRM.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CRM.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthentificationController : Controller
    {
        private readonly ICustomLogger logger;
        private readonly IRepository repository;
        private readonly IHashGenerator hashGenerator;
        private readonly IUserIdentityProvider userIdentityProvider;
        private readonly IJWTProvider jWTProvider;

        public AuthentificationController(IRepository repository, ICustomLogger logger, 
            IHashGenerator hashGenerator, IUserIdentityProvider userIdentityProvider, IJWTProvider jWTProvider)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.hashGenerator = hashGenerator ?? throw new ArgumentNullException(nameof(hashGenerator));
            this.userIdentityProvider = userIdentityProvider ?? throw new ArgumentNullException(nameof(userIdentityProvider));
            this.jWTProvider = jWTProvider ?? throw new ArgumentNullException(nameof(jWTProvider));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] DTOModels.User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = ServerMessage.NOT_VALID_PARAMETERS });

                string passwordHash = hashGenerator.GetHash(user.Password);
                var userData = await repository.GetUser(user.Login, passwordHash);
                if (userData == null)
                    return StatusCode(401, new { message = ServerMessage.USER_NOT_AUTHORIZED });

                if (!userData.IsActive)
                    return StatusCode(401, new { message = ServerMessage.USER_ACCOUNT_DEACTIVATED });

                var userIdentityClaim = userIdentityProvider.GetIdentity(userData.Login);
                var jwtToken = jWTProvider.GetToken(userIdentityClaim);

                return Ok(new { token = jWTProvider.WriteToken(jwtToken), user = 
                    new CRM.DTOModels.User { 
                        Login = userData.Login, 
                        Name = userData.Name, 
                        RoleId = userData.UserRoleId,
                        RoleName = userData.UserRole.Name
                    } });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ServerMessage.INTERNAL_SERVER_ERROR });
            }
        }

    }
}