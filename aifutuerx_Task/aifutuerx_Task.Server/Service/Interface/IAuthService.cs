namespace aifutuerx_Task.Server.Service.Interface
{
    public interface IAuthService
    {
        Task<bool> SendResetPasswordEmailAsync(string email);
        Task<bool> VerifyResetTokenAsync(string email, string otp);
        Task<bool> ResetPasswordAsync(string email, string otp, string newPassword);
    }
}
