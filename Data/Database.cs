using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            // Check if the database file exists
            if (!File.Exists(_databasePath))
            {
                Console.WriteLine("Database file does not exist, creating and initializing...");
                InitializeDatabase();
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
                // The connection will automatically be closed when the 'using' block ends.
                Console.WriteLine($"Database file created (blank) for guild ID: {_databasePath}");

                // Connection is closed here because of the 'using' block
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        public string DatabasePath => _databasePath;

        public ProfilesTable Profiles => new ProfilesTable(DatabasePath);
    }
}
