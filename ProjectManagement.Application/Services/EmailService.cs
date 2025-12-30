using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Options;
using ProjectManagement.Application.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailjetOptions _options;

        public EmailService(IOptions<MailjetOptions> options)
        {
            _options = options.Value;

            
            if (string.IsNullOrEmpty(_options.ApiKey) || string.IsNullOrEmpty(_options.SecretKey))
            {
                Console.WriteLine("Warning: Mailjet API Key or Secret is missing. Emails will not be sent.");
            }

            if (string.IsNullOrEmpty(_options.FromEmail) || string.IsNullOrEmpty(_options.FromName))
            {
                Console.WriteLine("Warning: Mailjet FromEmail or FromName is missing.");
            }
        }

        public async Task SendMentionEmailAsync(string toEmail, string content, string fromName = null)
        {
            
            if (string.IsNullOrEmpty(_options.ApiKey) || string.IsNullOrEmpty(_options.SecretKey))
            {
                Console.WriteLine($"Skipping email to {toEmail} because Mailjet configuration is missing.");
                return;
            }

            try
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
                else
                {
                    Console.WriteLine($"Email successfully sent to {toEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while sending email to {toEmail}: {ex.Message}");
            }
        }
    }
}
