using Microsoft.Data.Sqlite;
using RPG_Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Bot.Data
{
    /// <summary>
    /// Manages the operations for the Profiles table in the database.
    /// Responsible for creating the table, adding profiles, retrieving profiles, and ensuring schema consistency.
    /// </summary>
    public class ProfilesTable
    {
        private readonly string _databasePath;

        /// <summary>
        /// Defines the expected columns for the Profiles table along with their SQL types and default values.
        /// </summary>
        private readonly Dictionary<string, string> _expectedColumns = new()
{
            { "DiscordUserId", "TEXT PRIMARY KEY" },
            { "Username", "TEXT" },
            { "Level", "INTEGER DEFAULT 0" },
            { "Xp", "INTEGER DEFAULT 0" },
            { "MaxXp", "INTEGER DEFAULT 10" },
            { "Coin", "INTEGER DEFAULT 0" }
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilesTable"/> class.
        /// Ensures that the Profiles table exists in the database.
        /// </summary>
        /// <param name="databasePath">The file path of the database.</param>
        public ProfilesTable(string databasePath)
        {
            _databasePath = databasePath;
            EnsureProfilesTableExists();
        }


        /// <summary>
        /// Ensures the Profiles table exists in the database. Creates the table if it does not exist.
        /// Checks for each required column, adding any missing columns.
        /// </summary>
        private void EnsureProfilesTableExists()
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={_databasePath}");
                connection.Open();

                // SQL query to create the Profiles table if it doesn't already exist
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Profiles (
                        DiscordUserId TEXT PRIMARY KEY,
                        Username TEXT,
                        Level INTEGER DEFAULT 0,
                        Xp INTEGER DEFAULT 0,
                        MaxXp INTEGER DEFAULT 10,
                        Coin INTEGER DEFAULT 0
                    );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Ensure all columns are present in the table
                foreach (var column in _expectedColumns)
                {
                    EnsureColumnExists(connection, column.Key, column.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring Profiles table exists: {ex.Message}");
            }
        }

        /// <summary>
        /// Ensures a specific column exists in the Profiles table. Adds the column if it is missing.
        /// </summary>
        /// <param name="connection">The active SQLite connection.</param>
        /// <param name="columnName">The name of the column to check.</param>
        /// <param name="columnDefinition">The SQL definition for the column, including type and default value.</param>
        private void EnsureColumnExists(SqliteConnection connection, string columnName, string columnDefinition)
        {
            string checkColumnQuery = $"PRAGMA table_info(Profiles);";
            using var command = new SqliteCommand(checkColumnQuery, connection);
            using var reader = command.ExecuteReader();

            bool columnExists = false;
            while (reader.Read())
            {
                if (reader["name"].ToString() == columnName)
                {
                    columnExists = true;
                    break;
                }
            }

            // If the column doesn't exist, add it to the table
            if (!columnExists)
            {
                string addColumnQuery = $"ALTER TABLE Profiles ADD COLUMN {columnName} {columnDefinition};";
                using var alterCommand = new SqliteCommand(addColumnQuery, connection);
                alterCommand.ExecuteNonQuery();
                Console.WriteLine($"Added missing column '{columnName}' to Profiles table.");
            }
        }

        /// <summary>
        /// Creates a new profile for a user if it doesn't already exist.
        /// </summary>
        /// <param name="discordUserId">The Discord user ID of the profile owner.</param>
        /// <param name="username">The Discord username of the profile owner.</param>
        public void CreateProfile(string discordUserId, string username)
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={_databasePath}");
                connection.Open();

                string query = @"INSERT INTO Profiles (DiscordUserId, Username, Level, Xp, MaxXp, Coin)
                                 VALUES (@DiscordUserId, @Username, @Level, @Xp, @MaxXp, @Coin)";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DiscordUserId", discordUserId);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Level", 0);  // Default Level
                    command.Parameters.AddWithValue("@Xp", 0);     // Default XP
                    command.Parameters.AddWithValue("@MaxXp", 10); // Default MaxXP
                    command.Parameters.AddWithValue("@Coin", 0);   // Default Coin

                    command.ExecuteNonQuery();
                }

                Console.WriteLine($"Profile created for user {username} ({discordUserId}).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating profile: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the profile of a user by their Discord user ID.
        /// </summary>
        /// <param name="discordUserId">The Discord user ID to search for.</param>
        /// <returns>The profile data if found, otherwise null.</returns>
        public Profile? GetProfile(string discordUserId)
        {
            using var connection = new SqliteConnection($"Data Source={_databasePath}");
            connection.Open();

            string query = @"SELECT DiscordUserId, Username, Level, Xp, MaxXp, Coin FROM Profiles WHERE DiscordUserId = @DiscordUserId";
            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@DiscordUserId", discordUserId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Profile
                {
                    DiscordUserId = reader.GetString(0),
                    Username = reader.GetString(1),
                    Level = reader.GetInt32(2),
                    Xp = reader.GetInt32(3),
                    MaxXp = reader.GetInt32(4),
                    Coin = reader.GetInt32(5)
                };
            }
            return null;
        }

        /// <summary>
        /// Checks if a profile already exists for the given Discord user ID.
        /// </summary>
        /// <param name="discordUserId">The Discord user ID to check.</param>
        /// <returns>True if the profile exists, false otherwise.</returns>
        public bool DoesProfileExist(string discordUserId)
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={_databasePath}");
                connection.Open();

                string checkQuery = @"SELECT COUNT(1) FROM Profiles WHERE DiscordUserId = @DiscordUserId";
                using var command = new SqliteCommand(checkQuery, connection);
                command.Parameters.AddWithValue("@DiscordUserId", discordUserId);

                long count = (long)command.ExecuteScalar();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking profile existence: {ex.Message}");
                return false;
            }
        }
    }
}

