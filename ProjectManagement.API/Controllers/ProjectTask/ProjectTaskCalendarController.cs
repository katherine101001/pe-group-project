// using Microsoft.AspNetCore.Mvc;
// using ProjectManagement.Application.DTOs.Tasks;
// using ProjectManagement.Application.Interfaces.Services;



// namespace ProjectManagement.API.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ProjectTaskCalendarController : ControllerBase
//     {
//         private readonly IProjectTaskService _projectTaskService;

//         public ProjectTaskCalendarController(IProjectTaskService projectTaskService)
//         {
//             _projectTaskService = projectTaskService;
//         }

//         [HttpGet("tasks")]
//         public async Task<IActionResult> GetMonthlyTaskCalendar(
//             [FromQuery] int year,
//             [FromQuery] int month)
//         {
//             if (year <= 0 || month < 1 || month > 12)
//                 return BadRequest("Invalid year or month.");

//             var result = await _projectTaskService.GetTaskCalendarAsync(year, month);

//             return Ok(result);
//         }
//     }
// }
