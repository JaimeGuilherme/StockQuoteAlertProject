using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace StockQuoteAlertProject.services
{
    public class EmailService
    {
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string smtpUser;
        private readonly string smtpPassword;
        private readonly string senderEmail;

        public EmailService(string host, int port, string user, string password, string sender)
        {
            smtpHost = host;
            smtpPort = port;
            smtpUser = user;
            smtpPassword = password;
            senderEmail = sender;
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string corpo)
        {
            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true
            };

            using var mail = new MailMessage(senderEmail, destinatario, assunto, corpo);
            await client.SendMailAsync(mail);
        }
    }
}
