// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using ProjectManagement.Application.Interfaces.Services;

// namespace ProjectManagement.API.Controllers.Dashboard
// {
//     [ApiController]
//     [Route("api/dashboard")]
//     //[Authorize]
//     public class DashboardController : ControllerBase
//     {
//         private readonly IDashboardService _dashboardService;

//         public DashboardController(IDashboardService dashboardService)
//         {
//             _dashboardService = dashboardService;
//         }

//         [HttpGet("projects")]
//         public async Task<IActionResult> GetProjectDashboardStats()
//         {
//             var userIdClaim = User.FindFirst("id");
//             if (userIdClaim == null)
//                 return Unauthorized("UserId not found in token");

//             if (!Guid.TryParse(userIdClaim.Value, out var userId))
//                 return Unauthorized("Invalid UserId");

//             var result = await _dashboardService.GetDashboardProjectStatsAsync(userId);

//             return Ok(result);
//         }

//         [HttpGet("projects/test")]
//         public async Task<IActionResult> GetProjectDashboardStatsTest([FromQuery] Guid userId)
//         {
//             var result = await _dashboardService.GetDashboardProjectStatsAsync(userId);
//             return Ok(result);
//         }

//         // [HttpGet("tasks-summary")]
//         // public async Task<IActionResult> GetTasksSummary()
//         // {
//         //     var userIdClaim = User.FindFirst("id");
//         //     if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
//         //         return Unauthorized();

//         //     var result = await _dashboardService.GetDashboardTasksAsync(userId);
//         //     return Ok(result);
//         // }

//     }
// }
