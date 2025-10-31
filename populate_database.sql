-- ===================================================================
-- Script de População de Dados Falsos para TrainMaster
-- ===================================================================
-- Este script popula todas as tabelas do banco de dados com dados
-- realistas para fins de estudo e demonstração
-- ===================================================================
--
-- ATENÇÃO: Se você já executou este script antes e quer reexecutá-lo,
-- execute primeiro estes comandos SQL para limpar os dados anteriores:
--
-- DELETE FROM "UserBadgeEntity" WHERE "UserId" >= 2;
-- DELETE FROM "BadgeEntity";
-- DELETE FROM "HistoryPasswordEntity" WHERE "UserId" >= 2;
-- DELETE FROM "NotificationEntity";
-- DELETE FROM "ExamQuestionEntity";
-- DELETE FROM "QuestionOptionEntity";
-- DELETE FROM "ExamEntity";
-- DELETE FROM "QuestionEntity";
-- DELETE FROM "CourseActivitieEntity";
-- DELETE FROM "CourseFeedbackEntity";
-- DELETE FROM "CourseAvaliationEntity";
-- DELETE FROM "CourseEntity";
-- DELETE FROM "TeamEntity";
-- DELETE FROM "DepartmentEntity";
-- DELETE FROM "EducationLevelEntity" WHERE "ProfessionalProfileId" >= 1;
-- DELETE FROM "AddressEntity" WHERE "PessoalProfileId" >= 1;
-- DELETE FROM "ProfessionalProfileEntity" WHERE "UserId" >= 2;
-- DELETE FROM "PessoalProfileEntity" WHERE "UserId" >= 2;
-- DELETE FROM "UserEntity" WHERE "Id" >= 2;
--
-- ===================================================================

-- Limpar dados existentes (opcional - descomente se necessário)
-- TRUNCATE TABLE "UserBadgeEntity", "BadgeEntity", "HistoryPasswordEntity",
--   "NotificationEntity", "ExamQuestionEntity", "QuestionOptionEntity",
--   "ExamEntity", "QuestionEntity", "CourseActivitieEntity",
--   "CourseFeedbackEntity", "CourseAvaliationEntity", "TeamEntity",
--   "DepartmentEntity", "EducationLevelEntity", "AddressEntity",
--   "ProfessionalProfileEntity", "PessoalProfileEntity", "CourseEntity",
--   "UserEntity" CASCADE;

-- ===================================================================
-- IMPORTANTE: Este script assume que já existe um usuário com ID 1
-- Se precisar começar do zero, descomente o TRUNCATE acima
-- ===================================================================

-- Limpar dados de teste anteriores (em ordem reversa de dependências)
DELETE FROM "UserBadgeEntity";
DELETE FROM "BadgeEntity";
DELETE FROM "HistoryPasswordEntity";
DELETE FROM "CourseFeedbackEntity";
DELETE FROM "CourseAvaliationEntity";
DELETE FROM "NotificationEntity";
DELETE FROM "ExamQuestionEntity";
DELETE FROM "ExamEntity";
DELETE FROM "QuestionOptionEntity";
DELETE FROM "QuestionEntity";
DELETE FROM "CourseActivitieEntity";
DELETE FROM "CourseEntity";
DELETE FROM "TeamEntity";
DELETE FROM "DepartmentEntity";
DELETE FROM "EducationLevelEntity";
DELETE FROM "AddressEntity";
DELETE FROM "ProfessionalProfileEntity" WHERE "UserId" > 1;
DELETE FROM "PessoalProfileEntity" WHERE "UserId" > 1;
DELETE FROM "UserEntity" WHERE "Id" > 1;

-- Resetar todas as sequências de IDs para começar em 1
SELECT setval('"UserEntity_Id_seq"', 1);
SELECT setval('"PessoalProfileEntity_Id_seq"', 0, true); -- Usa false para permitir 0
SELECT setval('"ProfessionalProfileEntity_Id_seq"', 0, true);
SELECT setval('"AddressEntity_Id_seq"', 0, true);
SELECT setval('"EducationLevelEntity_Id_seq"', 0, true);
SELECT setval('"DepartmentEntity_Id_seq"', 0, true);
SELECT setval('"TeamEntity_Id_seq"', 0, true);
SELECT setval('"CourseEntity_Id_seq"', 0, true);
SELECT setval('"CourseActivitieEntity_Id_seq"', 0, true);
SELECT setval('"QuestionEntity_Id_seq"', 0, true);
SELECT setval('"QuestionOptionEntity_Id_seq"', 0, true);
SELECT setval('"ExamEntity_Id_seq"', 0, true);
SELECT setval('"ExamQuestionEntity_Id_seq"', 0, true);
SELECT setval('"BadgeEntity_Id_seq"', 0, true);
SELECT setval('"CourseFeedbackEntity_Id_seq"', 0, true);
SELECT setval('"CourseAvaliationEntity_Id_seq"', 0, true);
SELECT setval('"HistoryPasswordEntity_Id_seq"', 0, true);
SELECT setval('"NotificationEntity_Id_seq"', 0, true);

