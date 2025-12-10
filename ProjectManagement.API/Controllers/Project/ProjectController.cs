using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.Interfaces.Services;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // To create project
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
    {
        await _projectService.CreateProjectAsync(dto);
        return Ok();
    }

    // To get a specific project 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(Guid id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }


    // To get ALL Projects
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _projectService.DeleteProjectAsync(id);
        return NoContent();
    }


    // Fill in the forms in Project Setting page first before updating
    [HttpGet("{id:guid}/update/form")]
    public async Task<IActionResult> GetUpdateDetailsProject(Guid id)
    {
        var project = await _projectService.GetUpdateProjectByIdAsync(id);
        return Ok(project);
    }

    // Update the forms
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectDto dto)
    {
        await _projectService.UpdateProjectAsync(id, dto);
        return NoContent();
    }

}
