using System.Text.Json;
using System.Globalization;
using System.Net.Http.Headers;
using StockQuoteAlertProject.services;

namespace StockQuoteAlertProject{
    public class Program{
        public static async Task Main(string[] args){
            // Verifica se os argumentos necessários foram passados
            if (args.Length != 3){
                Console.WriteLine("Necessita-se 3 argumentos: ativo, preço de venda e preço de compra.");
                Console.WriteLine("Uso: dotnet run -- <ATIVO> <PRECO_VENDA> <PRECO_COMPRA>");
                return;
            }

            // Obtém o ativo, preço de venda e compra das cotações
            string ativo = args[0];

            // Tenta converter os preços de venda e compra para decimal, usando cultura invariante
            if (!decimal.TryParse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precoVenda) ||
                !decimal.TryParse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precoCompra)){
                Console.WriteLine("Preços inválidos.");
                return;
            }

            try{
                // Carrega configuração do arquivo config.json
                var config = ConfigService.LoadConfig("config.json");

                // Instancia o serviço de envio de e-mails com os parâmetros do SMTP
                var emailService = new EmailService(
                    config.SMTP.Host,
                    config.SMTP.Port,
                    config.SMTP.User,
                    config.SMTP.Password,
                    config.SMTP.Sender
                );

                Console.WriteLine("\n⏳ Monitorando. Pressione Ctrl + C para encerrar.");
                Console.WriteLine($"\nMonitorando {ativo}... Venda: R$ {precoVenda}, Compra: R$ {precoCompra}");

                // Loop infinito para monitorar o preço periodicamente
                while (true)
                {
                    try
                    {
                        // Obtém o preço atual do ativo via API
                        decimal precoAtual = await ObterPrecoAtual(ativo, config.Brapi.Token);
                        Console.WriteLine($"\n{DateTime.Now}: {ativo} = R$ {precoAtual}");

                        // Verifica se o preço ultrapassou o preço de venda configurado
                        if (precoAtual > precoVenda)
                        {
                            // Envia email recomendando venda
                            await emailService.EnviarEmailAsync(
                                config.Email.Recipients,
                                $"Venda recomendada: {ativo}",
                                $"O preço está em R$ {precoAtual}. Recomendado vender."
                            );
                        }
                        // Verifica se o preço está abaixo do preço de compra configurado
                        else if (precoAtual < precoCompra)
                        {
                            // Envia email recomendando compra
                            await emailService.EnviarEmailAsync(
                                config.Email.Recipients,
                                $"Compra recomendada: {ativo}",
                                $"O preço está em R$ {precoAtual}. Recomendado comprar."
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        // Caso ocorra algum erro durante a consulta ou envio, exibe mensagem
                        Console.WriteLine($"Erro durante monitoramento: {ex.Message}");
                    }

                    // Aguarda o intervalo configurado antes de nova consulta
                    await Task.Delay(TimeSpan.FromSeconds(config.MonitoringIntervalSeconds));
                }
            }
            // Caso ocorra erro ao carregar as configurações, exibe mensagem
            catch (Exception ex){
                Console.WriteLine($"Erro ao carregar configuração: {ex.Message}");
            }
        }

        // Método para obter o preço atual do ativo via API da Brapi
        static async Task<decimal> ObterPrecoAtual(string ativo, string token){
            using var client = new HttpClient{
                Timeout = TimeSpan.FromSeconds(10)
            };

            // Define o token Bearer para autenticação
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string url = $"https://brapi.dev/api/quote/{ativo}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = JsonDocument.Parse(stream);

            var results = doc.RootElement.GetProperty("results");

            // Verifica se retornou algum dado para o ativo
            if (results.GetArrayLength() == 0)
                throw new Exception($"Ativo '{ativo}' não encontrado ou sem dados.");

            var root = results[0];

            // Retorna o preço do mercado regular
            return root.GetProperty("regularMarketPrice").GetDecimal();
        }
    }
}
