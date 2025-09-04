using aifutuerx_Task.DTOs;
using aifutuerx_Task.Server.DTOs;
using KafanaTask.Repository.Interface;
using aifutuerx_Task.Server.Models;
using Microsoft.EntityFrameworkCore;



namespace aifutuerx_Task.Repository.Implemetnation
{
    public class UserRepository : IUserRepository
    {
        private readonly DbaifutuerxTaskContext _context;

        public UserRepository(DbaifutuerxTaskContext context)
        {
            _context = context;
        }



       

        public async Task<User?> GetUserById(int Id)
        {
            var currentUser = await _context.Users.FindAsync(Id);

            return currentUser;
        }

       

       




        public async Task<User?> UpdateStatusAsync(int id, string statusEn, string statusAr)
        {
            var customer = await _context.Users.FindAsync(id);
            if (customer == null)
                return null;

            

            await _context.SaveChangesAsync();

            return customer;
        }


        public async Task<bool> Add(User customer)
        {
            _context.Users.Add(customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetCustomerByEmail(string email)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);


        }

        


    }
}
