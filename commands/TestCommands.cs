using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Bot.commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("test")]
        public async Task TestCommand(CommandContext commandContext)
        {
            await commandContext.Channel.SendMessageAsync($"This is the test {commandContext.User.Username}");
        }

        [Command("embed")]
        public async Task EmbededMessage(CommandContext commandContext)
        {
            var message = new DiscordEmbedBuilder
            {
                Title = "test",
                Description = $"test {commandContext.User.Username}",
                Color = DiscordColor.Aquamarine,
            };

            await commandContext.Channel.SendMessageAsync(embed: message);
        }
    }
}
