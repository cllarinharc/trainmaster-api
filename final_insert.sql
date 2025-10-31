-- Inserção final dos dados faltantes

-- 1. INSERIR ENDEREÇOS (IDs corretos dos perfis)
INSERT INTO "AddressEntity" ("PostalCode", "Street", "Neighborhood", "City", "Uf", "PessoalProfileId", "CreateDate", "ModificationDate") VALUES
('20020-040', 'Rua do Ouvidor 50', 'Centro', 'Rio de Janeiro', 'RJ', 99, NOW(), NOW()),
('30130-010', 'Avenida Afonso Pena 3000', 'Centro', 'Belo Horizonte', 'MG', 100, NOW(), NOW()),
('40010-000', 'Avenida Sete de Setembro 100', 'Comércio', 'Salvador', 'BA', 101, NOW(), NOW()),
('01310-200', 'Rua Augusta 800', 'Consolação', 'São Paulo', 'SP', 102, NOW(), NOW()),
('22021-001', 'Av Atlântica 100', 'Copacabana', 'Rio de Janeiro', 'RJ', 103, NOW(), NOW()),
('30140-100', 'Rua da Bahia 1200', 'Centro', 'Belo Horizonte', 'MG', 104, NOW(), NOW());

-- 2. INSERIR DEPARTAMENTOS
INSERT INTO "DepartmentEntity" ("Name", "Description", "IsActive", "UserId", "CreateDate", "ModificationDate") VALUES
('Recursos Humanos', 'Gestão de pessoas, recrutamento e desenvolvimento profissional', true, 81, NOW(), NOW()),
('Tecnologia da Informação', 'Desenvolvimento, infraestrutura e suporte técnico', true, 82, NOW(), NOW());

-- 3. INSERIR EQUIPES
INSERT INTO "TeamEntity" ("Name", "Description", "IsActive", "DepartmentId", "CreateDate", "ModificationDate") VALUES
('RH - Recrutamento', 'Equipe responsável por processos seletivos e recrutamento', true, 1, NOW(), NOW()),
('RH - Treinamento', 'Equipe responsável por treinamentos e desenvolvimento', true, 1, NOW(), NOW()),
('TI - Desenvolvimento', 'Equipe de desenvolvedores de software', true, 2, NOW(), NOW()),
('TI - Infraestrutura', 'Equipe de suporte e infraestrutura', true, 2, NOW(), NOW()),
('TI - Qualidade', 'Equipe de QA e testes', true, 2, NOW(), NOW());

-- 4. INSERIR OPÇÕES DE QUESTÕES
INSERT INTO "QuestionOptionEntity" ("QuestionId", "Text", "IsCorrect", "CreateDate", "ModificationDate")
SELECT q."Id", opts."Text", opts."IsCorrect", NOW(), NOW()
FROM "QuestionEntity" q
CROSS JOIN (VALUES
    ('npm install', true),
    ('npm update', false),
    ('npm create', false),
    ('npm run', false)
) AS opts("Text", "IsCorrect");

-- 5. INSERIR QUESTÕES DE EXAME
INSERT INTO "ExamQuestionEntity" ("ExamId", "QuestionId", "Order", "Points")
SELECT e."Id", q."Id", 1, 10.00
FROM "ExamEntity" e
CROSS JOIN "QuestionEntity" q
LIMIT 14;

SELECT 'Final insert completed!' as status;

