using DocumentManager.Data;
using DocumentManager.Handlers;
using DocumentManager.Models;
using DocumentManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            return Ok(_userHandler.CreateUser(request));
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(Account request)
        {
            var test = _userHandler.VerifyUser(request);
            return Ok(test);
        }
    }
}
