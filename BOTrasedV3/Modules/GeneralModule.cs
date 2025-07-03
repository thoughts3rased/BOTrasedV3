using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Modules
{
    public class GeneralModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Pings the bot to check if it's alive.")]
        public async Task PingAsync()
        {
            await RespondAsync("Pong!");
        }
    }
}
