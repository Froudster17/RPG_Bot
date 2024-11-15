﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Bot.Models
{
    /// <summary>
    /// Represents a user's profile in the RPG bot system.
    /// Stores information about the user, including their Discord ID, username, level, experience points (XP), 
    /// maximum XP required for the next level, and in-game currency (Coins).
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Gets or sets the Discord user ID associated with this profile.
        /// Used as a unique identifier for each profile.
        /// </summary>
        public string DiscordUserId { get; set; }

        /// <summary>
        /// Gets or sets the username associated with this profile.
        /// Reflects the user's Discord username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the level of the user in the RPG system.
        /// This represents the user's current progression stage.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the current XP of the user.
        /// XP (Experience Points) increase as the user participates and can lead to leveling up when reaching <see cref="MaxXp"/>.
        /// </summary>
        public int Xp { get; set; }

        /// <summary>
        /// Gets or sets the maximum XP required for the user to reach the next level.
        /// When <see cref="Xp"/> reaches this value, the user levels up.
        /// </summary>
        public int MaxXp { get; set; }

        /// <summary>
        /// Gets or sets the amount of in-game currency (Coins) the user has.
        /// Coins can be used to purchase items or upgrades within the RPG system.
        /// </summary>
        public int Coin { get; set; }

        /// <summary>
        /// Gets or sets the current health of the user.
        /// This value decreases when the user takes damage in battles or events and can increase with healing.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or sets the maximum health the user can have.
        /// This defines the upper limit for <see cref="Health"/> and may increase as the user levels up or gains items.
        /// </summary>
        public int MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the base damage the user can inflict in combat.
        /// This value determines the amount of damage done to opponents in battles.
        /// </summary>
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the defense level of the user.
        /// This value reduces incoming damage in battles, increasing the user's resilience.
        /// </summary>
        public int Defense { get; set; }

    }
}
