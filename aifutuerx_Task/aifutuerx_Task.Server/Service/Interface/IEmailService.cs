﻿namespace aifutuerx_Task.Server.Service.Interface
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string body);

    }
}
