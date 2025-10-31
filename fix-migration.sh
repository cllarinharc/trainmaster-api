#!/bin/bash

# Script to mark the migration as applied
# This fixes the issue where tables already exist in the database

echo "🔧 Marking migration '20251030143956_Primeira' as applied..."

cd TrainMaster

# Use dotnet ef to execute raw SQL
dotnet ef database update --connection "Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ekxsphpaocqpewmufmow;Password=JptEWOJvDTil3ggj;" --context DataContext --no-build --verbose 2>&1 | grep -i "error\|exception\|failed" && {
    echo "⚠️  Migration update failed (expected if tables exist)"
    echo ""
    echo "📝 Creating migration history entry..."

    # Create a temporary C# file to mark migration as applied
    cat > ../temp_mark_migration.cs << 'EOFC'
using System;
using Npgsql;

var connString = "Host=aws-1-us-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ekxsphpaocqpewmufmow;Password=JptEWOJvDTil3ggj;";

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
Console.WriteLine("✅ Migration marked as applied successfully!");
EOFC

    echo "⚠️  Please run the SQL manually in Supabase Dashboard SQL Editor:"
    echo ""
    cat ../mark_migration_applied.sql
    echo ""
    echo "Or use the Supabase Dashboard to execute the SQL from mark_migration_applied.sql"
} || echo "✅ Migration completed successfully!"

cd ..

