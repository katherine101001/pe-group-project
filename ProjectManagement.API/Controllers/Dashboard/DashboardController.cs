using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Dashboard;
using ProjectManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [AllowAnonymous]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats(
            [FromQuery] Guid userId,
            [FromQuery] string role)
        {
            if (userId == Guid.Empty || string.IsNullOrEmpty(role))
                return BadRequest("Missing user context");

            DashboardStatsDto stats;

            if (role == "ADMIN")
            {
                stats = await _dashboardService.GetAdminDashboardStatsAsync();
            }
            else
            {
                stats = await _dashboardService.GetUserDashboardStatsAsync(userId);
            }

            return Ok(stats);
        }


    }

}
