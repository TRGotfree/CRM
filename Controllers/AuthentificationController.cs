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

namespace CRM.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthentificationController : Controller
    {
        private readonly ICustomLogger logger;
        private readonly RepositoryContext repositoryContext;
        private readonly IHashGenerator hashGenerator;
        private readonly IUserIdentityProvider userIdentityProvider;
        private readonly IJWTProvider jWTProvider;

        public AuthentificationController(RepositoryContext repositoryContext, ICustomLogger logger, 
            IHashGenerator hashGenerator, IUserIdentityProvider userIdentityProvider, IJWTProvider jWTProvider)
        {
            this.repositoryContext = repositoryContext ?? throw new ArgumentNullException(nameof(repositoryContext));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.hashGenerator = hashGenerator ?? throw new ArgumentNullException(nameof(hashGenerator));
            this.userIdentityProvider = userIdentityProvider ?? throw new ArgumentNullException(nameof(userIdentityProvider));
            this.jWTProvider = jWTProvider ?? throw new ArgumentNullException(nameof(jWTProvider));
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] DTOModels.User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = ServerMessage.NOT_VALID_PARAMETERS });

                string passwordHash = hashGenerator.GetHash(user.Password);
                var userData = repositoryContext.User.FirstOrDefault(u => u.Login == user.Login && u.Password == passwordHash);
                if (userData == null)
                    return StatusCode(401, new { message = ServerMessage.USER_NOT_AUTHORIZED });

                if (!userData.IsActive)
                    return StatusCode(401, new { message = ServerMessage.USER_ACCOUNT_DEACTIVATED });

                var userIdentityClaim = userIdentityProvider.GetIdentity(userData.Login);
                var jwtToken = jWTProvider.GetToken(userIdentityClaim);

                return Ok(new { token = jWTProvider.WriteToken(jwtToken) });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ServerMessage.INTERNAL_SERVER_ERROR });
            }
        }

    }
}