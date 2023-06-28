using DocumentManager.Data;
using DocumentManager.Handlers;
using DocumentManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusHandler _statusHandler;

        public StatusController(StatusHandler statusHandler)
        {
            _statusHandler = statusHandler;
        }

        [HttpGet("userstatus/{userId}")]
        public async Task<IActionResult> GetUserStatusAndAllStatusesAsync(int userId)
        {
            var result = await _statusHandler.GetUserStatusAndAllStatusesAsync(userId);
            return Ok(result);
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetAllStatusesAsync()
        {
            var result = await _statusHandler.GetAllStatusesAsync();
            return Ok(result);
        }


    }
}
