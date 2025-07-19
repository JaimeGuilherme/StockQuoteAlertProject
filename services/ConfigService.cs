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

            // Verifica se a desserialização retornou nulo
            if (config == null)
                throw new Exception($"Erro ao desserializar o arquivo de configuração. Verifique o arquivo '{path}'.");

            // Valida os dados carregados da configuração
            ValidateConfig(config);

            return config;
        }

        // Método que valida as configurações obrigatórias e exibe erros caso algo esteja faltando ou incorreto
        private static void ValidateConfig(AppConfig config){
            // Validação feita pela própria classe AppConfig
            config.Validate();
        }
    }
}
