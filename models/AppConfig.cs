namespace StockQuoteAlertProject
{
    // Classe principal de configuração da aplicação
    public class AppConfig{
        // Intervalo em segundos para o monitoramento (padrão 60s)
        public int MonitoringIntervalSeconds { get; set; } = 60;

        // Configuração dos emails (destinatários)
        public EmailConfig Email { get; set; } = new EmailConfig();

        // Configuração do SMTP para envio de email
        public SmtpConfig SMTP { get; set; } = new SmtpConfig();

        // Configuração da API Brapi (token de autenticação)
        public BrapiConfig Brapi { get; set; } = new BrapiConfig();

        // Método para validar as configurações carregadas
        public void Validate(){
            if (MonitoringIntervalSeconds <= 0){
                Console.WriteLine("⚠️ Intervalo inválido no config.json. Usando 60s como padrão.");
                MonitoringIntervalSeconds = 60;
            }

            if (Email == null)
                throw new Exception("Configuração de Email não pode ser nula.");
            Email.Validate();

            if (SMTP == null)
                throw new Exception("Configuração de SMTP não pode ser nula.");
            SMTP.Validate();

            if (Brapi == null)
                throw new Exception("Configuração da API Brapi não pode ser nula.");
            Brapi.Validate();
        }
    }

    // Configurações específicas de Email
    public class EmailConfig{
        // Lista de destinatários que receberão os alertas por email
        public List<string> Recipients { get; set; } = new List<string>();

        // Validação para garantir que há destinatários configurados
        public void Validate(){
            if (Recipients == null || Recipients.Count == 0)
                throw new Exception("Nenhum destinatário de email configurado no arquivo config.json.");
        }
    }

    // Configurações do servidor SMTP para envio de email
    public class SmtpConfig{
        public string Host { get; set; } = string.Empty;     // Provedor do SMTP
        public int Port { get; set; } = 587;                 // Porta padrão TLS
        public string User { get; set; } = string.Empty;     // Usuário do SMTP
        public string Password { get; set; } = string.Empty; // Senha do SMTP
        public string Sender { get; set; } = string.Empty;   // Email remetente

        // Validação dos campos SMTP obrigatórios
        public void Validate(){
            if (string.IsNullOrWhiteSpace(Host))
                throw new Exception("Provedor de Email não configurado no arquivo de configuração.");

            if (!(Port > 0))
                throw new Exception("Porta no arquivo de configuração inválida.");

            if (string.IsNullOrWhiteSpace(User))
                throw new Exception("Usuário não configurado no arquivo de configuração.");

            if (string.IsNullOrWhiteSpace(Password))
                throw new Exception("Senha não configurada no arquivo de configuração.");

            if (string.IsNullOrWhiteSpace(Sender))
                throw new Exception("Email remetente não configurado no arquivo de configuração.");
        }
    }

    // Configuração específica da API Brapi
    public class BrapiConfig{
        // Token para autenticação na API
        public string Token { get; set; } = string.Empty;

        // Validação para garantir que o token foi informado
        public void Validate(){
            if (string.IsNullOrWhiteSpace(Token))
                throw new Exception("Token da API Brapi não configurado no arquivo de configuração.");
        }
    }
}