-- ===================================================================
-- 1. USERS (Usuários do Sistema)
-- ===================================================================
INSERT INTO "UserEntity" ("Cpf", "Email", "Password", "IsActive", "CreateDate", "ModificationDate") VALUES
-- Professores/Instrutores
('23456789012', 'prof.silva@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('34567890123', 'prof.costa@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('45678901234', 'prof.santos@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('56789012345', 'prof.oliveira@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
-- Alunos/Estudantes
('67890123456', 'maria.silva@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('78901234567', 'joao.santos@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('89012345678', 'ana.costa@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('90123456789', 'pedro.oliveira@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('01234567890', 'julia.pereira@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('11234567891', 'carlos.rodrigues@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('21234567892', 'beatriz.lima@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('31234567893', 'lucas.martins@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('41234567894', 'fernanda.ferreira@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('51234567895', 'rafael.almeida@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
-- Gerentes de Departamento
('61234567896', 'gerente.rh@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW()),
('71234567897', 'gerente.ti@trainmaster.com', '$2a$12$gvduQbf38QnxG3krABcLIOY./ihPjN0tnw4jRgjyy9/CdBdIHR9XK', true, NOW(), NOW());

-- ===================================================================
-- 2. PESOAL PROFILES (Perfis Pessoais)
-- ===================================================================
INSERT INTO "PessoalProfileEntity" ("FullName", "Cpf", "Email", "DateOfBirth", "Gender", "Marital", "UserId", "CreateDate", "ModificationDate") VALUES
-- Professores
('João Silva', '23456789012', 'prof.silva@trainmaster.com', '1980-03-20', 1, 2, 2, NOW(), NOW()),
('Maria Costa', '34567890123', 'prof.costa@trainmaster.com', '1978-07-12', 2, 2, 3, NOW(), NOW()),
('Carlos Santos', '45678901234', 'prof.santos@trainmaster.com', '1982-11-30', 1, 1, 4, NOW(), NOW()),
('Ana Oliveira', '56789012345', 'prof.oliveira@trainmaster.com', '1975-05-08', 2, 2, 5, NOW(), NOW()),
-- Alunos
('Maria Silva', '67890123456', 'maria.silva@trainmaster.com', '1995-04-15', 2, 1, 6, NOW(), NOW()),
('João Santos', '78901234567', 'joao.santos@trainmaster.com', '1998-09-22', 1, 1, 7, NOW(), NOW()),
('Ana Costa', '89012345678', 'ana.costa@trainmaster.com', '1996-02-10', 2, 1, 8, NOW(), NOW()),
('Pedro Oliveira', '90123456789', 'pedro.oliveira@trainmaster.com', '1997-08-05', 1, 1, 9, NOW(), NOW()),
('Julia Pereira', '01234567890', 'julia.pereira@trainmaster.com', '1999-12-18', 2, 1, 10, NOW(), NOW()),
('Carlos Rodrigues', '11234567891', 'carlos.rodrigues@trainmaster.com', '1994-06-25', 1, 2, 11, NOW(), NOW()),
('Beatriz Lima', '21234567892', 'beatriz.lima@trainmaster.com', '1996-03-11', 2, 1, 12, NOW(), NOW()),
('Lucas Martins', '31234567893', 'lucas.martins@trainmaster.com', '1997-10-07', 1, 1, 13, NOW(), NOW()),
('Fernanda Ferreira', '41234567894', 'fernanda.ferreira@trainmaster.com', '1995-01-20', 2, 2, 14, NOW(), NOW()),
('Rafael Almeida', '51234567895', 'rafael.almeida@trainmaster.com', '1998-07-14', 1, 1, 15, NOW(), NOW()),
-- Gerentes
('Roberto Mendes', '61234567896', 'gerente.rh@trainmaster.com', '1985-03-25', 1, 2, 16, NOW(), NOW()),
('Patricia Souza', '71234567897', 'gerente.ti@trainmaster.com', '1988-11-10', 2, 1, 17, NOW(), NOW());

-- ===================================================================
-- 3. PROFESSIONAL PROFILES (Perfis Profissionais)
-- ===================================================================
INSERT INTO "ProfessionalProfileEntity" ("JobTitle", "YearsOfExperience", "Skills", "Certifications", "UserId", "CreateDate", "ModificationDate") VALUES
-- Professores
('Professor de Desenvolvimento Web', 12, 'HTML, CSS, JavaScript, React, Node.js', 'AWS Certified Developer, React Certification', 2, NOW(), NOW()),
('Professora de Data Science', 10, 'Python, Machine Learning, SQL, Tableau', 'Google Data Analytics Certificate', 3, NOW(), NOW()),
('Professor de Segurança Cibernética', 15, 'Ethical Hacking, CISSP, Network Security', 'CISSP, CEH Certified', 4, NOW(), NOW()),
('Professora de DevOps', 8, 'Kubernetes, Docker, CI/CD, GitLab', 'Kubernetes Certified Administrator', 5, NOW(), NOW()),
-- Alunos
('Desenvolvedor Junior', 2, 'JavaScript, React, Node.js', 'FreeCodeCamp Full Stack Web Development', 6, NOW(), NOW()),
('Desenvolvedor Backend', 3, 'Java, Spring Boot, PostgreSQL', 'Oracle Certified Java Developer', 7, NOW(), NOW()),
('Analista de Dados', 1, 'Python, SQL, Excel', null, 8, NOW(), NOW()),
('Desenvolvedor Frontend', 2, 'React, TypeScript, Tailwind CSS', 'Meta Front-End Developer Certificate', 9, NOW(), NOW()),
('Estudante de Computação', 0, 'Java, Python, HTML, CSS', null, 10, NOW(), NOW()),
('Desenvolvedor Full Stack', 4, 'JavaScript, Node.js, React, MongoDB', 'MERN Stack Certificate', 11, NOW(), NOW()),
('Estudante de Engenharia de Software', 0, 'Java, C++, Data Structures', null, 12, NOW(), NOW()),
('Desenvolvedor Mobile', 2, 'Flutter, Dart, Firebase', 'Flutter Development Bootcamp', 13, NOW(), NOW()),
('Product Manager Junior', 3, 'Product Management, Agile, Scrum', 'Certified Product Manager', 14, NOW(), NOW()),
('DevOps Engineer Junior', 1, 'Docker, Kubernetes, Jenkins', 'Docker Certified Associate', 15, NOW(), NOW()),
-- Gerentes
('Gerente de RH', 12, 'Gestão de Pessoas, Recrutamento, Treinamento', 'SHRM Senior Certified Professional', 16, NOW(), NOW()),
('Gerente de TI', 14, 'Gestão de TI, Infraestrutura, Cloud', 'ITIL Foundation, PMP Certified', 17, NOW(), NOW());

-- ===================================================================
-- 4. ADDRESSES (Endereços)
-- ===================================================================
INSERT INTO "AddressEntity" ("PostalCode", "Street", "Neighborhood", "City", "Uf", "PessoalProfileId", "CreateDate", "ModificationDate") VALUES
('20020-040', 'Rua do Ouvidor 50', 'Centro', 'Rio de Janeiro', 'RJ', 1, NOW(), NOW()),
('30130-010', 'Avenida Afonso Pena 3000', 'Centro', 'Belo Horizonte', 'MG', 2, NOW(), NOW()),
('40010-000', 'Avenida Sete de Setembro 100', 'Comércio', 'Salvador', 'BA', 3, NOW(), NOW()),
('01310-200', 'Rua Augusta 800', 'Consolação', 'São Paulo', 'SP', 4, NOW(), NOW()),
('22021-001', 'Av Atlântica 100', 'Copacabana', 'Rio de Janeiro', 'RJ', 5, NOW(), NOW()),
('30140-100', 'Rua da Bahia 1200', 'Centro', 'Belo Horizonte', 'MG', 6, NOW(), NOW()),
('52010-001', 'Avenida Boa Viagem 500', 'Boa Viagem', 'Recife', 'PE', 7, NOW(), NOW()),
('01308-001', 'Avenida Faria Lima 2000', 'Itaim Bibi', 'São Paulo', 'SP', 8, NOW(), NOW()),
('22041-001', 'Av Nehmen Prates 500', 'Copacabana', 'Rio de Janeiro', 'RJ', 9, NOW(), NOW()),
('30110-000', 'Rua Rio de Janeiro 1500', 'Centro', 'Belo Horizonte', 'MG', 10, NOW(), NOW()),
('40015-020', 'Praça Castro Alves 10', 'Centro', 'Salvador', 'BA', 11, NOW(), NOW()),
('51020-001', 'Av Boa Viagem 200', 'Boa Viagem', 'Recife', 'PE', 12, NOW(), NOW()),
('01310-300', 'Rua Joaquim Floriano 400', 'Itaim Bibi', 'São Paulo', 'SP', 13, NOW(), NOW()),
('20070-020', 'Av Marechal Floriano 200', 'Centro', 'Rio de Janeiro', 'RJ', 14, NOW(), NOW()),
('30140-200', 'Rua dos Caetés 600', 'Funcionários', 'Belo Horizonte', 'MG', 15, NOW(), NOW()),
('52010-200', 'Rua do Brum 200', 'Recife Antigo', 'Recife', 'PE', 16, NOW(), NOW());

-- ===================================================================
-- 5. EDUCATION LEVELS (Níveis de Educação)
-- ===================================================================
INSERT INTO "EducationLevelEntity" ("Title", "Institution", "StartedAt", "EndeedAt", "ProfessionalProfileId", "CreateDate", "ModificationDate") VALUES
-- Professores
('Bacharelado em Sistemas de Informação', 'PUC-Rio', '2005-01-01', '2009-12-31', 1, NOW(), NOW()),
('Mestrado em Ciência da Computação', 'Universidade Estadual de Campinas', '2010-01-01', '2012-12-31', 1, NOW(), NOW()),
('Bacharelado em Estatística', 'Universidade Federal do Rio de Janeiro', '2008-01-01', '2012-12-31', 2, NOW(), NOW()),
('Mestrado em Ciência de Dados', 'Instituto de Matemática Pura e Aplicada', '2013-01-01', '2015-12-31', 2, NOW(), NOW()),
('Bacharelado em Engenharia de Software', 'Instituto Militar de Engenharia', '1998-01-01', '2002-12-31', 3, NOW(), NOW()),
('Mestrado em Segurança da Informação', 'Instituto Tecnológico da Aeronáutica', '2003-01-01', '2005-12-31', 3, NOW(), NOW()),
('Bacharelado em Engenharia de Computação', 'Universidade Federal de Minas Gerais', '2005-01-01', '2010-12-31', 4, NOW(), NOW()),
('Mestrado em Engenharia de Software', 'UFMG', '2011-01-01', '2013-12-31', 4, NOW(), NOW()),
-- Alunos
('Bacharelado em Ciência da Computação', 'Universidade Federal de São Paulo', '2018-01-01', '2022-12-31', 5, NOW(), NOW()),
('Bacharelado em Sistemas de Informação', 'Universidade Federal Fluminense', '2017-01-01', '2020-12-31', 6, NOW(), NOW()),
('Bacharelado em Engenharia de Dados', 'PUC-MG', '2020-01-01', '2024-12-31', 7, NOW(), NOW()),
('Técnico em Informática', 'ETEC', '2019-01-01', '2021-12-31', 8, NOW(), NOW()),
('Bacharelado em Ciência da Computação', 'USP', '2020-01-01', '2025-12-31', 9, NOW(), NOW()),
('Bacharelado em Análise e Desenvolvimento de Sistemas', 'UNIFACS', '2016-01-01', '2019-12-31', 10, NOW(), NOW()),
('Bacharelado em Engenharia de Software', 'UFPE', '2022-01-01', '2026-12-31', 11, NOW(), NOW()),
('Técnico em Desenvolvimento Mobile', 'SENAI', '2020-01-01', '2022-12-31', 12, NOW(), NOW()),
('MBA em Gestão de Produtos', 'Fundação Getúlio Vargas', '2018-01-01', '2019-12-31', 13, NOW(), NOW()),
('Bacharelado em Engenharia da Computação', 'UFBA', '2019-01-01', '2024-12-31', 14, NOW(), NOW()),
-- Gerentes
('Bacharelado em Psicologia', 'Universidade de São Paulo', '2000-01-01', '2004-12-31', 15, NOW(), NOW()),
('Especialização em Gestão de RH', 'Fundação Dom Cabral', '2006-01-01', '2007-12-31', 15, NOW(), NOW()),
('Bacharelado em Ciência da Computação', 'PUC-MG', '2004-01-01', '2008-12-31', 16, NOW(), NOW()),
('MBA em Gestão de TI', 'IBMEC', '2010-01-01', '2011-12-31', 16, NOW(), NOW());

-- ===================================================================
-- 6. DEPARTMENTS (Departamentos)
-- ===================================================================
INSERT INTO "DepartmentEntity" ("Name", "Description", "IsActive", "UserId", "CreateDate", "ModificationDate") VALUES
('Recursos Humanos', 'Gestão de pessoas, recrutamento e desenvolvimento profissional', true, 16, NOW(), NOW()),
('Tecnologia da Informação', 'Desenvolvimento, infraestrutura e suporte técnico', true, 17, NOW(), NOW()),
('Vendas', 'Gestão de vendas e relacionamento com clientes', true, 1, NOW(), NOW()),
('Marketing', 'Promoção e comunicação da empresa', true, 1, NOW(), NOW()),
('Financeiro', 'Gestão financeira e contábil', true, 1, NOW(), NOW());

-- ===================================================================
-- 7. TEAMS (Equipes)
-- ===================================================================
INSERT INTO "TeamEntity" ("Name", "Description", "IsActive", "DepartmentId", "CreateDate", "ModificationDate") VALUES
('RH - Recrutamento', 'Equipe responsável por processos seletivos e recrutamento', true, 1, NOW(), NOW()),
('RH - Treinamento', 'Equipe responsável por treinamentos e desenvolvimento', true, 1, NOW(), NOW()),
('TI - Desenvolvimento', 'Equipe de desenvolvedores de software', true, 2, NOW(), NOW()),
('TI - Infraestrutura', 'Equipe de suporte e infraestrutura', true, 2, NOW(), NOW()),
('TI - Qualidade', 'Equipe de QA e testes', true, 2, NOW(), NOW()),
('Vendas - Comercial', 'Equipe de vendas comerciais', true, 3, NOW(), NOW()),
('Marketing - Digital', 'Equipe de marketing digital e redes sociais', true, 4, NOW(), NOW()),
('Financeiro - Contábil', 'Equipe contábil', true, 5, NOW(), NOW());

-- ===================================================================
-- 8. COURSES (Cursos)
-- ===================================================================
INSERT INTO "CourseEntity" ("Name", "Description", "Author", "StartDate", "EndDate", "IsActive", "UserId", "CreateDate", "ModificationDate") VALUES
('Desenvolvimento Web Completo com React', 'Aprenda a criar aplicações web modernas usando React, JavaScript ES6+, Hooks, Context API e Redux. Curso completo desde fundamentos até projetos avançados.', 'João Silva', '2024-01-15', '2024-04-15', true, 2, NOW(), NOW()),
('Data Science e Machine Learning com Python', 'Domine técnicas de análise de dados, machine learning e inteligência artificial usando Python, Pandas, Scikit-learn e TensorFlow.', 'Maria Costa', '2024-02-01', '2024-05-01', true, 3, NOW(), NOW()),
('Segurança Cibernética e Ethical Hacking', 'Aprenda técnicas avançadas de segurança da informação, penetração testing e defesa contra ataques cibernéticos.', 'Carlos Santos', '2024-01-20', '2024-03-20', true, 4, NOW(), NOW()),
('DevOps: Containers, CI/CD e Cloud', 'Compreenda CI/CD, Docker, Kubernetes e deploy em nuvem. Automação completa de pipelines de desenvolvimento.', 'Ana Oliveira', '2024-02-10', '2024-05-10', true, 5, NOW(), NOW()),
('JavaScript Avançado e Node.js', 'Aprofunde-se em JavaScript moderno ES6+, Node.js, Express e desenvolvimento de APIs RESTful e real-time.', 'João Silva', '2024-03-01', '2024-06-01', true, 2, NOW(), NOW()),
('Banco de Dados com PostgreSQL', 'Mastery em PostgreSQL: queries avançadas, otimização, stored procedures, triggers e gerenciamento de dados complexos.', 'Maria Costa', '2024-01-10', '2024-03-10', true, 3, NOW(), NOW()),
('React Native: Desenvolvimento Mobile', 'Crie apps iOS e Android com React Native. Hooks, navegação, integração com APIs e publicação nas stores.', 'João Silva', '2024-02-20', '2024-05-20', true, 2, NOW(), NOW()),
('Cloud Computing com AWS', 'Domine serviços AWS: EC2, S3, Lambda, RDS. Arquiteturas escaláveis e custo-efetivas na nuvem.', 'Ana Oliveira', '2024-03-15', '2024-06-15', true, 5, NOW(), NOW());

-- ===================================================================
-- 9. COURSE ACTIVITIES (Atividades dos Cursos)
-- ===================================================================
INSERT INTO "CourseActivitieEntity" ("Title", "Description", "StartDate", "DueDate", "MaxScore", "CourseId", "CreateDate", "ModificationDate") VALUES
-- Curso 1: React
('Configuração do Ambiente', 'Configure Node.js, NPM e seu primeiro projeto React', '2024-01-15', '2024-01-18', 10, 1, NOW(), NOW()),
('Componentes e Props', 'Entenda componentes React e passagem de propriedades', '2024-01-19', '2024-01-25', 20, 1, NOW(), NOW()),
('State e Hooks', 'Gerenciamento de estado com useState e useEffect', '2024-01-26', '2024-02-05', 25, 1, NOW(), NOW()),
('Projeto Todo App', 'Crie uma aplicação de lista de tarefas completa', '2024-02-06', '2024-02-20', 50, 1, NOW(), NOW()),
('Router e Navegação', 'Implemente navegação com React Router', '2024-02-21', '2024-03-05', 30, 1, NOW(), NOW()),
('Context API e Redux', 'Gerenciamento global de estado', '2024-03-06', '2024-03-20', 40, 1, NOW(), NOW()),
-- Curso 2: Data Science
('Introdução ao Python', 'Fundamentos de Python para Data Science', '2024-02-01', '2024-02-10', 15, 2, NOW(), NOW()),
('Pandas: Manipulação de Dados', 'Análise e transformação de dados com Pandas', '2024-02-11', '2024-02-25', 30, 2, NOW(), NOW()),
('Visualização de Dados', 'Crie gráficos com Matplotlib e Seaborn', '2024-02-26', '2024-03-10', 25, 2, NOW(), NOW()),
('Machine Learning com Scikit-learn', 'Algoritmos de ML: regressão, classificação', '2024-03-11', '2024-03-25', 40, 2, NOW(), NOW()),
('Deep Learning com TensorFlow', 'Redes neurais e deep learning', '2024-03-26', '2024-04-10', 50, 2, NOW(), NOW()),
-- Curso 3: Segurança
('Fundamentos de Segurança', 'Conceitos básicos de cibersegurança', '2024-01-20', '2024-01-30', 20, 3, NOW(), NOW()),
('OWASP Top 10', 'Vulnerabilidades web mais comuns', '2024-01-31', '2024-02-10', 30, 3, NOW(), NOW()),
('Penetration Testing', 'Técnicas de teste de penetração', '2024-02-11', '2024-02-28', 40, 3, NOW(), NOW()),
-- Curso 4: DevOps
('Introdução ao Docker', 'Containers e Docker fundamentals', '2024-02-10', '2024-02-20', 20, 4, NOW(), NOW()),
('Kubernetes Basics', 'Orquestração de containers', '2024-02-21', '2024-03-10', 35, 4, NOW(), NOW()),
('CI/CD com Jenkins e GitLab', 'Automação de pipelines', '2024-03-11', '2024-03-25', 45, 4, NOW(), NOW()),
-- Curso 5: JavaScript Avançado
('JavaScript Moderno ES6+', 'Async/await, Promises, destructuring', '2024-03-01', '2024-03-15', 25, 5, NOW(), NOW()),
('Node.js e Express', 'Criação de APIs RESTful', '2024-03-16', '2024-04-01', 40, 5, NOW(), NOW()),
('WebSockets e Real-time', 'Comunicação em tempo real', '2024-04-02', '2024-04-20', 35, 5, NOW(), NOW());

-- ===================================================================
-- 10. QUESTIONS (Questões das Atividades)
-- ===================================================================
INSERT INTO "QuestionEntity" ("CourseActivitieId", "Statement", "Order", "Points", "CreateDate", "ModificationDate") VALUES
-- Atividade 1: Configuração Ambiente
(1, 'Qual é o comando para instalar dependências do projeto React?', 1, 5.00, NOW(), NOW()),
(1, 'Quais são os três principais arquivos de configuração de um projeto React?', 2, 5.00, NOW(), NOW()),

-- Atividade 2: Componentes e Props
(2, 'O que são Props em React?', 1, 6.67, NOW(), NOW()),
(2, 'Como criar um componente funcional em React?', 2, 6.67, NOW(), NOW()),
(2, 'Qual a diferença entre componentes funcionais e de classe?', 3, 6.66, NOW(), NOW()),

-- Atividade 3: State e Hooks
(3, 'Explique o hook useState', 1, 12.50, NOW(), NOW()),
(3, 'Para que serve o hook useEffect?', 2, 12.50, NOW(), NOW()),

-- Atividade 4: Projeto Todo App
(4, 'Como você estruturaria o estado da aplicação Todo?', 1, 16.67, NOW(), NOW()),
(4, 'Implemente a funcionalidade de adicionar tarefa', 2, 16.67, NOW(), NOW()),
(4, 'Implemente a funcionalidade de remover tarefa', 3, 16.66, NOW(), NOW()),

-- Atividade 5: Router
(5, 'Como implementar rotas em React Router?', 1, 15.00, NOW(), NOW()),
(5, 'Explique o componente Link do React Router', 2, 15.00, NOW(), NOW()),

-- Atividade 7: Python Intro
(7, 'Qual é a sintaxe para criar uma lista em Python?', 1, 5.00, NOW(), NOW()),
(7, 'Como criar um dicionário em Python?', 2, 5.00, NOW(), NOW()),
(7, 'O que são list comprehensions?', 3, 5.00, NOW(), NOW()),

-- Atividade 8: Pandas
(8, 'Como ler um arquivo CSV com Pandas?', 1, 10.00, NOW(), NOW()),
(8, 'Explique o método groupby do Pandas', 2, 10.00, NOW(), NOW()),
(8, 'Como filtrar um DataFrame?', 3, 10.00, NOW(), NOW()),

-- Atividade 13: Fundamentos Segurança
(13, 'O que é SQL Injection?', 1, 6.67, NOW(), NOW()),
(13, 'Explique o conceito de autenticação vs autorização', 2, 6.67, NOW(), NOW()),
(13, 'O que são vulnerabilidades OWASP Top 10?', 3, 6.66, NOW(), NOW()),

-- Atividade 16: Docker
(16, 'O que é um container Docker?', 1, 10.00, NOW(), NOW()),
(16, 'Qual a diferença entre imagem e container?', 2, 10.00, NOW(), NOW());

-- ===================================================================
-- 11. QUESTION OPTIONS (Opções das Questões)
-- ===================================================================
INSERT INTO "QuestionOptionEntity" ("QuestionId", "Text", "IsCorrect", "CreateDate", "ModificationDate") VALUES
-- Pergunta 1: Comando npm install
(1, 'npm install', true, NOW(), NOW()),
(1, 'npm update', false, NOW(), NOW()),
(1, 'npm create', false, NOW(), NOW()),
(1, 'npm run', false, NOW(), NOW()),

-- Pergunta 2: Arquivos configuração React
(2, 'package.json, node_modules, index.js', false, NOW(), NOW()),
(2, 'package.json, src/index.js, public/index.html', true, NOW(), NOW()),
(2, 'App.js, index.js, README.md', false, NOW(), NOW()),
(2, 'App.js, public/index.html, webpack.config.js', false, NOW(), NOW()),

-- Pergunta 3: Props
(3, 'Propriedades locais do componente', false, NOW(), NOW()),
(3, 'Propriedades passadas de um componente pai para filho', true, NOW(), NOW()),
(3, 'Funções do componente', false, NOW(), NOW()),
(3, 'Estados locais do componente', false, NOW(), NOW()),

-- Pergunta 4: Componente funcional
(4, 'function Component() {}', false, NOW(), NOW()),
(4, 'const Component = () => {}', true, NOW(), NOW()),
(4, 'class Component() {}', false, NOW(), NOW()),
(4, 'var Component = function() {}', false, NOW(), NOW()),

-- Pergunta 5: Diferença componentes
(5, 'Componentes funcionais usam classes', false, NOW(), NOW()),
(5, 'Componentes de classe usam this, funcionais usam hooks', true, NOW(), NOW()),
(5, 'Não há diferença', false, NOW(), NOW()),
(5, 'Só existem componentes de classe', false, NOW(), NOW()),

-- Pergunta 6: useState
(6, 'Hook para gerenciar estado do componente', true, NOW(), NOW()),
(6, 'Hook para efeitos colaterais', false, NOW(), NOW()),
(6, 'Hook para contexto', false, NOW(), NOW()),
(6, 'Hook para performance', false, NOW(), NOW()),

-- Pergunta 7: useEffect
(7, 'Gerenciar estado local', false, NOW(), NOW()),
(7, 'Executar efeitos colaterais após renderização', true, NOW(), NOW()),
(7, 'Criar variáveis', false, NOW(), NOW()),
(7, 'Importar dependências', false, NOW(), NOW()),

-- Pergunta 8: Estrutura estado Todo
(8, 'Um array de objetos com text e completed', true, NOW(), NOW()),
(8, 'Uma string', false, NOW(), NOW()),
(8, 'Um número', false, NOW(), NOW()),
(8, 'Um boolean', false, NOW(), NOW()),

-- Pergunta 9: Adicionar tarefa
(9, 'Usar push no array', false, NOW(), NOW()),
(9, 'Usar spread operator e adicionar novo item', true, NOW(), NOW()),
(9, 'Usar splice', false, NOW(), NOW()),
(9, 'Usar map', false, NOW(), NOW()),

-- Pergunta 10: Remover tarefa
(10, 'Usar splice diretamente', false, NOW(), NOW()),
(10, 'Usar filter para criar novo array', true, NOW(), NOW()),
(10, 'Usar pop', false, NOW(), NOW()),
(10, 'Usar shift', false, NOW(), NOW()),

-- Pergunta 11: Rotas React
(11, 'Usar React Router e componentes Route', true, NOW(), NOW()),
(11, 'Usar window.location', false, NOW(), NOW()),
(11, 'Usar redirects nativos', false, NOW(), NOW()),
(11, 'Usar links HTML normais', false, NOW(), NOW()),

-- Pergunta 12: Link
(12, 'Componente para navegação sem reload', true, NOW(), NOW()),
(12, 'Tag HTML padrão', false, NOW(), NOW()),
(12, 'Função JavaScript', false, NOW(), NOW()),
(12, 'Proprietário do React', false, NOW(), NOW()),

-- Pergunta 13: Lista Python
(13, '[1, 2, 3]', true, NOW(), NOW()),
(13, '{1, 2, 3}', false, NOW(), NOW()),
(13, '(1, 2, 3)', false, NOW(), NOW()),
(13, '1, 2, 3', false, NOW(), NOW()),

-- Pergunta 14: Dicionário Python
(14, '{key: value}', true, NOW(), NOW()),
(14, '[key: value]', false, NOW(), NOW()),
(14, '(key: value)', false, NOW(), NOW()),
(14, '<key: value>', false, NOW(), NOW()),

-- Pergunta 15: List comprehensions
(15, 'Forma concisa de criar listas', true, NOW(), NOW()),
(15, 'Método de uma lista', false, NOW(), NOW()),
(15, 'Tipo de loop', false, NOW(), NOW()),
(15, 'Operador matemático', false, NOW(), NOW()),

-- Pergunta 16: Ler CSV Pandas
(16, 'pd.read_csv("file.csv")', true, NOW(), NOW()),
(16, 'pd.read_excel("file.csv")', false, NOW(), NOW()),
(16, 'pd.load("file.csv")', false, NOW(), NOW()),
(16, 'pd.import("file.csv")', false, NOW(), NOW()),

-- Pergunta 17: groupby
(17, 'Agrupar dados por uma coluna e aplicar funções', true, NOW(), NOW()),
(17, 'Ordenar dados', false, NOW(), NOW()),
(17, 'Filtrar dados', false, NOW(), NOW()),
(17, 'Mesclar DataFrames', false, NOW(), NOW()),

-- Pergunta 18: Filtrar DataFrame
(18, 'df[df["col"] > value]', true, NOW(), NOW()),
(18, 'df.filter("col")', false, NOW(), NOW()),
(18, 'df.select("col")', false, NOW(), NOW()),
(18, 'df.where("col")', false, NOW(), NOW()),

-- Pergunta 19: SQL Injection
(19, 'Ataque que injeta código SQL malicioso', true, NOW(), NOW()),
(19, 'Tipo de banco de dados', false, NOW(), NOW()),
(19, 'Método de segurança', false, NOW(), NOW()),
(19, 'Protocolo de rede', false, NOW(), NOW()),

-- Pergunta 20: Autenticação vs Autorização
(20, 'Autenticação: quem você é. Autorização: o que você pode fazer', true, NOW(), NOW()),
(20, 'São a mesma coisa', false, NOW(), NOW()),
(20, 'Autenticação: senha. Autorização: login', false, NOW(), NOW()),
(20, 'Autenticação: permissões. Autorização: credenciais', false, NOW(), NOW()),

-- Pergunta 21: OWASP Top 10
(21, 'Top 10 vulnerabilidades web mais comuns', true, NOW(), NOW()),
(21, 'Top 10 linguagens de programação', false, NOW(), NOW()),
(21, 'Top 10 frameworks', false, NOW(), NOW()),
(21, 'Top 10 bancos de dados', false, NOW(), NOW()),

-- Pergunta 22: Container Docker
(22, 'Instância executável de uma imagem', true, NOW(), NOW()),
(22, 'Imagem Docker', false, NOW(), NOW()),
(22, 'Servidor físico', false, NOW(), NOW()),
(22, 'Banco de dados', false, NOW(), NOW()),

-- Pergunta 23: Imagem vs Container
(23, 'Imagem: blueprint. Container: instância em execução', true, NOW(), NOW()),
(23, 'São a mesma coisa', false, NOW(), NOW()),
(23, 'Container: blueprint. Imagem: instância', false, NOW(), NOW()),
(23, 'Não há diferença', false, NOW(), NOW());

-- ===================================================================
-- 12. EXAMS (Exames)
-- ===================================================================
INSERT INTO "ExamEntity" ("CourseId", "Title", "Instructions", "StartAt", "EndAt", "IsPublished", "CreateDate", "ModificationDate") VALUES
(1, 'Exame Final - Desenvolvimento Web React', 'Complete o exame em até 2 horas. Use componentes React e hooks. Boa sorte!', '2024-04-10 14:00:00', '2024-04-10 16:00:00', true, NOW(), NOW()),
(2, 'Exame Final - Data Science', 'Resolva os problemas de análise de dados e machine learning.', '2024-04-25 14:00:00', '2024-04-25 17:00:00', true, NOW(), NOW()),
(3, 'Exame Final - Segurança Cibernética', 'Identifique vulnerabilidades e implemente soluções de segurança.', '2024-03-15 14:00:00', '2024-03-15 16:00:00', true, NOW(), NOW()),
(4, 'Exame Final - DevOps', 'Configure pipelines CI/CD e orquestre containers.', '2024-05-05 14:00:00', '2024-05-05 17:00:00', true, NOW(), NOW()),
(5, 'Exame Final - JavaScript Avançado', 'Crie APIs RESTful e implemente comunicação real-time.', '2024-05-25 14:00:00', '2024-05-25 17:00:00', true, NOW(), NOW());

-- ===================================================================
-- 13. EXAM QUESTIONS (Questões dos Exames)
-- ===================================================================
INSERT INTO "ExamQuestionEntity" ("ExamId", "QuestionId", "Order", "Points") VALUES
-- Exame 1: React (usa questões das atividades 1-5)
(1, 1, 1, 10),
(1, 2, 2, 10),
(1, 3, 3, 13.33),
(1, 4, 4, 13.34),
(1, 5, 5, 13.33),

-- Exame 2: Data Science (usa questões das atividades 7-8)
(2, 13, 1, 16.67),
(2, 14, 2, 16.67),
(2, 15, 3, 16.66),
(2, 16, 4, 16.67),
(2, 17, 5, 16.67),

-- Exame 3: Segurança (usa questão 13)
(3, 19, 1, 33.33),
(3, 20, 2, 33.33),
(3, 21, 3, 33.34),

-- Exame 4: DevOps (usa questões Docker)
(4, 22, 1, 50),
(4, 23, 2, 50);

-- ===================================================================
-- 14. BADGES (Insígnias)
-- ===================================================================
INSERT INTO "BadgeEntity" ("Name", "Description", "CreateDate", "ModificationDate") VALUES
('Desenvolvedor Iniciante', 'Completou seu primeiro curso de programação', NOW(), NOW()),
('Mestre React', 'Domina React e seus conceitos avançados', NOW(), NOW()),
('Cientista de Dados', 'Completó o curso de Data Science com sucesso', NOW(), NOW()),
('Guardião da Segurança', 'Certificado em Segurança Cibernética', NOW(), NOW()),
('DevOps Expert', 'Mestre em CI/CD e orquestração', NOW(), NOW()),
('100% Aproveitamento', 'Conseguiu 100% nas atividades', NOW(), NOW()),
('Top Student', 'Entre os melhores 10% do curso', NOW(), NOW()),
('Assiduidade', 'Não faltou nenhuma atividade', NOW(), NOW()),
('Entrega Pontual', 'Todas as atividades entregues no prazo', NOW(), NOW()),
('Mentor', 'Ajudou outros alunos no fórum', NOW(), NOW());

-- ===================================================================
-- 15. USER BADGES (Insígnias dos Usuários)
-- ===================================================================
INSERT INTO "UserBadgeEntity" ("UserId", "BadgeId", "EarnedAt") VALUES
(6, 1, '2024-02-20 10:00:00'),
(6, 8, '2024-03-10 12:00:00'),
(7, 1, '2024-02-15 14:30:00'),
(7, 9, '2024-02-28 16:00:00'),
(8, 3, '2024-03-15 11:00:00'),
(9, 1, '2024-03-05 09:30:00'),
(9, 2, '2024-04-15 13:00:00'),
(10, 8, '2024-03-20 15:00:00'),
(11, 6, '2024-03-30 10:30:00'),
(12, 4, '2024-02-25 14:00:00'),
(13, 7, '2024-04-05 11:30:00'),
(14, 5, '2024-03-15 16:00:00'),
(15, 2, '2024-04-01 09:00:00');

-- ===================================================================
-- 16. COURSE FEEDBACK (Feedback dos Cursos)
-- ===================================================================
INSERT INTO "CourseFeedbackEntity" ("CourseId", "StudentId", "Rating", "Comment", "CreateDate", "ModificationDate") VALUES
(1, 6, 5, 'Excelente curso! Aprendi muito sobre React.', NOW(), NOW()),
(1, 9, 5, 'Professores muito didáticos. Material completo.', NOW(), NOW()),
(1, 15, 4, 'Bom curso, mas poderia ter mais exemplos práticos.', NOW(), NOW()),
(2, 7, 5, 'Curso de Data Science muito completo!', NOW(), NOW()),
(2, 11, 4, 'Bom conteúdo, especialmente a parte de ML.', NOW(), NOW()),
(3, 7, 5, 'Aprendi muito sobre segurança.', NOW(), NOW()),
(3, 12, 5, 'Excelente! Superou minhas expectativas.', NOW(), NOW()),
(4, 11, 4, 'Conteúdo avançado de DevOps.', NOW(), NOW()),
(4, 15, 5, 'Melhor curso de DevOps que já fiz.', NOW(), NOW()),
(5, 6, 4, 'Bom curso de JavaScript avançado.', NOW(), NOW()),
(5, 9, 5, 'Node.js explicado de forma clara.', NOW(), NOW());

-- ===================================================================
-- 17. COURSE AVALIATIONS (Avaliações dos Cursos)
-- ===================================================================
INSERT INTO "CourseAvaliationEntity" ("Rating", "Comment", "ReviewDate", "CourseId", "CreateDate", "ModificationDate") VALUES
(5, 'Melhor curso de React que já vi!', '2024-04-12 10:00:00', 1, NOW(), NOW()),
(5, 'Excelente material de Data Science.', '2024-04-26 14:00:00', 2, NOW(), NOW()),
(5, 'Curso completo de segurança cibernética.', '2024-03-16 11:00:00', 3, NOW(), NOW()),
(4, 'DevOps muito bem explicado.', '2024-05-06 15:00:00', 4, NOW(), NOW()),
(5, 'JavaScript avançado com ótimos exemplos.', '2024-05-26 13:00:00', 5, NOW(), NOW()),
(4, 'Curso bom, recomendo.', '2024-03-12 16:00:00', 1, NOW(), NOW());

-- ===================================================================
-- 18. HISTORY PASSWORDS (Histórico de Senhas)
-- ===================================================================
INSERT INTO "HistoryPasswordEntity" ("OldPassword", "UserId", "CreateDate", "ModificationDate") VALUES
('OldPass123!', 2, '2023-12-01 10:00:00', NOW()),
('OldPass456!', 6, '2023-12-15 14:00:00', NOW()),
('OldPass789!', 7, '2023-12-20 16:00:00', NOW()),
('OldPass321!', 8, '2024-01-10 11:00:00', NOW()),
('OldPass654!', 9, '2024-01-15 09:00:00', NOW());

-- ===================================================================
-- 19. NOTIFICATIONS (Notificações)
-- ===================================================================
INSERT INTO "NotificationEntity" ("Description", "CourseId", "CreateDate", "ModificationDate") VALUES
('Novo vídeo publicado: Introdução ao React', 1, NOW(), NOW()),
('Deadline estendido para Atividade 3', 1, NOW(), NOW()),
('Novo material disponível sobre Machine Learning', 2, NOW(), NOW()),
('Lembrete: Exame final daqui a 3 dias', 2, NOW(), NOW()),
('Novo módulo de segurança disponível', 3, NOW(), NOW()),
('Certificado disponível para download', 3, NOW(), NOW()),
('Nova atividade: Docker Compose', 4, NOW(), NOW()),
('Mentoria ao vivo marcada para amanhã', 5, NOW(), NOW());

-- ===================================================================
-- SCRIPT CONCLUÍDO
-- ===================================================================
-- IMPORTANTE: Este script assume que já existe um usuário com ID 1
-- Os dados inseridos começam a partir do ID 2
-- ===================================================================
--
-- CREDENCIAIS DE LOGIN:
-- -------------------
-- Email: prof.silva@trainmaster.com
-- Senha: 123456
--
-- OBSERVAÇÃO: Todas as senhas são "123456"
-- Se o hash bcrypt não funcionar, você precisará gerar um novo hash
-- Para gerar um novo hash bcrypt para "123456", use:
--
-- Em C# (.NET):
--   var hash = BCrypt.Net.BCrypt.HashPassword("123456");
--
-- Ou use um gerador online de bcrypt
-- ===================================================================
-- Total de registros inseridos:
-- - 16 Novos Usuários (IDs 2-17)
-- - 16 Perfis Pessoais
-- - 16 Perfis Profissionais
-- - 16 Endereços
-- - 22 Níveis de Educação
-- - 5 Departamentos
-- - 8 Equipes
-- - 8 Cursos (criados pelos professores)
-- - 18 Atividades de Curso
-- - 23 Questões
-- - 69 Opções de Questões
-- - 5 Exames
-- - 14 Questões de Exame
-- - 10 Badges
-- - 13 User Badges
-- - 11 Feedbacks
-- - 6 Avaliações
-- - 5 Histórico de Senhas
-- - 8 Notificações
-- ===================================================================
