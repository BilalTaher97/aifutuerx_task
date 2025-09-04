using aifutuerx_Task.DTOs;
using aifutuerx_Task.Server.DTOs;
using aifutuerx_Task.Server.Models;

namespace aifutuerx_Task.Service.Interface
{
    public interface IUserService
    {

        Task<bool> AddUser(User customer);

        Task<string> LoginAsync(string email, string password);


        Task<User?> GetUserByIdAsync(int id);
        Task<User?> UpdateStatusAsync(int id, string statusEn, string statusAr);

    }
}
