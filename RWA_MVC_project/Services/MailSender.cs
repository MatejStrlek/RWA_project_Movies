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
        public void SendMail(string sender, string receiver, string subject, string message)
        {
            MailMessage messageToSend = new();

            SmtpClient client = new("localhost");

            MailAddress from = new(sender);
            messageToSend.From = from;

            MailAddress to = new(receiver);
            messageToSend.To.Add(to);

            messageToSend.Subject = subject;
            messageToSend.Body = message;

            client.Send(messageToSend);
        }
    }
}
