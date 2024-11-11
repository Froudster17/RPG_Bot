using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Bot.Models
{
    public class Profile
    {
        public string DiscordUserId { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
        public int MaxXp { get; set; }
    }
}
