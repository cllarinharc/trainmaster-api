-- Script to mark migration '20251030143956_Primeira' as applied
-- Run this if all tables from the migration already exist in your database

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251030143956_Primeira', '8.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;
