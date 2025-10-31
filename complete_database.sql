-- ===================================================================
-- Script para Completar os Dados Faltantes
-- ===================================================================
-- Este script preenche as tabelas que ficaram vazias devido a problemas
-- com sequências de IDs. Ele usa subqueries para encontrar IDs dinâmicos.
-- ===================================================================

-- 1. INSERIR ENDEREÇOS (usando IDs dinâmicos dos perfis pessoais)
INSERT INTO "AddressEntity" ("PostalCode", "Street", "Neighborhood", "City", "Uf", "PessoalProfileId", "CreateDate", "ModificationDate")
SELECT
    addresses."PostalCode",
    addresses."Street",
    addresses."Neighborhood",
    addresses."City",
    addresses."Uf",
    pp."Id" as "PessoalProfileId",
    NOW(),
    NOW()
FROM (VALUES
    ('20020-040', 'Rua do Ouvidor 50', 'Centro', 'Rio de Janeiro', 'RJ'),
    ('30130-010', 'Avenida Afonso Pena 3000', 'Centro', 'Belo Horizonte', 'MG'),
    ('40010-000', 'Avenida Sete de Setembro 100', 'Comércio', 'Salvador', 'BA'),
    ('01310-200', 'Rua Augusta 800', 'Consolação', 'São Paulo', 'SP'),
    ('22021-001', 'Av Atlântica 100', 'Copacabana', 'Rio de Janeiro', 'RJ'),
    ('30140-100', 'Rua da Bahia 1200', 'Centro', 'Belo Horizonte', 'MG'),
    ('52010-001', 'Avenida Boa Viagem 500', 'Boa Viagem', 'Recife', 'PE'),
    ('01308-001', 'Avenida Faria Lima 2000', 'Itaim Bibi', 'São Paulo', 'SP'),
    ('22041-001', 'Av Nehmen Prates 500', 'Copacabana', 'Rio de Janeiro', 'RJ'),
    ('30110-000', 'Rua Rio de Janeiro 1500', 'Centro', 'Belo Horizonte', 'MG'),
    ('40015-020', 'Praça Castro Alves 10', 'Centro', 'Salvador', 'BA'),
    ('51020-001', 'Av Boa Viagem 200', 'Boa Viagem', 'Recife', 'PE'),
    ('01310-300', 'Rua Joaquim Floriano 400', 'Itaim Bibi', 'São Paulo', 'SP'),
    ('20070-020', 'Av Marechal Floriano 200', 'Centro', 'Rio de Janeiro', 'RJ'),
    ('30140-200', 'Rua dos Caetés 600', 'Funcionários', 'Belo Horizonte', 'MG'),
    ('52010-200', 'Rua do Brum 200', 'Recife Antigo', 'Recife', 'PE')
) AS addresses("PostalCode", "Street", "Neighborhood", "City", "Uf")
CROSS JOIN (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "PessoalProfileEntity"
) pp;

-- 2. INSERIR NÍVEIS DE EDUCAÇÃO
INSERT INTO "EducationLevelEntity" ("Title", "Institution", "StartedAt", "EndeedAt", "ProfessionalProfileId", "CreateDate", "ModificationDate")
SELECT
    edu."Title",
    edu."Institution",
    edu."StartedAt",
    edu."EndeedAt",
    prof."Id",
    NOW(),
    NOW()
FROM (VALUES
    ('Bacharelado em Sistemas de Informação', 'PUC-Rio', '2005-01-01'::timestamp, '2009-12-31'::timestamp, 1),
    ('Mestrado em Ciência da Computação', 'Universidade Estadual de Campinas', '2010-01-01'::timestamp, '2012-12-31'::timestamp, 1),
    ('Bacharelado em Estatística', 'Universidade Federal do Rio de Janeiro', '2008-01-01'::timestamp, '2012-12-31'::timestamp, 2),
    ('Mestrado em Ciência de Dados', 'Instituto de Matemática Pura e Aplicada', '2013-01-01'::timestamp, '2015-12-31'::timestamp, 2)
) AS edu("Title", "Institution", "StartedAt", "EndeedAt", profile_order)
CROSS JOIN (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "ProfessionalProfileEntity"
    ORDER BY "Id"
    LIMIT 4
) prof WHERE prof.rn = edu.profile_order;

