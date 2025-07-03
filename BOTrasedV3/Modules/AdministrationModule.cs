using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace BOTrasedV3.Modules
{
    [Group("admin", "Commands for administration actions in the server.")]
    public class AdministrationModule : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("ban", "Bans a user from the server. If no length is provided, the ban is permanent.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [CommandContextType(InteractionContextType.Guild)]
        public async Task BanUser(
            [Summary("user", "The user to ban")] SocketGuildUser user,
            [Summary("reason", "The reason for the ban")] string reason,
            [Summary("deletedays", "Number of days of messages to delete (0-7).")] int pruneDays = 0
        )
        {
            if (Context.Guild == null)
            {
                await RespondAsync("❌ This command can only be used in a server.", ephemeral: true);
                return;
            }

            // Ensure the bot has permissions to ban
            var botGuildUser = Context.Guild.CurrentUser; // The bot itself as a guild user
            if (!botGuildUser.GuildPermissions.BanMembers)
            {
                await RespondAsync("❌ I don't have permission to ban members in this server. Please check my role permissions.", ephemeral: true);
                return;
            }

            // Ensure the target user is actually in the guild (SocketGuildUser guarantees this)
            if (user == null)
            {
                await RespondAsync("❌ Could not find the specified user in this server.", ephemeral: true);
                return;
            }

            // Ensure you are not trying to ban yourself or the bot itself
            if (user.Id == Context.User.Id)
            {
                await RespondAsync("❌You cannot ban yourself.", ephemeral: true);
                return;
            }
            if (user.Id == Context.Client.CurrentUser.Id)
            {
                await RespondAsync("❌ I cannot ban myself. **Tip:** If you want me to leave, please kick me.", ephemeral: true);
                return;
            }

            // Ensure the bot can ban the target user (hierarchy check)
            // Bots cannot ban users with higher or equal roles than themselves, or the server owner.
            if (user.Hierarchy >= botGuildUser.Hierarchy)
            {
                await RespondAsync($"❌ I cannot ban {user.Mention} because their role is higher than or equal to mine, or they are the server owner.", ephemeral: true);
                return;
            }

            pruneDays = Math.Clamp(pruneDays, 0, 7);

            try
            {
                await Context.Guild.AddBanAsync(user, pruneDays, reason);

                await RespondAsync($"Successfully banned {user.Mention} for: `{reason}`. " +
                                   $"Messages from the last {pruneDays} day(s) were deleted.", ephemeral: false);
            }
            catch (Discord.Net.HttpException ex)
            {
                await RespondAsync($"Failed to ban {user.Mention}: {ex.Message}", ephemeral: true);
            }
            catch (Exception ex)
            {
                await RespondAsync($"An unexpected error occurred while trying to ban {user.Mention}.", ephemeral: true);
                Console.WriteLine($"Error banning user: {ex}");
            }
        }
    }
}
