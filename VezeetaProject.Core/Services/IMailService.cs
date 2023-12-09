namespace VezeetaProject.Core.Services
{
    public interface IMailService
    {
        Task MailSender(string MailTo, string MailSubject, string Body);
    }
}
