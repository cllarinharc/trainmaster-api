using Microsoft.EntityFrameworkCore;
using TrainMaster.Infrastracture.Connections;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "TrainMaster"))
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

var configuration = builder.Build();

var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
optionsBuilder.UseNpgsql(configuration.GetConnectionString("WebApiDatabase"));

using var context = new DataContext(configuration);

try
{
    Console.WriteLine("🔧 Verificando se a migration precisa ser marcada como aplicada...");

    // Ensure migrations history table exists
    await context.Database.ExecuteSqlRawAsync(@"
        CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
            ""MigrationId"" character varying(150) NOT NULL,
            ""ProductVersion"" character varying(32) NOT NULL,
            CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
        );
    ");

    // Check if migration is already applied
    var migrationExists = await context.Database.ExecuteSqlRawAsync(@"
        SELECT 1 FROM ""__EFMigrationsHistory""
        WHERE ""MigrationId"" = '20251030143956_Primeira'
    ");

    // Mark migration as applied
    var rowsAffected = await context.Database.ExecuteSqlRawAsync(@"
        INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
        VALUES ('20251030143956_Primeira', '8.0.0')
        ON CONFLICT (""MigrationId"") DO NOTHING;
    ");

    Console.WriteLine("✅ Migration '20251030143956_Primeira' marcada como aplicada com sucesso!");
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erro ao marcar migration como aplicada: {ex.Message}");
    return 1;
}
