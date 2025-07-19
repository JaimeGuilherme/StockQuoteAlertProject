using System.Text.Json;

namespace StockQuoteAlertProject.services{
    public static class ConfigService{
        // Método para carregar o arquivo de configuração JSON e desserializar em AppConfig
        public static AppConfig LoadConfig(string path){
            // Verifica se o arquivo existe
            if (!File.Exists(path))
                throw new FileNotFoundException($"Arquivo de configuração '{path}' não encontrado.");

            // Lê todo o conteúdo do arquivo JSON
            var json = File.ReadAllText(path);

            // Opções para desserialização: ignorar case sensitive nos nomes das propriedades
            var options = new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            };

            // Desserializa o JSON para o objeto AppConfig
            var config = JsonSerializer.Deserialize<AppConfig>(json, options);

            if (config == null)
                throw new Exception("Erro ao desserializar o arquivo de configuração.");

            // Valida os dados carregados da configuração
            ValidateConfig(config);

            return config;
        }

        // Método que valida as configurações obrigatórias e exibe erros caso algo esteja faltando ou incorreto
        private static void ValidateConfig(AppConfig config){
            if (!(config.MonitoringIntervalSeconds > 0)){
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
