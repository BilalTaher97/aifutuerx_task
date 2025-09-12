using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography;
using aifutuerx_Task.DTOs;
using aifutuerx_Task.Server.DTOs;
using aifutuerx_Task.Server.Models;
using aifutuerx_Task.Service.Interface;
using aifutuerx_Task.Server.Service.Interface;

namespace aifutuerx_Task.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _customerService;
        private readonly IAuthService _authService;

        public UserController(IUserService customerService, IAuthService authService)
        {
            _customerService = customerService;
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> AddNewUser([FromBody] RegisterCustomerDto dto)
        {
            try
            {
                var customer = new User
                {
                    Name = dto.Name ?? "",
                    Email = dto.Email ?? "",
                    Phone = dto.Phone ?? "",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password ?? ""),
                    Gender = dto.Gender,
                    DateOfBirth = dto.DateOfBirth.HasValue ? DateOnly.FromDateTime(dto.DateOfBirth.Value) : null
                };

                bool result = await _customerService.AddUser(customer);

                if (result)
                    return Ok(new { message = "User Added Successfully" });
                else
                    return BadRequest(new { message = "Failed to Add User (Service returned false)" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "EXCEPTION", error = ex.Message, stack = ex.StackTrace });
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
        
       
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email))
                return BadRequest(new { message = "Email is required" });

            var result = await _authService.SendResetPasswordEmailAsync(dto.Email);

            if (result)
                return Ok(new { message = "Reset password link sent to email" });
            else
                return NotFound(new { message = "User with this email not found" });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Otp) || string.IsNullOrEmpty(dto.NewPassword))
                return BadRequest(new { message = "Email, OTP and NewPassword are required" });

            var result = await _authService.ResetPasswordAsync(dto.Email, dto.Otp, dto.NewPassword);
           
            if (result)
                return Ok(new { message = "Password reset successfully" });
            else
                return BadRequest(new { message = "Invalid OTP or OTP expired" });
        }


       
    }
}
