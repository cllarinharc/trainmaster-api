using Microsoft.EntityFrameworkCore;
using TrainMaster.Infrastracture.Connections;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "TrainMaster"))
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .Build();

try {
    using var ctx = new DataContext(config);
    await ctx.Database.ExecuteSqlRawAsync(@"
        CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
            ""MigrationId"" character varying(150) NOT NULL,
            ""ProductVersion"" character varying(32) NOT NULL,
            CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
        );
        INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
        VALUES ('20251030143956_Primeira', '8.0.0')
        ON CONFLICT (""MigrationId"") DO NOTHING;
    ");
    Console.WriteLine("✅ Migration marcada como aplicada!");
} catch (Exception ex) {
    Console.WriteLine($"❌ Erro: {ex.Message}");
    Environment.Exit(1);
}