-- 3. INSERIR DEPARTAMENTOS
INSERT INTO "DepartmentEntity" ("Name", "Description", "IsActive", "UserId", "CreateDate", "ModificationDate")
SELECT
    dept."Name",
    dept."Description",
    dept."IsActive",
    u."Id" as "UserId",
    NOW(),
    NOW()
FROM (VALUES
    ('Recursos Humanos', 'Gestão de pessoas, recrutamento e desenvolvimento profissional', true, 2),
    ('Tecnologia da Informação', 'Desenvolvimento, infraestrutura e suporte técnico', true, 3)
) AS dept("Name", "Description", "IsActive", user_offset)
CROSS JOIN (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "UserEntity"
    WHERE "Email" LIKE 'gerente.%@trainmaster.com'
    ORDER BY "Id"
) u;

-- 4. INSERIR EQUIPES
INSERT INTO "TeamEntity" ("Name", "Description", "IsActive", "DepartmentId", "CreateDate", "ModificationDate")
SELECT
    teams."Name",
    teams."Description",
    teams."IsActive",
    d."Id" as "DepartmentId",
    NOW(),
    NOW()
FROM (VALUES
    ('RH - Recrutamento', 'Equipe responsável por processos seletivos e recrutamento', true, 1),
    ('RH - Treinamento', 'Equipe responsável por treinamentos e desenvolvimento', true, 1),
    ('TI - Desenvolvimento', 'Equipe de desenvolvedores de software', true, 2),
    ('TI - Infraestrutura', 'Equipe de suporte e infraestrutura', true, 2),
    ('TI - Qualidade', 'Equipe de QA e testes', true, 2)
) AS teams("Name", "Description", "IsActive", dept_offset)
CROSS JOIN (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "DepartmentEntity"
    ORDER BY "Id"
    LIMIT 2
) d WHERE d.rn = teams.dept_offset;

-- 5. INSERIR ATIVIDADES DE CURSO
INSERT INTO "CourseActivitieEntity" ("Title", "Description", "StartDate", "DueDate", "MaxScore", "CourseId", "CreateDate", "ModificationDate")
SELECT
    activities."Title",
    activities."Description",
    activities."StartDate",
    activities."DueDate",
    activities."MaxScore",
    c."Id" as "CourseId",
    NOW(),
    NOW()
FROM (VALUES
    ('Configuração do Ambiente', 'Configure Node.js, NPM e seu primeiro projeto React', '2024-01-15'::timestamp, '2024-01-18'::timestamp, 10, 1),
    ('Componentes e Props', 'Entenda componentes React e passagem de propriedades', '2024-01-19'::timestamp, '2024-01-25'::timestamp, 20, 1),
    ('State e Hooks', 'Gerenciamento de estado com useState e useEffect', '2024-01-26'::timestamp, '2024-02-05'::timestamp, 25, 1),
    ('Introdução ao Python', 'Fundamentos de Python para Data Science', '2024-02-01'::timestamp, '2024-02-10'::timestamp, 15, 2),
    ('Pandas: Manipulação de Dados', 'Análise e transformação de dados com Pandas', '2024-02-11'::timestamp, '2024-02-25'::timestamp, 30, 2)
) AS activities("Title", "Description", "StartDate", "DueDate", "MaxScore", course_offset)
CROSS JOIN (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "CourseEntity"
    ORDER BY "Id"
    LIMIT 8
) c WHERE c.rn = activities.course_offset;

-- 6. INSERIR QUESTÕES
INSERT INTO "QuestionEntity" ("CourseActivitieId", "Statement", "Order", "Points", "CreateDate", "ModificationDate")
SELECT
    ca."Id" as "CourseActivitieId",
    q."Statement",
    q."Order",
    q."Points",
    NOW(),
    NOW()
