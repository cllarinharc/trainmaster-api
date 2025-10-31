using System;
using System.IO;
using System.Threading.Tasks;
using Npgsql;
using Newtonsoft.Json.Linq;

async Task Main()
{
    try
    {
        // Read connection string from appsettings.json
        var appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "TrainMaster", "appsettings.json");
        var json = File.ReadAllText(appSettingsPath);
        var config = JObject.Parse(json);
        var connectionString = config["ConnectionStrings"]?["WebApiDatabase"]?.ToString();

        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("❌ Connection string not found in appsettings.json");
            return;
        }

        Console.WriteLine("🔧 Marking migration '20251030143956_Primeira' as applied...");

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        var cmd = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
                ""MigrationId"" character varying(150) NOT NULL,
                ""ProductVersion"" character varying(32) NOT NULL,
                CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
            );

            INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
            VALUES ('20251030143956_Primeira', '8.0.0')
            ON CONFLICT (""MigrationId"") DO NOTHING;
        ", conn);

        await cmd.ExecuteNonQueryAsync();

        Console.WriteLine("✅ Migration marked as applied successfully!");
        Console.WriteLine("You can now run the API without migration errors.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}");
        Console.WriteLine("\n⚠️  Please run the SQL manually in Supabase Dashboard:");
        Console.WriteLine(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "mark_migration_applied.sql")));
    }
}

