#!/bin/bash

echo "🔧 Fixing migration issue and running API..."

# Step 1: Mark migration as applied
echo "Step 1: Marcando migration como aplicada..."
cd TrainMaster
dotnet run --project ../MarkMigrationApplied.cs --no-build 2>/dev/null || {
    # If that doesn't work, use direct SQL approach
    echo "Tentando abordagem alternativa..."
    cd ..

    # Create a simple C# program to mark migration
    cat > MarkMigrationTool.cs << 'EOF'
using System;
using System.Threading.Tasks;
using Npgsql;

class Program
{
    static async Task Main()
    {
        var connString = "Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ekxsphpaocqpewmufmow;Password=JptEWOJvDTil3ggj;";

        try
        {
            await using var conn = new NpgsqlConnection(connString);
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
            Console.WriteLine("✅ Migration marcada como aplicada!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
EOF

    # Try to compile and run (requires Npgsql package reference)
    echo "⚠️  Por favor, execute o SQL manualmente no Supabase Dashboard ou use o script mark_migration_applied.sql"
    echo ""
    echo "SQL para executar:"
    cat mark_migration_applied.sql
    echo ""
    echo "Após executar o SQL, pressione ENTER para continuar..."
    read
}

cd ..

# Step 2: Build project
echo ""
echo "Step 2: Compilando projeto..."
./run-api.sh --build-only || {
    echo "❌ Falha na compilação"
    exit 1
}

# Step 3: Run API
echo ""
echo "Step 3: Executando API..."
./run-api.sh

