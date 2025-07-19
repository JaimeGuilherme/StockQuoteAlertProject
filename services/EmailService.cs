using System.Net;
using System.Net.Mail;
using System.Net.Sockets;

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
        public async Task EnviarEmailAsync(List<string> recipients, string assunto, string corpo){
            try{
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
                foreach (var recipient in recipients){
                    mailMessage.To.Add(recipient);
                }

                // Envia o email de forma assíncrona e captura erros específicos de SMTP
                try{
                    await client.SendMailAsync(mailMessage);
                }
                catch (SmtpException ex) when (ex.InnerException is SocketException socketEx){
                    throw new Exception($"Erro ao conectar ao servidor SMTP '{_host}:{_port}': {socketEx.Message}", ex);
                }
                catch (SmtpException ex){
                    throw new Exception($"Falha no envio de email via SMTP: {ex.Message}", ex);
                }
                catch (Exception ex){
                    throw new Exception($"Erro inesperado ao enviar email: {ex.Message}", ex);
                }

                Console.WriteLine("Email enviado com sucesso!");
            }
            // Capturando outros erros que não sejam de SMTP
            catch (Exception ex){
                throw new Exception($"Falha ao enviar alerta por e-mail: {ex.Message}. Verifique as configurações de SMTP e os destinatários.", ex);
            }
        }
    }
}