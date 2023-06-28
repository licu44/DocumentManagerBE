using DocumentManager.Data;
using DocumentManager.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManager.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminHandler _adminHandler;
        public AdminController(AdminHandler adminHandler)
        {
            _adminHandler = adminHandler;
        }
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _adminHandler.GetAllUsers();

            if (users == null || !users.Any())
            {
                return NotFound("No users found");
            }

            return Ok(users);
        }
        [HttpPost("userstatus/{userId}")]
        public async Task<IActionResult> AddOrUpdateUserStatusAsync(int userId, [FromBody] UserStatusDto request)
        {
            var updatedUserStatus = await _adminHandler.AddOrUpdateUserStatusAsync(userId, request.FeedbackStatusId, request.AuthorizationStatusId, request.EngineeringStatusId);
            return Ok(updatedUserStatus);
        }

    }
}
