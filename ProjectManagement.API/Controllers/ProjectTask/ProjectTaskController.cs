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


    [HttpGet("calendar")]
    public async Task<IActionResult> GetTaskCalendar([FromQuery] int year, [FromQuery] int month)
    {
        if (year <= 0 || month < 1 || month > 12)
            return BadRequest("Invalid year or month.");

        var result = await _projectTaskService.GetTaskCalendarAsync(year, month);

        return Ok(result);
    }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectDto dto)
    // {
    //     await _projectTaskService.UpdateProjectTaskAsync(id, dto);
    //     return NoContent();
    // }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _projectTaskService.DeleteProjectTaskAsync(id);
        return NoContent();
    }

}
