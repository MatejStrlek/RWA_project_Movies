using RWA_MVC_project.Models;
using System.Net.Mail;

namespace RWA_MVC_project.Services
{
    public interface IMailSender
    {
        void SendMail(string sender, string receiver, string subject, string message);
    }

    public class MailSender: IMailSender
    {
        private readonly RwaMoviesContext _context;

        public MailSender(RwaMoviesContext context)
        {
             _context = context;
        }

        public void SendMail(string sender, string receiver, string subject, string message)
        {
            MailMessage messageToSend = new();

            SmtpClient client = new("localhost");

            MailAddress from = new(sender);
            messageToSend.From = from;

            MailAddress to = new(receiver);
            messageToSend.To.Add(to);

            client.Send(messageToSend);

            _context.Notifications.Add(new Notification { 
                CreatedAt = DateTime.Now, 
                ReceiverEmail = receiver,
                Subject = subject,
                Body = message,
                SentAt = DateTime.Now
            });
            _context.SaveChanges();
        }
    }
}
