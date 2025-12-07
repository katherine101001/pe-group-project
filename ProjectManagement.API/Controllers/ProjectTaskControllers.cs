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

        // Create a new task
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _taskService.CreateProjectTaskAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        // Get a task by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var task = await _taskService.GetProjectTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        // Get all tasks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllProjectTasksAsync();
            return Ok(tasks);
        }

        // Update a task
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _taskService.UpdateProjectTaskAsync(id, dto);
                return NoContent();
            }
            catch (ProjectManagement.Shared.Exceptions.NotFoundException)
            {
                return NotFound();
            }
        }

        // Delete a task
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _taskService.DeleteProjectTaskAsync(id);
                return NoContent();
            }
            catch (ProjectManagement.Shared.Exceptions.NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
