using Microsoft.EntityFrameworkCore;
using TrainMaster.Infrastracture.Connections;
using Microsoft.Extensions.Configuration;

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "TrainMaster"))
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile("appsettings.Development.json", optional: true);

var configuration = configBuilder.Build();

try
{
    using var context = new DataContext(configuration);
    
    Console.WriteLine("🔧 Marcando migration como aplicada...");
    
    await context.Database.ExecuteSqlRawAsync(@"
        CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
            ""MigrationId"" character varying(150) NOT NULL,
            ""ProductVersion"" character varying(32) NOT NULL,
            CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
        );
    ");
    
    await context.Database.ExecuteSqlRawAsync(@"
        INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
        VALUES ('20251030143956_Primeira', '8.0.0')
        ON CONFLICT (""MigrationId"") DO NOTHING;
    ");
    
    Console.WriteLine("✅ Migration marcada como aplicada!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erro: {ex.Message}");
    Environment.Exit(1);
}