FROM (VALUES
    ('Qual é o comando para instalar dependências do projeto React?', 1, 5.00),
    ('O que são Props em React?', 1, 6.67),
    ('Qual é a sintaxe para criar uma lista em Python?', 1, 5.00)
) AS q("Statement", "Order", "Points")
CROSS JOIN (
    SELECT "Id", ROW_NUMBER() OVER (ORDER BY "Id") as rn
    FROM "CourseActivitieEntity"
    ORDER BY "Id"
    LIMIT 3
) ca WHERE ca.rn <= 3;

-- 7. INSERIR OPÇÕES DAS QUESTÕES
INSERT INTO "QuestionOptionEntity" ("QuestionId", "Text", "IsCorrect", "CreateDate", "ModificationDate")
SELECT
    q."Id" as "QuestionId",
    opts."Text",
    opts."IsCorrect",
    NOW(),
    NOW()
FROM "QuestionEntity" q
CROSS JOIN (VALUES
    ('npm install', true),
    ('npm update', false),
    ('npm create', false),
    ('npm run', false)
) AS opts("Text", "IsCorrect");

-- 8. INSERIR EXAMES
INSERT INTO "ExamEntity" ("CourseId", "Title", "Instructions", "StartAt", "EndAt", "IsPublished", "CreateDate", "ModificationDate")
SELECT
    c."Id" as "CourseId",
    'Exame Final',
    'Complete o exame em até 2 horas. Boa sorte!',
    '2024-04-10 14:00:00'::timestamp,
    '2024-04-10 16:00:00'::timestamp,
    true,
    NOW(),
    NOW()
FROM "CourseEntity" c
ORDER BY c."Id"
LIMIT 5;

-- 9. INSERIR QUESTÕES DE EXAME
INSERT INTO "ExamQuestionEntity" ("ExamId", "QuestionId", "Order", "Points")
SELECT
    e."Id" as "ExamId",
    q."Id" as "QuestionId",
    1,
    10.00
FROM "ExamEntity" e
CROSS JOIN "QuestionEntity" q
LIMIT 5;

-- 10. INSERIR USER BADGES
INSERT INTO "UserBadgeEntity" ("UserId", "BadgeId", "EarnedAt")
SELECT
    u."Id" as "UserId",
    b."Id" as "BadgeId",
    NOW()
FROM "UserEntity" u
CROSS JOIN "BadgeEntity" b
WHERE u."Email" LIKE '%@trainmaster.com'
LIMIT 13;

-- 11. INSERIR FEEDBACK DE CURSO
INSERT INTO "CourseFeedbackEntity" ("CourseId", "StudentId", "Rating", "Comment", "CreateDate", "ModificationDate")
SELECT
    c."Id" as "CourseId",
    u."Id" as "StudentId",
    5,
    'Excelente curso!',
    NOW(),
    NOW()
FROM "CourseEntity" c
CROSS JOIN "UserEntity" u
WHERE u."Email" LIKE '%@trainmaster.com' AND u."Email" NOT LIKE 'prof.%@trainmaster.com' AND u."Email" NOT LIKE 'gerente.%@trainmaster.com'
LIMIT 11;

-- 12. INSERIR AVALIAÇÕES DE CURSO
INSERT INTO "CourseAvaliationEntity" ("Rating", "Comment", "ReviewDate", "CourseId", "CreateDate", "ModificationDate")
SELECT
    5,
    'Melhor curso que já vi!',
    NOW(),
    c."Id",
    NOW(),
    NOW()
FROM "CourseEntity" c
ORDER BY c."Id"
LIMIT 6;

-- 13. INSERIR HISTÓRICO DE SENHAS
INSERT INTO "HistoryPasswordEntity" ("OldPassword", "UserId", "CreateDate", "ModificationDate")
SELECT
    'OldPass123!',
    u."Id",
    '2023-12-01 10:00:00'::timestamp,
    NOW()
FROM "UserEntity" u
WHERE u."Email" LIKE '%@trainmaster.com'
LIMIT 5;

-- 14. INSERIR NOTIFICAÇÕES
INSERT INTO "NotificationEntity" ("Description", "CourseId", "CreateDate", "ModificationDate")
SELECT
    'Nova atividade disponível!',
    c."Id",
    NOW(),
    NOW()
FROM "CourseEntity" c
ORDER BY c."Id"
LIMIT 8;

SELECT 'Script completed successfully!' as status;

