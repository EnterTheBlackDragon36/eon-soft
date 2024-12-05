using eon_soft.com;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

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
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("AzureKeyVault")!);
config.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
})
            .ConfigureWebHostDefaults(config =>
            {
                config.UseStartup<Startup>();
            });

}