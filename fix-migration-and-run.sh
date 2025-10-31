#!/bin/bash

# Script para marcar migration como aplicada e executar API

set +e  # Não parar em erros

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

print_success() { echo -e "${GREEN}✅ $1${NC}"; }
print_warning() { echo -e "${YELLOW}⚠️  $1${NC}"; }
print_error() { echo -e "${RED}❌ $1${NC}"; }
print_info() { echo -e "${BLUE}ℹ️  $1${NC}"; }

print_info "Marcando migration como aplicada..."

# Criar um programa C# temporário para marcar a migration
cd TrainMaster

# Tentar usar dotnet-script se disponível, senão criar um programa temporário
cat > ../temp_mark_migration.cs << 'EOFC'
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
EOFC

# Tentar executar diretamente via dotnet
print_info "Tentando marcar migration usando Npgsql diretamente..."

# Criar script SQL direto
cat > ../execute_migration_fix.sql << 'EOFSQL'
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251030143956_Primeira', '8.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;
EOFSQL

cd ..

# Como não temos psql, vamos criar uma solução usando o projeto TrainMaster
print_info "Criando programa temporário para marcar migration..."

# Criar um projeto temporário
mkdir -p temp-migration-fix
cd temp-migration-fix
cat > Program.cs << 'EOFPROG'
using Microsoft.EntityFrameworkCore;
using TrainMaster.Infrastracture.Connections;
using Microsoft.Extensions.Configuration;
using System.Reflection;

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "TrainMaster"))
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

    var result = await context.Database.ExecuteSqlRawAsync(@"
        INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
        VALUES ('20251030143956_Primeira', '8.0.0')
        ON CONFLICT (""MigrationId"") DO NOTHING;
    ");

    Console.WriteLine("✅ Migration marcada como aplicada com sucesso!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erro: {ex.Message}");
    Console.WriteLine($"Stack: {ex.StackTrace}");
    Environment.Exit(1);
}
EOFPROG

cat > MigrationFix.csproj << 'EOFPROJ'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="../TrainMaster.Infrastracture/TrainMaster.Infrastracture.csproj" />
    <ProjectReference Include="../TrainMaster.Domain/TrainMaster.Domain.csproj" />
  </ItemGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="8.0.0" />
  </ItemGroup>
</Project>
EOFPROJ

# Compilar e executar
print_info "Compilando e executando programa de fix..."
if dotnet build -q 2>/dev/null && dotnet run --no-build 2>/dev/null; then
    print_success "Migration marcada como aplicada!"
    cd ..
    rm -rf temp-migration-fix
else
    print_warning "Não foi possível marcar migration automaticamente."
    print_info "Você precisa executar o SQL manualmente no Supabase Dashboard:"
    echo ""
    cat ../execute_migration_fix.sql
    echo ""
    print_info "Após executar o SQL, pressione ENTER para continuar..."
    read
    cd ..
    rm -rf temp-migration-fix
fi

# Agora executar o run-api.sh
print_info "Executando run-api.sh..."
cd ..
./run-api.sh

