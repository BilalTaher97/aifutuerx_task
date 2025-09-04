using aifutuerx_Task;
using aifutuerx_Task.Server.DTOs;
using aifutuerx_Task.Server.Models;
using KafanaTask.Repository.Interface;
using KafanaTask.Server.JwtSetting;
using KafanaTask.Service.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;



namespace aifutuerx_Task.Service.Implemetnation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _customerRepository;
        private readonly string _secretKey;
        public UserService(IUserRepository customerRepository, IOptions<JwtSettings> jwtSettings)
        {
            _customerRepository = customerRepository;
            _secretKey = jwtSettings.Value.SecretKey;
        }

        

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var customer = await _customerRepository.GetUserById(id);

            //if (customer == null || customer.StatusEn != "Active")
            //    return null;


            return customer;
        }




        public async Task<User?> UpdateStatusAsync(int id, string statusEn, string statusAr)
        {
            return await _customerRepository.UpdateStatusAsync(id, statusEn, statusAr);
        }


        public async Task<bool> AddUser(User user)
        {
            return await _customerRepository.Add(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var customer = await _customerRepository.GetCustomerByEmail(email);

            if (customer == null)
                return null;

            if (!VerifyPassword(password, customer.PasswordHash))
                return null;

            return GenerateJwtToken(customer);
        }



        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));

                return builder.ToString();
            }
        }


        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = ComputeSha256Hash(enteredPassword);
            return enteredHash == storedHash;
        }


        private string GenerateJwtToken(User customer)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            new Claim(ClaimTypes.Email, customer.Email),
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "yourAppName",
                Audience = "yourAppUsers",
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


       




    }
}
