using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.Interfaces.Services;

namespace ProjectManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IProjectTaskService _taskService;

        public TasksController(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }

        // ===========================
        // 创建任务
        // POST: /api/tasks
        // ===========================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _taskService.CreateProjectTaskAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        // ===========================
        // 根据 ID 获取任务（供 CreatedAtAction 返回用）
        // GET: /api/tasks/{id}
        // ===========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskService.GetProjectTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }
    }
}
