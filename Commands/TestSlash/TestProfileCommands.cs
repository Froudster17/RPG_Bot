using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RPG_Bot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Bot.Commands.TestSlash
{
    public class TestProfileCommands : ApplicationCommandModule
    {
        [SlashCommand("Profile", "Displays the users profile")]
        public async Task ProfileCommand(InteractionContext interactionContext)
        {
            await interactionContext.DeferAsync();
            ulong guildId = interactionContext.Guild.Id;

            var database = new Database(guildId);

            await interactionContext.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Hello World"));
        }
    }
}
