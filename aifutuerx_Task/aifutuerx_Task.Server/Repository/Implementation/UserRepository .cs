using aifutuerx_Task.Repository.Interface;
using aifutuerx_Task.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace aifutuerx_Task.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly DbaifutuerxTaskContext _context;

        public UserRepository(DbaifutuerxTaskContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetCustomerByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> Add(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
