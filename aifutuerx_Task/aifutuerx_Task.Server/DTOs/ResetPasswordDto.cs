﻿namespace aifutuerx_Task.Server.DTOs
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
