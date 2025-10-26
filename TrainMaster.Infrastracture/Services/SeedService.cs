using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Seeds;

namespace TrainMaster.Infrastracture.Services
{
    public class SeedService
    {
        private readonly DataContext _context;
        private readonly ILogger<SeedService> _logger;

        public SeedService(DataContext context, ILogger<SeedService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando processo de seed do banco de dados...");

                // Verificar se o banco existe e está acessível
                if (!await _context.Database.CanConnectAsync())
                {
                    _logger.LogError("Não foi possível conectar ao banco de dados");
                    return;
                }

                // Executar o seed
                await DatabaseSeeder.SeedAsync(_context);

                _logger.LogInformation("Seed do banco de dados concluído com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar seed do banco de dados");
                throw;
            }
        }

        public async Task<bool> IsDatabaseSeededAsync()
        {
            try
            {
                return await _context.UserEntity.AnyAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar se o banco foi populado");
                return false;
            }
        }
    }
}
