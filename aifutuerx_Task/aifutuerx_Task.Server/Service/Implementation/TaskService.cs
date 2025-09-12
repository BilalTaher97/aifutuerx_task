using aifutuerx_Task.Server.DTOs;
using aifutuerx_Task.Server.Models;
using aifutuerx_Task.Server.Repository.Interface;
using aifutuerx_Task.Server.Service.Interface;

namespace aifutuerx_Task.Server.Service.Implementation
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasks(int userId)
        {
            var tasks = await _repository.GetAllTasksByUserAsync(userId);
            return tasks.Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                UserId = t.UserId,
                Title = t.Title,
                Description = t.Description,
                StatusId = t.StatusId,
                StatusName = t.Status.StatusName,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });
        }

        public async Task<TaskDto?> GetTaskById(int id)
        {
            var task = await _repository.GetTaskByIdAsync(id);
            if (task == null) return null;

            return new TaskDto
            {
                TaskId = task.TaskId,
                UserId = task.UserId,
                Title = task.Title,
                Description = task.Description,
                StatusId = task.StatusId,
                StatusName = task.Status.StatusName,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }

        public async Task<TaskDto> CreateTask(CreateTaskDto dto)
        {
            var task = new UserTask
            {
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                StatusId = dto.StatusId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddTaskAsync(task);



            return new TaskDto
            {
                TaskId = created.TaskId,
                UserId = created.UserId,
                Title = created.Title,
                Description = created.Description,
                StatusId = created.StatusId,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };
        }

        public async Task<TaskDto> UpdateTask(int id, CreateTaskDto dto)
        {
            var existing = await _repository.GetTaskByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.StatusId = dto.StatusId;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateTaskAsync(existing);

            return new TaskDto
            {
                TaskId = updated.TaskId,
                UserId = updated.UserId,
                Title = updated.Title,
                Description = updated.Description,
                StatusId = updated.StatusId,
                CreatedAt = updated.CreatedAt,
                UpdatedAt = updated.UpdatedAt
            };
        }

        public async Task<bool> DeleteTask(int id)
        {
            return await _repository.DeleteTaskAsync(id);
        }



        public async Task<IEnumerable<TaskDto>> GetTasksByFilter(int userId, string filter)
        {
            var tasks = await _repository.GetTasksByFilterAsync(userId, filter);
            return tasks.Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                UserId = t.UserId,
                Title = t.Title,
                Description = t.Description,
                StatusId = t.StatusId,
                StatusName = t.Status.StatusName,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            });
        }

    }
}
