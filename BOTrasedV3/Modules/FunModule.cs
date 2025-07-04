using BOTrasedV3.Interfaces;
using BOTrasedV3.Models;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOTrasedV3.Modules
{
    [Group("fun", "Fun commands for entertainment")]
    public class FunModule : InteractionModuleBase<SocketInteractionContext>
    {

        private readonly IFunCommandDataService _funCommandDataService;

        public FunModule(IFunCommandDataService funCommandDataService)
        {
            _funCommandDataService = funCommandDataService;
        }

        [SlashCommand("tellmeasecret", "Tells a random, non-sensical piece of trivia")]
        public async Task TellMeASecretAsync()
        {
            FakeSecret fakeSecret = await _funCommandDataService.GetRandomSecretAsync();

            if (fakeSecret == null)
            {
                await RespondAsync("I don't have any secrets for you right now... Check again later for some wisdom...");
            }
            else
            {
                await RespondAsync(fakeSecret.Content);
            }
        }
    }
}
