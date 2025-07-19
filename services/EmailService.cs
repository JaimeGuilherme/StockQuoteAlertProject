using System.Net;
using System.Net.Mail;

namespace StockQuoteAlertProject.services
{
    public class EmailService{
        // Propriedades privadas para armazenar os dados do servidor SMTP e remetente
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _password;
        private readonly string _sender;

        // Construtor para inicializar os dados do servidor SMTP e remetente
        public EmailService(string host, int port, string user, string password, string sender){
            _host = host;
            _port = port;
            _user = user;
            _password = password;
            _sender = sender;
        }

        // Método assíncrono para enviar email para uma lista de destinatários
        public async Task EnviarEmailAsync(List<string> destinatarios, string assunto, string corpo){
            // Configura o cliente SMTP com host, porta, credenciais e SSL habilitado
            using var client = new SmtpClient(_host, _port){
                Credentials = new NetworkCredential(_user, _password),
                EnableSsl = true
            };

            // Cria a mensagem de email
            var mailMessage = new MailMessage{
                From = new MailAddress(_sender),
                Subject = assunto,
                Body = corpo,
                IsBodyHtml = false, // Corpo em texto simples (não HTML)
            };

            // Adiciona todos os destinatários no campo "To"
            foreach (var destinatario in destinatarios){
                mailMessage.To.Add(destinatario);
            }

            // Envia o email de forma assíncrona
            await client.SendMailAsync(mailMessage);

            Console.WriteLine("📧 Alerta enviado por e-mail.");
        }
    }
}