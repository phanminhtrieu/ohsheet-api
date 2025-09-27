namespace CleanArchitecture.Core.Interfaces.MailServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
