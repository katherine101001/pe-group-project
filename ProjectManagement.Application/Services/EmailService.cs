using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Options;
using ProjectManagement.Application.Interfaces.Services;
using ProjectManagement.Application.Services;


namespace ProjectManagement.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailjetOptions _options;

        public EmailService(IOptions<MailjetOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendMentionEmailAsync(string toEmail, string content, string fromName)
        {
            var client = new MailjetClient(_options.ApiKey, _options.SecretKey);

            var email = new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_options.FromEmail, _options.FromName))
                .WithSubject("You were mentioned in a comment")
                .WithHtmlPart($"<p>{content}</p>")
                .WithTo(new SendContact(toEmail))
                .Build();

            var response = await client.SendTransactionalEmailAsync(email);

            if (!response.Messages[0].Status.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Failed to send email to {toEmail}: {response.Messages[0].Status}");
            }
        }
    }
}
