using aifutuerx_Task.Server.Models;
using aifutuerx_Task.Server.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace aifutuerx_Task.Server.Repository.Implementation
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DbaifutuerxTaskContext _context;
        public TaskRepository(DbaifutuerxTaskContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<UserTask>> GetAllTasksByUserAsync(int userId)
        {
            return await _context.UserTasks
                .Include(t => t.Status)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserTask?> GetTaskByIdAsync(int id)
        {
            return await _context.UserTasks
                .Include(t => t.Status)
                .FirstOrDefaultAsync(t => t.TaskId == id);
        }

        public async Task<UserTask> AddTaskAsync(UserTask task)
        {
            _context.UserTasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<UserTask> UpdateTaskAsync(UserTask task)
        {
            _context.UserTasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.UserTasks.FindAsync(id);
            if (task == null) return false;

            _context.UserTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<IEnumerable<UserTask>> GetTasksByFilterAsync(int userId, string filter)
        {
            var query = _context.UserTasks
                .Include(t => t.Status)
                .Where(t => t.UserId == userId);

            switch (filter.ToLower())
            {
                case "completed":
                    query = query.Where(t => t.StatusId == 3);
                    break;
                case "inprogress":
                    query = query.Where(t => t.StatusId == 2);
                    break;
                case "pending":
                    query = query.Where(t => t.StatusId == 1);
                    break;
                case "all":
                default:
                    break;
            }

            return await query.ToListAsync();
        }


    }

}
