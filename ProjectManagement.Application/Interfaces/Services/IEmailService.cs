using ProjectManagement.Application.DTOs.Collaboration;

namespace ProjectManagement.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendMentionEmailAsync(string toEmail, string content, string fromName);
    }

}