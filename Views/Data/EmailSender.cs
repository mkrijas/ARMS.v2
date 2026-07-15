using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Views.Data
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions Options;
        public EmailSender(IOptions<EmailSenderOptions> options)
        {
            this.Options = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(email, subject, message);
        }

        public Task Execute(string to, string subject, string message)
        {
            // create message
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(Options.Sender_EMail);
            if (!string.IsNullOrEmpty(Options.Sender_Name))
                email.Sender.Name = Options.Sender_Name;
            email.From.Add(email.Sender);
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            // send email
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    
                    var secureSocketOptions = MailKit.Security.SecureSocketOptions.Auto;
                    if (Options.Host_Port == 25)
                    {
                        secureSocketOptions = MailKit.Security.SecureSocketOptions.None;
                    }
                    
                    smtp.Connect(Options.Host_Address, Options.Host_Port, secureSocketOptions);
                    
                    if (!string.IsNullOrEmpty(Options.Host_Username) && !string.IsNullOrEmpty(Options.Host_Password))
                    {
                        smtp.Authenticate(Options.Host_Username, Options.Host_Password);
                    }
                    
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SMTP Connection/Send failed: {ex}");
                Console.WriteLine("==================================================");
                Console.WriteLine("EMAIL SEND FAILURE (logged to console for development):");
                Console.WriteLine($"To: {to}");
                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine($"Message: {message}");
                Console.WriteLine($"Error Details: {ex.Message}");
                Console.WriteLine("==================================================");
            }
            return Task.FromResult(true);
        }
    }
}