-- Script para criar a tabela ExamHistoryEntity caso não exista
CREATE TABLE IF NOT EXISTS "ExamHistoryEntity" (
    "Id" SERIAL PRIMARY KEY,
    "ExamId" INTEGER NOT NULL,
    "StudentId" INTEGER NOT NULL,
    "AttemptNumber" INTEGER NOT NULL,
    "StartedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "FinishedAt" TIMESTAMP WITH TIME ZONE,
    "Score" NUMERIC(10,2),
    "DurationSeconds" INTEGER,
    "Status" INTEGER NOT NULL,
    "CreateDate" TIMESTAMP WITH TIME ZONE,
    "ModificationDate" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_ExamHistoryEntity_ExamEntity_ExamId"
        FOREIGN KEY ("ExamId")
        REFERENCES "ExamEntity"("Id")
        ON DELETE CASCADE,
    CONSTRAINT "FK_ExamHistoryEntity_UserEntity_StudentId"
        FOREIGN KEY ("StudentId")
        REFERENCES "UserEntity"("Id")
        ON DELETE RESTRICT
);

-- Criar índices
CREATE INDEX IF NOT EXISTS "IX_ExamHistoryEntity_StudentId" ON "ExamHistoryEntity"("StudentId");
CREATE INDEX IF NOT EXISTS "IX_ExamHistoryEntity_ExamId_StudentId" ON "ExamHistoryEntity"("ExamId", "StudentId");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_ExamHistoryEntity_ExamId_StudentId_AttemptNumber"
    ON "ExamHistoryEntity"("ExamId", "StudentId", "AttemptNumber");

