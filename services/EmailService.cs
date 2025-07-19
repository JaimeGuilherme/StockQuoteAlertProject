using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace StockQuoteAlertProject.services
{
    public class EmailService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _password;
        private readonly string _sender;

        public EmailService(string host, int port, string user, string password, string sender)
        {
            _host = host;
            _port = port;
            _user = user;
            _password = password;
            _sender = sender;
        }

        public async Task EnviarEmailAsync(List<string> destinatarios, string assunto, string corpo)
        {
            using var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_user, _password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_sender),
                Subject = assunto,
                Body = corpo,
                IsBodyHtml = false,
            };

            foreach (var destinatario in destinatarios)
            {
                mailMessage.To.Add(destinatario);
            }

            await client.SendMailAsync(mailMessage);
            Console.WriteLine("📧 Alerta enviado por e-mail.");
        }
    }
}