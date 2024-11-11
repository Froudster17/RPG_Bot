using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Bot.Data
{
    public class Database
    {
        private readonly string _databasePath;
        public Database(ulong guildID)
        {
            string guildDbFileName = $"{guildID}.db";
            string basePath = Path.Combine(AppContext.BaseDirectory, "Data", "Server Data");

            // Ensure "Data" and "Server Data" folders exist
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
                Console.WriteLine($"Created directory structure: {basePath}");
            }

            _databasePath = Path.Combine(basePath, guildDbFileName);

            Console.WriteLine("Checking if database file exists at: " + _databasePath);

            // Check if the database file exists
            if (!File.Exists(_databasePath))
            {
                Console.WriteLine("Database file does not exist, creating and initializing...");
                InitializeDatabase();
            }
            else
            {
                Console.WriteLine($"Database file already exists for guild ID {guildID}, using existing database.");
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                // Ensure the directory exists before trying to create the database file
                string directoryPath = Path.GetDirectoryName(_databasePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                    Console.WriteLine($"Created directory: {directoryPath}");
                }

                // Connect to the SQLite database at _databasePath (SQLite will create the file if it doesn't exist)
                using var connection = new SqliteConnection($"Data Source={_databasePath}");
                connection.Open();

                // Create the Users table if it doesn't exist
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS Users (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        DiscordUserId TEXT NOT NULL,
                                        Username TEXT,
                                        Points INTEGER DEFAULT 0
                                    );";

                using var command = new SqliteCommand(createTableQuery, connection);
                command.ExecuteNonQuery();

                Console.WriteLine($"Database file created and initialized for guild ID: {_databasePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

    }
}
