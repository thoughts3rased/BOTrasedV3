using BOTrasedV3.DAO;
using BOTrasedV3.Interfaces;
using BOTrasedV3.Models;
using BOTrasedV3.Services;
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
                    services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(), new InteractionServiceConfig()
                    {
                        LogLevel = LogSeverity.Verbose,
                        DefaultRunMode = RunMode.Async
                    }));

                    services.AddHostedService<DiscordBotWorker>();

                    services.AddSingleton<IDatabaseService, DatabaseService>();
                    services.AddSingleton<ICommandStatisticsService, CommandStatisticsService>();
                    services.AddSingleton<IFunCommandDataService, FunCommandDataService>();
                    services.AddSingleton<IBotStatusService, BotStatusService>();

                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Debug);
                });
    }
}
