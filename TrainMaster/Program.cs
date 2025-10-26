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
        await context.Database.MigrateAsync();

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