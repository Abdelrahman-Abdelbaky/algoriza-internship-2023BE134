using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;


namespace VezeetaProject.Services
{
    public class MailService : IMailService
    {
        private readonly Mail _mail;
        public MailService(IOptions<Mail> mail)
        {
            _mail = mail.Value;
        }
        /// <summary>
        /// send Email to user with his Email and Password 
        /// </summary>
        /// <param name="MailTo"></param>
        /// <param name="MailSubject"></param>
        /// <param name="Body"></param>
        /// <returns></returns>
        public async Task MailSender(string MailTo, string MailSubject, string Body)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mail.Email),
                Subject = MailSubject
            };

            email.To.Add(MailboxAddress.Parse(MailTo));
            var builder = new BodyBuilder();
            builder.HtmlBody = Body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mail.DisplayName, _mail.Email));
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_mail.Host, _mail.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mail.Email, _mail.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }


        }
    }
}
