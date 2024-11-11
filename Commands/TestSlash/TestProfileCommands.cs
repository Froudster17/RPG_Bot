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
            string discordUserID = interactionContext.User.Id.ToString();
            string username = interactionContext.User.Username;

            var database = new Database(guildId);
            var profilesTable = new ProfilesTable(database.DatabasePath);

            var profile = profilesTable.GetProfile(discordUserID);

            if (profile == null)
            {
                profilesTable.CreateProfile(discordUserID, username);
                profile = profilesTable.GetProfile(discordUserID);
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{username}'s Profile",
                Color = DiscordColor.Azure
            };

            embed.AddField("Username", profile.Username, true);
            embed.AddField("Level", profile.Level.ToString(), true);
            embed.AddField("XP", profile.Xp.ToString(), true);

            await interactionContext.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));


        }
    }
}
