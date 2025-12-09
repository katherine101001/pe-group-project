using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.Interfaces.Services;


[ApiController]
[Route("api/projects/{projectId}/analytics")]
public class ProjectAnalyticsController : ControllerBase
{
    private readonly IProjectTaskAnalyticsService _analyticsService;

    public ProjectAnalyticsController(IProjectTaskAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("overview")]
    public async Task<IActionResult> GetAnalytics(Guid projectId)
    {
        var result = await _analyticsService.GetProjectAnalyticsAsync(projectId);
        return Ok(result);
    }
}

