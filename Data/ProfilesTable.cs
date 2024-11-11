using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Bot.Data
{
    public  class ProfilesTable
    {
        private readonly string _databasePath;

        public ProfilesTable(string databasePath)
        {
            _databasePath = databasePath;
            EnsureProfilesTableExists();
        }

        // Ensure the Profiles table exists
        private void EnsureProfilesTableExists()
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    connection.Open();

                    // SQL query to create the Profiles table if it doesn't already exist
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Profiles (
                            DiscordUserId TEXT PRIMARY KEY,
                            Username TEXT,
                            Level INTEGER DEFAULT 0,
                            Xp INTEGER DEFAULT 0,
                            MaxXp INTEGER DEFAULT 0
                        );";

                    using (var command = new SqliteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Profiles table ensured (created if necessary).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring Profiles table exists: {ex.Message}");
            }
        }


        // Create a new profile for the user
        public void CreateProfile(string discordUserId, string username)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    connection.Open();

                    // SQL query to insert a new profile into the database
                    string query = @"INSERT INTO Profiles (DiscordUserId, Username, Level, Xp, MaxXp) 
                                     VALUES (@DiscordUserId, @Username, @Level, @Xp, @MaxXp)";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.Add("@DiscordUserId", SqliteType.Text).Value = discordUserId;
                        command.Parameters.Add("@Username", SqliteType.Text).Value = username;
                        command.Parameters.Add("@Level", SqliteType.Integer).Value = 0;   // Default Level
                        command.Parameters.Add("@Xp", SqliteType.Integer).Value = 0;      // Default XP
                        command.Parameters.Add("@MaxXp", SqliteType.Integer).Value = 10;  // Default MaxXP

                        // Execute the query to create the profile
                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine($"Profile created for user {username} ({discordUserId}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating profile: {ex.Message}");
            }
        }

        // Check if a profile already exists for the given DiscordUserId
        public bool DoesProfileExist(string discordUserId)
        {
            try
            {
                using (var connection = new SqliteConnection($"Data Source={_databasePath}"))
                {
                    connection.Open();

                    string checkQuery = @"SELECT COUNT(1) FROM Profiles WHERE DiscordUserId = @DiscordUserId";
                    using (var command = new SqliteCommand(checkQuery, connection))
                    {
                        command.Parameters.Add("@DiscordUserId", SqliteType.Text).Value = discordUserId;

                        long count = (long)command.ExecuteScalar();
                        return count > 0; // Returns true if profile exists
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking profile existence: {ex.Message}");
                return false; // Assume profile doesn't exist in case of error
            }
        }
    }
}
