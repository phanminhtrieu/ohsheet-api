using CleanArchitecture.Core.Interfaces.MailServices;
using CleanArchitecture.Shared;
using System.Net.Mail;
using System.Net;

namespace CleanArchitecture.Core.Services.MailServices
{
    public class EmailService : IEmailService
    {
        private readonly MailConfigurations _mailSettings ;

        public EmailService(AppSettings appSettings)
        {
            _mailSettings = appSettings.MailConfigurations;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = _mailSettings.From;
            string fromPassword = _mailSettings.Password;

            MailMessage message = new()
            {
                From = new MailAddress(fromMail),
                Subject = subject
            };

            message.To.Add(new MailAddress(email));
            message.Body = "<html><body>" + htmlMessage + "</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient(_mailSettings.Host)
            {
                Port = _mailSettings.Port,
                Credentials = new NetworkCredential(_mailSettings.UserName, fromPassword),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
