using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/tasks/briefly")]
public class ProjectTaskDetailsController : ControllerBase
{
    private readonly IProjectTaskService _service;

    public ProjectTaskDetailsController(IProjectTaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _service.GetAllProjectTasksBrieflyAsync();
        return Ok(tasks);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _service.GetProjectTaskBrieflyByIdAsync(id);
        if (task == null) return NotFound();
        return Ok(task);
    }
}
