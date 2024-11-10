using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
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
    }
}
