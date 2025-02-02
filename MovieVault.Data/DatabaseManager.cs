using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MovieVault.Data
{
    public static class DatabaseManager
    {
        private static string _connectionString = string.Empty;

        static DatabaseManager()
        {
            LoadConfiguration();
        }

        private static void LoadConfiguration()
        {
            Console.WriteLine("Loading appsettings.json from UI...");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = config.GetConnectionString("MovieVaultDB")
                ?? throw new InvalidOperationException("Database connection string is missing.");

            Console.WriteLine($"Connection String Loaded: {_connectionString}");
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
