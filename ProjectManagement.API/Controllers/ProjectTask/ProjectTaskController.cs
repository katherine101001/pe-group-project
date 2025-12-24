using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.Interfaces.Services;

[ApiController]
[Route("api/tasks")]
public class ProjectTaskController : ControllerBase
{
    private readonly IProjectTaskService _projectTaskService;

    public ProjectTaskController(IProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProjectTask([FromBody] CreateProjectTaskDto dto)
    {
        await _projectTaskService.CreateProjectTaskAsync(dto);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProject(Guid id)
    {
        var project = await _projectTaskService.GetProjectTaskByIdAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _projectTaskService.GetAllProjectTasksAsync();
        return Ok(projects);
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetTasksByProjectId(Guid projectId)
    {
        var tasks = await _projectTaskService.GetTasksByProjectIdAsync(projectId);
        return Ok(tasks);
    }


    [HttpGet("calendar")]
    public async Task<IActionResult> GetTaskCalendar([FromQuery] int year, [FromQuery] int month)
    {
        if (year <= 0 || month < 1 || month > 12)
            return BadRequest("Invalid year or month.");

        var result = await _projectTaskService.GetTaskCalendarAsync(year, month);

        return Ok(result);
    }

    [HttpGet("overdue")]
    public async Task<IActionResult> GetAllOverdueTasks()
    {
        var overdueTasks = await _projectTaskService.GetAllOverdueTasksAsync();
        return Ok(overdueTasks);
    }


    [HttpGet("overdue/{projectId}")]
    public async Task<IActionResult> GetOverdueTasksByProject(Guid projectId)
    {
        var overdueTasks = await _projectTaskService.GetOverdueTasksByProjectIdAsync(projectId);
        return Ok(overdueTasks);
    }

    [HttpGet("overdue/count")]
    public async Task<IActionResult> GetSoonOverdueTaskCount()
    {
        var count = await _projectTaskService.GetSoonToOverdueTaskCountAsync();
        return Ok(new { count });
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentTasks([FromQuery] int limit = 5)
    {
        var result = await _projectTaskService.GetRecentTasksAsync(limit);
        return Ok(result);
    }

    // [HttpGet("my-tasks")]
    // public async Task<IActionResult> GetMyTasks()
    // {
    //     // 假设你已经有用户 Id，从 token 或 session 获取
    //     var userIdClaim = User.FindFirst("72da7c5e-a07c-4e30-b59c-a6a4fc164f58");
    //     if (userIdClaim == null)
    //         return Unauthorized("User not found.");

    //     if (!Guid.TryParse(userIdClaim.Value, out var userId))
    //         return Unauthorized("Invalid user ID.");

    //     var tasks = await _projectTaskService.GetTasksByUserAsync(userId);

    //     return Ok(tasks);
    // }



    [HttpGet("my-tasks/{userId:guid}")]
    public async Task<IActionResult> GetMyTasks(Guid userId)
    {
        var tasks = await _projectTaskService.GetTasksByUserAsync(userId);
        return Ok(tasks);
    }


    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectTaskDto dto)
    // {
    //     await _projectTaskService.UpdateProjectTaskAsync(id, dto);
    //     return NoContent();
    // }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] UpdateTaskStatusDto dto)
    {
        await _projectTaskService.UpdateTaskStatusAsync(id, dto.Status);
        return NoContent();
    }




    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _projectTaskService.DeleteProjectTaskAsync(id);
        return NoContent();
    }


}
