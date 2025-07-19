using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StockQuoteAlertProject.services
{
    public static class ConfigService
    {
        public static AppConfig LoadConfig(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"Arquivo de configuração '{path}' não encontrado.");

            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var config = JsonSerializer.Deserialize<AppConfig>(json, options);

            if (config == null)
                throw new Exception("Erro ao desserializar o arquivo de configuração.");

            ValidateConfig(config);
            return config;
        }

        private static void ValidateConfig(AppConfig config)
        {
            if (!(config.MonitoringIntervalSeconds > 0))
            {
                Console.WriteLine("⚠️ Intervalo inválido no config.json. Usando 60s como padrão.");
                config.MonitoringIntervalSeconds = 60;
            }

            if (config.Brapi == null || string.IsNullOrWhiteSpace(config.Brapi.Token))
                throw new Exception("Token da API Brapi não configurado no config.json.");

            if (config.Email == null || config.Email.Recipients == null || config.Email.Recipients.Count == 0)
                throw new Exception("Nenhum destinatário de email configurado na seção 'Email' do config.json.");

            if (config.SMTP == null)
                throw new Exception("Seção 'SMTP' ausente no config.json.");

            if (string.IsNullOrWhiteSpace(config.SMTP.Host))
                throw new Exception("SMTP Host não configurado.");

            if (config.SMTP.Port <= 0)
                throw new Exception("Porta SMTP inválida ou não configurada.");

            if (string.IsNullOrWhiteSpace(config.SMTP.User))
                throw new Exception("Usuário SMTP não configurado.");

            if (string.IsNullOrWhiteSpace(config.SMTP.Password))
                throw new Exception("Senha SMTP não configurada.");

            if (string.IsNullOrWhiteSpace(config.SMTP.Sender))
                throw new Exception("Remetente (Sender) do email não configurado.");
        }
    }
}
