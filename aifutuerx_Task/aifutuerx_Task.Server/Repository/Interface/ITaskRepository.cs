using aifutuerx_Task.Server.Models;

namespace aifutuerx_Task.Server.Repository.Interface
{
    public interface ITaskRepository
    {
        Task<IEnumerable<UserTask>> GetAllTasksByUserAsync(int userId);
        Task<UserTask?> GetTaskByIdAsync(int id);
        Task<UserTask> AddTaskAsync(UserTask task);
        Task<UserTask> UpdateTaskAsync(UserTask task);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<UserTask>> GetTasksByFilterAsync(int userId, string filter);

    }
}
