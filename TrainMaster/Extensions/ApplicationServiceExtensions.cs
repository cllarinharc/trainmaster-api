using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using TrainMaster.Application.Services;
using TrainMaster.Application.Services.Interfaces;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Extensions.SwaggerDocumentation;
using TrainMaster.Infrastracture.Connections;
using TrainMaster.Infrastracture.Repository.Interfaces;
using TrainMaster.Infrastracture.Repository.RepositoryUoW;
using TrainMaster.Infrastracture.Repository.Request;
using TrainMaster.Infrastracture.Security.Cryptography;
using TrainMaster.Infrastracture.Security.Token.Access;

namespace TrainMaster.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API TrainMaster",
                    Description = @"
                    A **API TrainMaster** é uma solução inovadora para gerenciamento e otimização de treinamentos corporativos. 
                    Criada para oferecer um ambiente de aprendizado online acessível e flexível, ela permite que funcionários 
                    realizem treinamentos a qualquer hora, de qualquer lugar e em qualquer dispositivo.

                    🚀 **Principais Benefícios:**
                    - 📚 Aulas e conteúdos organizados de forma eficiente.
                    - 📝 Provas e avaliações para medir o progresso dos colaboradores.
                    - 🌍 Acesso global em qualquer dispositivo, proporcionando flexibilidade.
        
                    Com a **TrainMaster**, sua empresa pode capacitar colaboradores de maneira ágil, prática e eficiente!
                ",
                });


                opt.OperationFilter<CustomOperationDescriptions>();
            });

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseNpgsql(config.GetConnectionString("WebApiDatabase"));
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200");
                });
            });
            services.AddScoped<IRepositoryUoW, RepositoryUoW>();
            services.AddScoped<TokenService>();
            services.AddScoped<BCryptoAlgorithm>();
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
            services.AddScoped<AuthService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<TokenService>();
            services.AddScoped<BCryptoAlgorithm>();


            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            return services;
        }
    }
}