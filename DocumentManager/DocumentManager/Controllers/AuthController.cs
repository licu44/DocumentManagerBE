using DocumentManager.Data;
using DocumentManager.Handlers;
using DocumentManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DocumentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserHandler _userHandler;
        public AuthController(UserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Account request)
        {
            var result = _userHandler.CreateUser(request);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to register user");
            }
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginRequest request)
        {
            var result = _userHandler.VerifyUser(request.Username, request.Password);

            if (result != null && !string.IsNullOrEmpty(result.Token))
            {
                return Ok(result);
            }
            else
            {
                return Unauthorized("Username or password is not correct.");
            }
        }


    }
}
