using System;
using System.Net;
using System.Net.Mail;

namespace Example
{
    internal class Program
    {
        private static void Main()
        {
            Execute().Wait();
        }

        static async Task Execute()
        {
            try
            {
                var fromEmail = "ituckycoon@gmail.com";
                var toEmail = "ahmedkedic7@gmail.com";
                var subject = "Sending Email with SMTP";
                var body = "This is a test email sent using SMTP in C#.";

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ituckycoon@gmail.com", "mysecretpassword"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
                await smtpClient.SendMailAsync(mailMessage);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
