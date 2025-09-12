using aifutuerx_Task.Server.DTOs;

namespace aifutuerx_Task.Server.Service.Interface
{
    public interface ITaskService
    {

        Task<IEnumerable<TaskDto>> GetAllTasks(int userId);
        Task<TaskDto?> GetTaskById(int id);
        Task<TaskDto> CreateTask(CreateTaskDto dto);
        Task<TaskDto> UpdateTask(int id, CreateTaskDto dto);
        Task<bool> DeleteTask(int id);

        Task<IEnumerable<TaskDto>> GetTasksByFilter(int userId, string filter);

    }
}
