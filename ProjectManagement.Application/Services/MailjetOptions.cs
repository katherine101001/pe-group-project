namespace ProjectManagement.Application.Services
{
    public class MailjetOptions
    {
        public string ApiKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
        public string FromName { get; set; } = null!;
    }
}
