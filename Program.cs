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
                Console.WriteLine("Uso: stock-quote-alert.exe <ATIVO> <PRECO_VENDA> <PRECO_COMPRA>");
                return;
            }

            string ativo = args[0];

            if (!decimal.TryParse(args[1], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precoVenda) ||
                !decimal.TryParse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture, out decimal precoCompra))
            {
                Console.WriteLine("Preços inválidos.");
                return;
            }

            try
            {
                var config = ConfigService.LoadConfig("config.json");

                var emailService = new EmailService(
                    config.SMTP.Host,
                    config.SMTP.Port,
                    config.SMTP.User,
                    config.SMTP.Password,
                    config.SMTP.Sender
                );

                Console.WriteLine("⏳ Monitorando. Pressione Ctrl + C para encerrar.");

                while (true)
                {
                    Console.WriteLine($"Monitorando {ativo}... Venda: R$ {precoVenda}, Compra: R$ {precoCompra}");
                    try
                    {
                        decimal precoAtual = await ObterPrecoAtual(ativo, config.Brapi.Token);
                        Console.WriteLine($"{DateTime.Now}: {ativo} = R$ {precoAtual}");

                        if (precoAtual > precoVenda)
                        {
                            await emailService.EnviarEmailAsync(
                                config.Email.Recipients,
                                $"Venda recomendada: {ativo}",
                                $"O preço está em R$ {precoAtual}. Recomendado vender."
                            );
                        }
                        else if (precoAtual < precoCompra)
                        {
                            await emailService.EnviarEmailAsync(
                                config.Email.Recipients,
                                $"Compra recomendada: {ativo}",
                                $"O preço está em R$ {precoAtual}. Recomendado comprar."
                            );
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro durante monitoramento: {ex.Message}");
                    }

                    await Task.Delay(TimeSpan.FromSeconds(config.MonitoringIntervalSeconds));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar configuração: {ex.Message}");
            }
        }

        static async Task<decimal> ObterPrecoAtual(string ativo, string token)
        {
            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            string url = $"https://brapi.dev/api/quote/{ativo}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = JsonDocument.Parse(stream);

            var results = doc.RootElement.GetProperty("results");

            if (results.GetArrayLength() == 0)
                throw new Exception($"Ativo '{ativo}' não encontrado ou sem dados.");

            var root = results[0];
            return root.GetProperty("regularMarketPrice").GetDecimal();
        }
    }
}