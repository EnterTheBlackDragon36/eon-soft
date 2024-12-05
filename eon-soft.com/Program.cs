using eon_soft.com;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using System.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var builder = WebApplication.CreateBuilder(args);
                // Add Key Vault configuration
                var keyVaultUrl = $"https://{builder.Configuration["AzureAd:KeyVaultName"]}.vault.azure.net/";
                builder.Configuration.AddAzureKeyVault(
                new Uri(keyVaultUrl),
                new DefaultAzureCredential());
            })
            .ConfigureWebHostDefaults(config =>
            {
                config.UseStartup<Startup>();
            });

}