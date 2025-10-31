using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TrainMaster.Extensions;
using TrainMaster.Extensions.ExtensionsLogs;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplicationServices(builder.Configuration);

// Registrar o SeedService
builder.Services.AddScoped<SeedService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy
            .SetIsOriginAllowed(origin =>
            {
                // Allow localhost for development
                if (origin.StartsWith("http://localhost:", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (origin.StartsWith("http://127.0.0.1:", StringComparison.OrdinalIgnoreCase))
                    return true;
                if (origin.StartsWith("https://localhost:", StringComparison.OrdinalIgnoreCase))
                    return true;
                // Allow any origin for production (customize as needed)
                return true;
            })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});



builder.Services.AddAuthorization();

LogExtension.InitializeLogger();
var loggerSerialLog = LogExtension.GetLogger();
loggerSerialLog.Information("Logging initialized.");

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API v1");
});

app.UseRouting();

// CORS must be called after UseRouting but before UseHttpsRedirection
app.UseCors();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<DataContext>();

        // Sempre tentar marcar a migration como aplicada primeiro (se tabelas já existem)
        try
        {
            loggerSerialLog.Information("Verificando se é necessário marcar migration como aplicada...");

            // Criar tabela de histórico de migrations se não existir
            await context.Database.ExecuteSqlRawAsync(@"
                CREATE TABLE IF NOT EXISTS ""__EFMigrationsHistory"" (
                    ""MigrationId"" character varying(150) NOT NULL,
                    ""ProductVersion"" character varying(32) NOT NULL,
                    CONSTRAINT ""PK___EFMigrationsHistory"" PRIMARY KEY (""MigrationId"")
                );
            ");

            // Sempre tentar inserir a migration (se já existir, será ignorado por ON CONFLICT)
            await context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
                VALUES ('20251030143956_Primeira', '8.0.0')
                ON CONFLICT (""MigrationId"") DO NOTHING;
            ");

            loggerSerialLog.Information("Migration '20251030143956_Primeira' verificada/marcada como aplicada.");
        }
        catch (Exception ex)
        {
            loggerSerialLog.Warning(ex, "Aviso ao verificar/marcar migration, mas continuando...");
        }

        // Tentar executar migrations (agora deve funcionar pois já está marcada)
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception migrateEx)
        {
            loggerSerialLog.Warning(migrateEx, "MigrateAsync falhou, mas continuando...");
        }

        // Executar seed do banco de dados
        var seedService = services.GetRequiredService<SeedService>();
        var isSeeded = await seedService.IsDatabaseSeededAsync();

        if (!isSeeded)
        {
            await seedService.SeedDatabaseAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration or seeding!");
    }
}

await app.RunAsync();