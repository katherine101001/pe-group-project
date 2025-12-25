using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IProjectTaskService _taskService;

        public SearchController(IProjectService projectService, IProjectTaskService taskService)
        {
            _projectService = projectService;
            _taskService = taskService; 
        }

        // Search projects by keyword
        [HttpGet("projects")]
            public async Task<IActionResult> SearchProjects([FromQuery] string keyword)
            {
                var projects = await _projectService.SearchProjectsAsync(keyword);

                if (projects == null || !projects.Any())
                    return Ok(new { message = "none", projects = new List<SearchProjectDto>() });

                return Ok(projects);
            }


        // Search tasks by keyword
        [HttpGet("tasks")]
            public async Task<IActionResult> SearchTasks([FromQuery] string keyword)
            {
                var tasks = await _taskService.SearchTasksAsync(keyword);

                if (tasks == null || !tasks.Any())
                    return Ok(new { message = "none", tasks = new List<SearchTaskDto>() });

                return Ok(tasks);
            }


        // Optional: Combined search endpoint (if you want both in one request)
        [HttpGet("all")]
        public async Task<IActionResult> SearchAll([FromQuery] string keyword)
        {
            // 如果服务返回 null，直接给空列表
            var projects = await _projectService.SearchProjectsAsync(keyword) ?? new List<SearchProjectDto>();
            var tasks = await _taskService.SearchTasksAsync(keyword) ?? new List<SearchTaskDto>();

            return Ok(new
            {
                projects,  // 前端用 response.data.projects
                tasks      // 前端用 response.data.tasks
            });
        }

    }
}
