-- Insere dados faltantes nas tabelas vazias

-- 1. INSERIR ENDEREÇOS
INSERT INTO "AddressEntity" ("PostalCode", "Street", "Neighborhood", "City", "Uf", "PessoalProfileId", "CreateDate", "ModificationDate")
SELECT postal."PostalCode", postal."Street", postal."Neighborhood", postal."City", postal."Uf", pp."Id", NOW(), NOW()
FROM (VALUES
    ('20020-040', 'Rua do Ouvidor 50', 'Centro', 'Rio de Janeiro', 'RJ'),
    ('30130-010', 'Avenida Afonso Pena 3000', 'Centro', 'Belo Horizonte', 'MG'),
    ('40010-000', 'Avenida Sete de Setembro 100', 'Comércio', 'Salvador', 'BA'),
    ('01310-200', 'Rua Augusta 800', 'Consolação', 'São Paulo', 'SP'),
    ('22021-001', 'Av Atlântica 100', 'Copacabana', 'Rio de Janeiro', 'RJ')
) AS postal("PostalCode", "Street", "Neighborhood", "City", "Uf")
CROSS JOIN (
    SELECT "Id" FROM "PessoalProfileEntity" ORDER BY "Id" LIMIT 5
) pp
WHERE NOT EXISTS (SELECT 1 FROM "AddressEntity" WHERE "AddressEntity"."PessoalProfileId" = pp."Id");

-- 2. INSERIR DEPARTAMENTOS
INSERT INTO "DepartmentEntity" ("Name", "Description", "IsActive", "UserId", "CreateDate", "ModificationDate")
SELECT name, descr, is_act, u."Id", NOW(), NOW()
FROM (VALUES
    ('Recursos Humanos', 'Gestão de pessoas, recrutamento e desenvolvimento profissional', true),
    ('Tecnologia da Informação', 'Desenvolvimento, infraestrutura e suporte técnico', true)
) AS dept(name, descr, is_act)
CROSS JOIN (
    SELECT "Id" FROM "UserEntity" WHERE "Email" LIKE 'gerente.%@trainmaster.com' ORDER BY "Id" LIMIT 2
) u
WHERE NOT EXISTS (SELECT 1 FROM "DepartmentEntity" WHERE "DepartmentEntity"."UserId" = u."Id");

-- 3. INSERIR EQUIPES
INSERT INTO "TeamEntity" ("Name", "Description", "IsActive", "DepartmentId", "CreateDate", "ModificationDate")
SELECT name, descr, is_act, d."Id", NOW(), NOW()
FROM (VALUES
    ('RH - Recrutamento', 'Equipe responsável por processos seletivos', true),
    ('RH - Treinamento', 'Equipe responsável por treinamentos', true),
    ('TI - Desenvolvimento', 'Equipe de desenvolvedores', true),
    ('TI - Infraestrutura', 'Equipe de suporte', true)
) AS teams(name, descr, is_act)
CROSS JOIN (
    SELECT "Id" FROM "DepartmentEntity" ORDER BY "Id"
) d
WHERE NOT EXISTS (SELECT 1 FROM "TeamEntity" WHERE "TeamEntity"."DepartmentId" = d."Id");

-- 4. INSERIR QUESTÕES
INSERT INTO "QuestionEntity" ("CourseActivitieId", "Statement", "Order", "Points", "CreateDate", "ModificationDate")
SELECT ca."Id", 'Qual é o comando para instalar dependências?', 1, 5.00, NOW(), NOW()
FROM "CourseActivitieEntity" ca
WHERE NOT EXISTS (SELECT 1 FROM "QuestionEntity" WHERE "QuestionEntity"."CourseActivitieId" = ca."Id");

SELECT 'Done!' as status;

