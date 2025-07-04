using Discord;
using Discord.Interactions;

namespace BOTrasedV3.Modules
{
    [Group("utilities", "General purpose utilities")]
    public class UtilitiesModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("id", "Displays your user ID, or the ID of another user.")]
        public async Task IdAsync([Summary("user", "The user whose ID you want to fetch")] IUser user = null)
        {
            user ??= Context.Interaction.User;

            bool targetIsInvoker = user.Id == Context.Interaction.User.Id;

            if (targetIsInvoker)
            {
                await RespondAsync($"Your user ID is {user.Id}", ephemeral: true);
            }
            else
            {
                await RespondAsync($"{user.Mention}'s user ID is {user.Id}", ephemeral: true);
            }
        }


    }
}
