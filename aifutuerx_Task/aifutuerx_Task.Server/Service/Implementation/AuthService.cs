using aifutuerx_Task.Repository.Interface;
using aifutuerx_Task.Server.Service.Interface;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;              
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace aifutuerx_Task.Server.Service.Implementation
{
    public class AuthService : IAuthService
    {

        private static string MyOTP = "";

        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;

        private readonly ConcurrentDictionary<string, (string otp, DateTime expiry)> _otpCache
            = new ConcurrentDictionary<string, (string otp, DateTime expiry)>();

        public AuthService(IUserRepository userRepo, IEmailService emailService)
        {
            _userRepo = userRepo;
            _emailService = emailService;
        }

        public async Task<bool> SendResetPasswordEmailAsync(string email)
        {
            var user = await _userRepo.GetCustomerByEmail(email);
            if (user == null) return false;

            var otp = new Random().Next(100000, 999999).ToString();

            _otpCache[email] = (otp, DateTime.UtcNow.AddMinutes(15));

            MyOTP = otp;

            Console.WriteLine($"Stored Email: {email}, OTP: {otp}");

            await _emailService.SendAsync(user.Email, "Reset Password OTP", $"Your OTP code is: {otp}");
            return true;
        }


        public Task<bool> VerifyResetTokenAsync(string email, string otp)
        {
            if (otp == MyOTP)
            {
               
                    return Task.FromResult(true);
            }

            Console.WriteLine("Email not found in OTP cache");
            return Task.FromResult(false);
        }

        public async Task<bool> ResetPasswordAsync(string email, string otp, string newPassword)
        {
            if (!await VerifyResetTokenAsync(email, otp)) return false;

            var user = await _userRepo.GetCustomerByEmail(email);
            if (user == null) return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepo.UpdateAsync(user);

            _otpCache.TryRemove(email, out _);

            return true;
        }


        
    }
}
