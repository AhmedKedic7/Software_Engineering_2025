using Microsoft.Extensions.Configuration;
using ShoeStore.Services.Interfaces;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;

using System.Net;
 
 

namespace ShoeStore.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    
        public async Task SendEmailAsync(string to, string subject, string htmlContent)
        {
            try
            {
                var fromEmail = _configuration["EmailFromAddress"];
                var fromPassword = _configuration["EmailFromPassword"]; // Ensure to store credentials securely, not hardcoded

                if (string.IsNullOrEmpty(fromEmail) || string.IsNullOrEmpty(fromPassword))
                {
                    throw new InvalidOperationException("Sender email or password is not configured.");
                }

                // Setup SMTP client
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true
                };

                // Create the email message
                var mailMessage = new MailMessage(fromEmail, to, subject, htmlContent)
                {
                    IsBodyHtml = true
                };

                // Send the email asynchronously
                await smtpClient.SendMailAsync(mailMessage);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                // Optionally, log the exception to a logging service
                throw;
            }
        }
    }
}
