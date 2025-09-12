using aifutuerx_Task.Server.DTOs;
using aifutuerx_Task.Server.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aifutuerx_Task.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllTickets(int userId)
        {
            var tasks = await _service.GetAllTasks(userId);
            return Ok(tasks);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var task = await _service.GetTaskById(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto)
        {
            var created = await _service.CreateTask(dto);
            return CreatedAtAction(nameof(GetTaskById), new { id = created.TaskId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, CreateTaskDto dto)
        {
            var updated = await _service.UpdateTask(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var deleted = await _service.DeleteTask(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }


        [HttpGet("{userId}/filter/{filter}")]
        public async Task<IActionResult> GetTasksByFilter(int userId, string filter)
        {
            var tasks = await _service.GetTasksByFilter(userId, filter);
            return Ok(tasks);
        }

    }
}