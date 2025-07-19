using System;
using System.Collections.Generic;

namespace StockQuoteAlertProject
{
    public class AppConfig
    {
        public int MonitoringIntervalSeconds { get; set; } = 60;

        public EmailConfig Email { get; set; } = new EmailConfig();

        public SmtpConfig SMTP { get; set; } = new SmtpConfig();

        public BrapiConfig Brapi { get; set; } = new BrapiConfig();

        public void Validate()
        {
            if (MonitoringIntervalSeconds <= 0)
            {
                Console.WriteLine("⚠️ Intervalo inválido no config.json. Usando 60s como padrão.");
                MonitoringIntervalSeconds = 60;
            }

            if (Email == null)
                throw new Exception("Configuração de Email não pode ser nula.");
            Email.Validate();

            if (SMTP == null)
                throw new Exception("Configuração de SMTP não pode ser nula.");
            SMTP.Validate();

            if (Brapi == null || string.IsNullOrWhiteSpace(Brapi.Token))
                throw new Exception("Token da API Brapi não configurado.");
            Brapi.Validate();
        }
    }

    public class EmailConfig
    {
        public List<string> Recipients { get; set; } = new List<string>();

        public void Validate()
        {
            if (Recipients == null || Recipients.Count == 0)
                throw new Exception("Nenhum destinatário de email configurado no arquivo config.json.");
        }
    }

    public class SmtpConfig
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;

        public void Validate()
        {
            if (!(Port > 0))
                throw new Exception("Porta inválida.");

            if (string.IsNullOrWhiteSpace(User))
                throw new Exception("Usuário não configurado.");

            if (string.IsNullOrWhiteSpace(Password))
                throw new Exception("Senha não configurada.");

            if (string.IsNullOrWhiteSpace(Sender))
                throw new Exception("Email remetente não configurado.");
        }
    }

    public class BrapiConfig
    {
        public string Token { get; set; } = string.Empty;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Token))
                throw new Exception("Token da API Brapi não configurado.");
        }
    }
}
