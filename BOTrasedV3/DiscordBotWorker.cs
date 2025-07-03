using BOTrasedV3.Interfaces;
using BOTrasedV3.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BOTrasedV3
{
    public class DiscordBotWorker : IHostedService
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _interactions;
        private readonly ILogger<DiscordBotWorker> _logger;
        private readonly Configuration _discordSettings;
        private readonly IServiceProvider _serviceProvider; // Required for CommandService context
        private readonly IHostEnvironment _env;
        private readonly ICommandStatisticsService _commandStatisticsService;

        public DiscordBotWorker(
            DiscordSocketClient client,
            InteractionService interactions,
            ILogger<DiscordBotWorker> logger,
            IOptions<Configuration> discordSettings,
            IServiceProvider serviceProvider,
            IHostEnvironment env,
            ICommandStatisticsService commandStatisticsService) // Inject IServiceProvider for CommandService
        {
            _client = client;
            _interactions = interactions;
            _logger = logger;
            _discordSettings = discordSettings.Value; // Access the settings
            _serviceProvider = serviceProvider;

            _client.Log += LogAsync;
            _interactions.Log += LogAsync;
            _client.Ready += OnReadyAsync;
            _client.InteractionCreated += HandleInteraction;
            _commandStatisticsService = commandStatisticsService;
            _env = env;

            _interactions.InteractionExecuted += async (ICommandInfo commandInfo, IInteractionContext context, IResult result) =>
            {
                await _commandStatisticsService.LogCommandUsage(commandInfo.Name);
            };

        }

        private Task LogAsync(LogMessage log)
        {
            var severity = log.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Debug,
                LogSeverity.Debug => LogLevel.Trace,
                _ => LogLevel.Information
            };
            _logger.Log(severity, log.Exception, "[Discord.NET] {Message}", log.Message);
            return Task.CompletedTask;
        }

        private async Task OnReadyAsync()
        {
            _logger.LogInformation($"Bot is connected as {_client.CurrentUser.Username}#{_client.CurrentUser.Discriminator}");
            await _client.SetActivityAsync(new Game("with DI", ActivityType.Playing));

            // Register command modules
            await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

#if DEBUG
            if (_discordSettings.TestGuildId.HasValue)
            {
                // Register commands to the specified test guild
                await _interactions.RegisterCommandsToGuildAsync(_discordSettings.TestGuildId.Value, true);
                _logger.LogInformation("Guild slash commands registered for guild {GuildId} (Development mode).", _discordSettings.TestGuildId.Value);
            }
            else
            {
                _logger.LogWarning("Running in Development mode but 'Discord:TestGuildId' is not configured in appsettings.Development.json. No guild commands registered.");
            }

#endif
#if !DEBUG
            // Register commands globally
            await _interactions.RegisterCommandsGloballyAsync(true);
            _logger.LogInformation("Global slash commands registered (Production mode).");
#endif
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                // Create an execution context that provides access to the DiscordSocketClient and the Interaction
                var context = new SocketInteractionContext(_client, interaction);

                // Execute the interaction. The InteractionService will find the correct module/command.
                var result = await _interactions.ExecuteCommandAsync(context, _serviceProvider);

                // Handle the result
                if (!result.IsSuccess)
                {
                    _logger.LogError("Error executing interaction: {ErrorReason}", result.ErrorReason);

                    // You can respond to the user with the error
                    if (result.Error == InteractionCommandError.UnmetPrecondition)
                    {
                        await interaction.RespondAsync($"You don't have permission to use this command: {result.ErrorReason}", ephemeral: true);
                    }
                    else if (interaction.Type == InteractionType.ApplicationCommand)
                    {
                        // For slash commands, ensure you respond even if there's an error
                        if (interaction.HasResponded)
                        {
                            await interaction.FollowupAsync($"An error occurred: {result.ErrorReason}", ephemeral: true);
                        }
                        else
                        {
                            await interaction.RespondAsync($"An error occurred: {result.ErrorReason}", ephemeral: true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling interaction: {InteractionId}", interaction.Id);

                // If something goes wrong, respond to the user.
                if (interaction.Type == InteractionType.ApplicationCommand && !interaction.HasResponded)
                {
                    await interaction.RespondAsync("An unexpected error occurred while processing your command.", ephemeral: true);
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discord Bot Service is starting.");

            // Login and start the bot
            await _client.LoginAsync(TokenType.Bot, _discordSettings.BotToken);
            await _client.StartAsync();

            _logger.LogInformation("Discord Bot Service started.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Discord Bot Service is stopping.");
            await _client.StopAsync();
            await _client.LogoutAsync();
            _logger.LogInformation("Discord Bot Service stopped.");
        }
    }
}
