
using KafanaTask.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using aifutuerx_Task.DTOs;
using aifutuerx_Task.Server.DTOs;
using aifutuerx_Task.Server.Models;


namespace aifutuerx_Task.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _customerService;

        public UserController(IUserService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> AddNewUser([FromForm] RegisterCustomerDto dto)
        {
            try
            {
               

                var customer = new User
                {
                    Name = dto.Name ?? "",               
                    Email = dto.Email ?? "",
                    Phone = dto.Phone ?? "",                  
                    PasswordHash = ComputeSha256Hash(dto.Password ?? ""),
                    Gender = dto.Gender,
                    DateOfBirth = dto.DateOfBirth.HasValue ? DateOnly.FromDateTime(dto.DateOfBirth.Value) : null
                };

                bool result = await _customerService.AddUser(customer);

                if (result)
                {
                    return Ok(new { message = "User Added Successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to Add User (Service returned false)" });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "EXCEPTION", error = ex.Message, stack = ex.StackTrace });
            }
        }


        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));

                return builder.ToString();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO LoginData)
        {

            var token = await _customerService.LoginAsync(LoginData.Email, LoginData.Password);

            if (token == null)
                return Unauthorized("Invalid credentials");

            return Ok(new { Token = token });

        }


    }
}
