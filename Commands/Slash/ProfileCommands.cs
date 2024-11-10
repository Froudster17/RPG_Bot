using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Bot.Commands.Slash
{
    public class ProfileCommands : ApplicationCommandModule
    {
        [SlashCommand("Profile", "Displays the users profile")]
        public async Task ProfileCommand(InteractionContext interactionContext)
        {

            await interactionContext.DeferAsync();

            await interactionContext.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Hello World"));
        }
    }
}
