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
        [HttpGet("types")]
        public async Task<IActionResult> GetGeneratedDocTypes()
        {
            var documentTypes = await _adminHandler.GetGeneratedDocTypes();

            if (documentTypes == null || !documentTypes.Any())
            {
                return NotFound("No document types found");
            }

            return Ok(documentTypes);
        }
        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteGeneratedDocType(int id)
        {
            var deleted = await _adminHandler.DeleteGeneratedDocType(id);

            if (deleted)
            {
                return Ok();
            }

            return NotFound();
        }
        [HttpPost("documents")]
        public async Task<IActionResult> AddDocument(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            string documentName = await _adminHandler.SaveDocument(file);

            return Ok(documentName);
        }
        [HttpPut("generatedocs/{id}/restricted")]
        public async Task<IActionResult> UpdateGeneratedDocRestricted(int id, [FromBody] bool restricted)
        {
            var updated = await _adminHandler.UpdateGeneratedDocRestricted(id, restricted);

            if (updated)
            {
                return Ok();
            }

            return NotFound();
        }

    }
}
