#r "nuget: Npgsql, 8.0.0"
#r "nuget: Microsoft.Extensions.Configuration, 8.0.0"
#r "nuget: Microsoft.Extensions.Configuration.Json, 8.0.0"

using System;
using System.IO;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("TrainMaster/appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Build();
var connectionString = configuration.GetConnectionString("WebApiDatabase");

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

    SELECT ""MigrationId"" FROM ""__EFMigrationsHistory"" WHERE ""MigrationId"" = '20251030143956_Primeira';
", conn);

var result = await cmd.ExecuteScalarAsync();

if (result != null)
{
    Console.WriteLine("✅ Migration '20251030143956_Primeira' has been marked as applied!");
    Console.WriteLine("You can now run migrations without errors.");
}
else
{
    Console.WriteLine("❌ Failed to mark migration as applied.");
}

