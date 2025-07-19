using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StockQuoteAlertProject.services;
using System.Globalization;

namespace StockQuoteAlertProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Variáveis: <ATIVO> <PRECO_VENDA> <PRECO_COMPRA>");
                return;
            }

            string ativo = args[0];
            if (!decimal.TryParse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precoVenda) ||
                !decimal.TryParse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precoCompra))
            {
                Console.WriteLine("Preços inválidos.");
                return;
            }

            if (precoVenda <= 0 || precoCompra <= 0)
            {
                Console.WriteLine("Os preços devem ser maiores que zero.");
                return;
            }

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            string emailDestino = GetRequiredConfig(config, "Email:Destino");
            var smtpConfig = config.GetSection("SMTP");
            string tokenBrapi = GetRequiredConfig(config, "Brapi:Token");

            var emailService = new EmailService(
                GetRequiredConfig(smtpConfig, "Host"),
                int.Parse(GetRequiredConfig(smtpConfig, "Port")),
                GetRequiredConfig(smtpConfig, "User"),
                GetRequiredConfig(smtpConfig, "Password"),
                GetRequiredConfig(smtpConfig, "Sender")
            );

            Console.WriteLine($"Monitorando {ativo}... Venda: R$ {precoVenda}, Compra: R$ {precoCompra}");

            while (true)
            {
                try
                {
                    decimal precoAtual = await ObterPrecoAtual(ativo, tokenBrapi);
                    Console.WriteLine($"{DateTime.Now}: {ativo} = R$ {precoAtual}");

                    if (precoAtual > precoVenda)
                    {
                        await emailService.EnviarEmailAsync(
                            emailDestino,
                            $"Venda recomendada: {ativo}",
                            $"O preço está em R$ {precoAtual}. Recomendado vender."
                        );
                    }
                    else if (precoAtual < precoCompra)
                    {
                        await emailService.EnviarEmailAsync(
                            emailDestino,
                            $"Compra recomendada: {ativo}",
                            $"O preço está em R$ {precoAtual}. Recomendado comprar."
                        );
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        static async Task<decimal> ObterPrecoAtual(string ativo, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            string url = $"https://brapi.dev/api/quote/{ativo}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement.GetProperty("results")[0];
            return root.GetProperty("regularMarketPrice").GetDecimal();
        }

        static string GetRequiredConfig(IConfiguration config, string key)
        {
            return config[key] ?? throw new ArgumentException($"Chave obrigatória '{key}' ausente no config.json");
        }
    }

    class BrapiResponse
    {
        public StockQuote[]? Results { get; set; }
    }

    class StockQuote
    {
        public string? Symbol { get; set; }
        public string? ShortName { get; set; }
        public decimal RegularMarketPrice { get; set; }
    }
}
