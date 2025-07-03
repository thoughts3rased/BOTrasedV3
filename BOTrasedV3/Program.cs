using BOTrasedV3.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BOTrasedV3
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<Configuration>(hostContext.Configuration.GetSection("AppConfig"));

                    services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                    {
                        GatewayIntents = GatewayIntents.AllUnprivileged,
                        LogLevel = LogSeverity.Verbose
                    }));

                    // NEW: Register InteractionService
                    services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(), new InteractionServiceConfig()
                    {
                        LogLevel = LogSeverity.Verbose,
                        DefaultRunMode = RunMode.Async // Essential for async operations
                    }));

                    // Register your Discord Bot as a Hosted Service
                    services.AddHostedService<DiscordBotWorker>();

                    // (Optional) Register other services your slash command handlers might need
                    // These will be automatically injected into your command modules
                    // services.AddSingleton<IDataService, DataService>();
                    // services.AddTransient<IMyUtility, MyUtility>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);
                });
    }
}
