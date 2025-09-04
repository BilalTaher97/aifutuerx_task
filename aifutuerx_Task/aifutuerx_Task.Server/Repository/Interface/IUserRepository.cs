
using aifutuerx_Task.Server.DTOs;
using aifutuerx_Task.Server.Models;
using aifutuerx_Task.DTOs;
using aifutuerx_Task.Server.DTOs;


namespace KafanaTask.Repository.Interface
{
    public interface IUserRepository
    {

        Task<User> GetUserById(int Id);
        Task<User?> UpdateStatusAsync(int id, string statusEn, string statusAr);

        Task<bool> Add(User customer);

        Task<User> GetCustomerByEmail(string email);






    }
}
