using aifutuerx_Task.Server.Models;

namespace aifutuerx_Task.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserById(int id);
        Task<bool> Add(User user);
        Task<User?> GetCustomerByEmail(string email);
        Task<bool> UpdateAsync(User user);
    }
}
