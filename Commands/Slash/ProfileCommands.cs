using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using RPG_Bot.Data;
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

            // Create a new embed message
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{username}'s Profile",
                Color = DiscordColor.Azure,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = interactionContext.User.AvatarUrl
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "Use /help for more commands." }
            };

            // Adding fields to the embed with emojis/icons
            embed.AddField("**🟦 Level**", $"{profile.Level}", true); // Level with a bar chart emoji
            embed.AddField("**⚡ XP**", $"{profile.Xp} / {profile.MaxXp}", true); // XP with a star emoji
            embed.AddField("**💰 Coin**", $"{profile.Coin}", true); // Coin with a money bag emoji

            // Add a separator line with a description
            embed.AddField("**Attributes**", "=================================", false);
            embed.AddField("**❤️ Health**", $"{profile.Health} / {profile.MaxHealth}", true); // Health with heart emoji
            embed.AddField("**⚔️ Damage**", $"{profile.Damage}", true); // Damage with explosion emoji
            embed.AddField("**🛡️ Defense**", $"{profile.Defense}", true); // Defense with shield emoji

            // Sending the embed as a response
            await interactionContext.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embed));
        }
    }
}
