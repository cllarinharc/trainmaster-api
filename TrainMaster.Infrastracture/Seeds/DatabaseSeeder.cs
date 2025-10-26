using Microsoft.EntityFrameworkCore;
using TrainMaster.Domain.Entity;
using TrainMaster.Domain.Enums;
using TrainMaster.Infrastracture.Connections;

namespace TrainMaster.Infrastracture.Seeds
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(DataContext context)
        {
            // Verificar se já existem dados
            if (await context.UserEntity.AnyAsync())
            {
                return; // Banco já populado
            }

            // 1. Criar usuários
            var users = await CreateUsers(context);

            // 2. Criar departamentos
            var departments = await CreateDepartments(context, users);

            // 3. Criar badges
            var badges = await CreateBadges(context);

            // 4. Criar cursos
            var courses = await CreateCourses(context, users);

            // 5. Criar atividades de curso
            var activities = await CreateCourseActivities(context, courses);

            // 6. Criar questões
            var questions = await CreateQuestions(context, activities);

            // 7. Criar opções de questões
            await CreateQuestionOptions(context, questions);

            // 8. Criar exames
            var exams = await CreateExams(context, courses);

            // 9. Criar questões de exame
            await CreateExamQuestions(context, exams, questions);

            // 10. Criar feedbacks de curso
            await CreateCourseFeedbacks(context, courses, users);

            // 11. Criar avaliações de curso
            await CreateCourseAvaliations(context, courses);

            // 12. Criar notificações
            await CreateNotifications(context, courses);

            // 13. Criar user badges
            await CreateUserBadges(context, users, badges);

            // 14. Criar histórico de senhas
            await CreateHistoryPasswords(context, users);

            // 15. Criar times
            await CreateTeams(context, departments);

            await context.SaveChangesAsync();
        }

        private static async Task<List<UserEntity>> CreateUsers(DataContext context)
        {
            var users = new List<UserEntity>
            {
                new UserEntity
                {
                    Cpf = "123.456.789-00",
                    Email = "admin@trainmaster.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    IsActive = true,
                    PessoalProfile = new PessoalProfileEntity
                    {
                        FullName = "Administrador do Sistema",
                        Cpf = "123.456.789-00",
                        Email = "admin@trainmaster.com",
                        DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                        Gender = GenderStatus.Male,
                        Marital = MaritalStatus.Married,
                        Address = new AddressEntity
                        {
                            PostalCode = "01234-567",
                            Street = "Rua das Flores, 123",
                            Neighborhood = "Centro",
                            City = "São Paulo",
                            Uf = "SP"
                        }
                    },
                    ProfessionalProfile = new ProfessionalProfileEntity
                    {
                        JobTitle = "Administrador de Sistema",
                        YearsOfExperience = 10,
                        Skills = "C#, .NET, Entity Framework, PostgreSQL, Docker",
                        Certifications = "Microsoft Certified Professional, AWS Solutions Architect",
                        EducationLevel = new EducationLevelEntity
                        {
                            Title = "Bacharelado em Ciência da Computação",
                            Institution = "Universidade de São Paulo",
                            StartedAt = new DateTime(2003, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                            EndeedAt = new DateTime(2007, 12, 31, 0, 0, 0, DateTimeKind.Utc)
                        }
                    }
                },
                new UserEntity
                {
                    Cpf = "987.654.321-00",
                    Email = "instrutor@trainmaster.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("instrutor123"),
                    IsActive = true,
                    PessoalProfile = new PessoalProfileEntity
                    {
                        FullName = "Maria Silva Santos",
                        Cpf = "987.654.321-00",
                        Email = "instrutor@trainmaster.com",
                        DateOfBirth = new DateTime(1990, 8, 22, 0, 0, 0, DateTimeKind.Utc),
                        Gender = GenderStatus.Woman,
                        Marital = MaritalStatus.Single,
                        Address = new AddressEntity
                        {
                            PostalCode = "04567-890",
                            Street = "Avenida Paulista, 1000",
                            Neighborhood = "Bela Vista",
                            City = "São Paulo",
                            Uf = "SP"
                        }
                    },
                    ProfessionalProfile = new ProfessionalProfileEntity
                    {
                        JobTitle = "Instrutora Sênior",
                        YearsOfExperience = 8,
                        Skills = "Treinamento, Desenvolvimento de Conteúdo, E-learning",
                        Certifications = "Certified Professional in Learning and Performance",
                        EducationLevel = new EducationLevelEntity
                        {
                            Title = "Mestrado em Educação",
                            Institution = "Pontifícia Universidade Católica de São Paulo",
                            StartedAt = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                            EndeedAt = new DateTime(2017, 12, 31, 0, 0, 0, DateTimeKind.Utc)
                        }
                    }
                },
                new UserEntity
                {
                    Cpf = "456.789.123-00",
                    Email = "aluno1@trainmaster.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("aluno123"),
                    IsActive = true,
                    PessoalProfile = new PessoalProfileEntity
                    {
                        FullName = "João Pedro Oliveira",
                        Cpf = "456.789.123-00",
                        Email = "aluno1@trainmaster.com",
                        DateOfBirth = new DateTime(1995, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                        Gender = GenderStatus.Male,
                        Marital = MaritalStatus.Single,
                        Address = new AddressEntity
                        {
                            PostalCode = "12345-678",
                            Street = "Rua da Consolação, 500",
                            Neighborhood = "Consolação",
                            City = "São Paulo",
                            Uf = "SP"
                        }
                    },
                    ProfessionalProfile = new ProfessionalProfileEntity
                    {
                        JobTitle = "Desenvolvedor Júnior",
                        YearsOfExperience = 2,
                        Skills = "JavaScript, React, Node.js",
                        Certifications = "AWS Cloud Practitioner",
                        EducationLevel = new EducationLevelEntity
                        {
                            Title = "Tecnólogo em Análise e Desenvolvimento de Sistemas",
                            Institution = "FATEC",
                            StartedAt = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                            EndeedAt = new DateTime(2020, 12, 31, 0, 0, 0, DateTimeKind.Utc)
                        }
                    }
                },
                new UserEntity
                {
                    Cpf = "789.123.456-00",
                    Email = "aluno2@trainmaster.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("aluno123"),
                    IsActive = true,
                    PessoalProfile = new PessoalProfileEntity
                    {
                        FullName = "Ana Carolina Costa",
                        Cpf = "789.123.456-00",
                        Email = "aluno2@trainmaster.com",
                        DateOfBirth = new DateTime(1992, 11, 5, 0, 0, 0, DateTimeKind.Utc),
                        Gender = GenderStatus.Woman,
                        Marital = MaritalStatus.Married,
                        Address = new AddressEntity
                        {
                            PostalCode = "23456-789",
                            Street = "Rua Augusta, 2000",
                            Neighborhood = "Consolação",
                            City = "São Paulo",
                            Uf = "SP"
                        }
                    },
                    ProfessionalProfile = new ProfessionalProfileEntity
                    {
                        JobTitle = "Analista de Sistemas",
                        YearsOfExperience = 5,
                        Skills = "Python, Django, SQL, Análise de Dados",
                        Certifications = "Google Data Analytics Certificate",
                        EducationLevel = new EducationLevelEntity
                        {
                            Title = "Bacharelado em Sistemas de Informação",
                            Institution = "Universidade Mackenzie",
                            StartedAt = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                            EndeedAt = new DateTime(2014, 12, 31, 0, 0, 0, DateTimeKind.Utc)
                        }
                    }
                }
            };

            context.UserEntity.AddRange(users);
            await context.SaveChangesAsync();
            return users;
        }

        private static async Task<List<DepartmentEntity>> CreateDepartments(DataContext context, List<UserEntity> users)
        {
            var departments = new List<DepartmentEntity>
            {
                new DepartmentEntity
                {
                    Name = "Tecnologia da Informação",
                    Description = "Departamento responsável por desenvolvimento e manutenção de sistemas",
                    IsActive = true,
                    UserId = users[0].Id
                },
                new DepartmentEntity
                {
                    Name = "Recursos Humanos",
                    Description = "Departamento responsável por gestão de pessoas e treinamentos",
                    IsActive = true,
                    UserId = users[1].Id
                },
                new DepartmentEntity
                {
                    Name = "Vendas",
                    Description = "Departamento responsável por vendas e relacionamento com clientes",
                    IsActive = true,
                    UserId = users[0].Id
                }
            };

            context.DepartmentEntity.AddRange(departments);
            await context.SaveChangesAsync();
            return departments;
        }

        private static async Task<List<BadgeEntity>> CreateBadges(DataContext context)
        {
            var badges = new List<BadgeEntity>
            {
                new BadgeEntity
                {
                    Name = "Primeiro Curso",
                    Description = "Concluiu seu primeiro curso na plataforma"
                },
                new BadgeEntity
                {
                    Name = "Estudante Dedicado",
                    Description = "Concluiu 5 cursos com sucesso"
                },
                new BadgeEntity
                {
                    Name = "Especialista",
                    Description = "Concluiu 10 cursos na mesma área"
                },
                new BadgeEntity
                {
                    Name = "Mestre",
                    Description = "Concluiu 20 cursos na plataforma"
                },
                new BadgeEntity
                {
                    Name = "Colaborador Ativo",
                    Description = "Participou de 5 discussões nos fóruns"
                },
                new BadgeEntity
                {
                    Name = "Instrutor",
                    Description = "Criou e ministrou seu primeiro curso"
                }
            };

            context.BadgeEntity.AddRange(badges);
            await context.SaveChangesAsync();
            return badges;
        }

        private static async Task<List<CourseEntity>> CreateCourses(DataContext context, List<UserEntity> users)
        {
            var courses = new List<CourseEntity>
            {
                new CourseEntity
                {
                    Name = "Introdução ao C# e .NET",
                    Description = "Curso completo de introdução à linguagem C# e framework .NET",
                    Author = "Maria Silva Santos",
                    StartDate = DateTime.UtcNow.AddDays(-30),
                    EndDate = DateTime.UtcNow.AddDays(30),
                    IsActive = true,
                    UserId = users[1].Id
                },
                new CourseEntity
                {
                    Name = "Desenvolvimento Web com React",
                    Description = "Aprenda a criar aplicações web modernas com React",
                    Author = "Maria Silva Santos",
                    StartDate = DateTime.UtcNow.AddDays(-15),
                    EndDate = DateTime.UtcNow.AddDays(45),
                    IsActive = true,
                    UserId = users[1].Id
                },
                new CourseEntity
                {
                    Name = "Fundamentos de Banco de Dados",
                    Description = "Conceitos essenciais de banco de dados relacionais",
                    Author = "Administrador do Sistema",
                    StartDate = DateTime.UtcNow.AddDays(-10),
                    EndDate = DateTime.UtcNow.AddDays(20),
                    IsActive = true,
                    UserId = users[0].Id
                },
                new CourseEntity
                {
                    Name = "Análise de Dados com Python",
                    Description = "Aprenda análise de dados usando Python e suas bibliotecas",
                    Author = "Maria Silva Santos",
                    StartDate = DateTime.UtcNow.AddDays(-5),
                    EndDate = DateTime.UtcNow.AddDays(25),
                    IsActive = true,
                    UserId = users[1].Id
                }
            };

            context.CourseEntity.AddRange(courses);
            await context.SaveChangesAsync();
            return courses;
        }

        private static async Task<List<CourseActivitieEntity>> CreateCourseActivities(DataContext context, List<CourseEntity> courses)
        {
            var activities = new List<CourseActivitieEntity>
            {
                // Atividades para o curso de C#
                new CourseActivitieEntity
                {
                    Title = "Exercício: Variáveis e Tipos de Dados",
                    Description = "Pratique o uso de variáveis e tipos de dados em C#",
                    StartDate = DateTime.UtcNow.AddDays(-25),
                    DueDate = DateTime.UtcNow.AddDays(5),
                    MaxScore = 100,
                    CourseId = courses[0].Id
                },
                new CourseActivitieEntity
                {
                    Title = "Projeto: Calculadora Simples",
                    Description = "Desenvolva uma calculadora simples em C#",
                    StartDate = DateTime.UtcNow.AddDays(-20),
                    DueDate = DateTime.UtcNow.AddDays(10),
                    MaxScore = 150,
                    CourseId = courses[0].Id
                },
                // Atividades para o curso de React
                new CourseActivitieEntity
                {
                    Title = "Exercício: Componentes React",
                    Description = "Crie seus primeiros componentes React",
                    StartDate = DateTime.UtcNow.AddDays(-10),
                    DueDate = DateTime.UtcNow.AddDays(20),
                    MaxScore = 100,
                    CourseId = courses[1].Id
                },
                new CourseActivitieEntity
                {
                    Title = "Projeto: Todo List",
                    Description = "Desenvolva uma aplicação de lista de tarefas",
                    StartDate = DateTime.UtcNow.AddDays(-5),
                    DueDate = DateTime.UtcNow.AddDays(25),
                    MaxScore = 200,
                    CourseId = courses[1].Id
                },
                // Atividades para o curso de Banco de Dados
                new CourseActivitieEntity
                {
                    Title = "Exercício: Consultas SQL",
                    Description = "Pratique consultas SQL básicas",
                    StartDate = DateTime.UtcNow.AddDays(-5),
                    DueDate = DateTime.UtcNow.AddDays(15),
                    MaxScore = 100,
                    CourseId = courses[2].Id
                }
            };

            context.CourseActivitieEntity.AddRange(activities);
            await context.SaveChangesAsync();
            return activities;
        }

        private static async Task<List<QuestionEntity>> CreateQuestions(DataContext context, List<CourseActivitieEntity> activities)
        {
            var questions = new List<QuestionEntity>
            {
                // Questões para a atividade de Variáveis C#
                new QuestionEntity
                {
                    Statement = "Qual é a palavra-chave usada para declarar uma variável constante em C#?",
                    Order = 1,
                    Points = 10,
                    CourseActivitieId = activities[0].Id
                },
                new QuestionEntity
                {
                    Statement = "Qual é o tipo de dados usado para representar números decimais em C#?",
                    Order = 2,
                    Points = 10,
                    CourseActivitieId = activities[0].Id
                },
                // Questões para a atividade de Componentes React
                new QuestionEntity
                {
                    Statement = "Qual hook é usado para gerenciar estado em componentes funcionais do React?",
                    Order = 1,
                    Points = 15,
                    CourseActivitieId = activities[2].Id
                },
                new QuestionEntity
                {
                    Statement = "Qual é a principal diferença entre props e state no React?",
                    Order = 2,
                    Points = 15,
                    CourseActivitieId = activities[2].Id
                },
                // Questões para a atividade de SQL
                new QuestionEntity
                {
                    Statement = "Qual comando SQL é usado para recuperar dados de uma tabela?",
                    Order = 1,
                    Points = 10,
                    CourseActivitieId = activities[4].Id
                }
            };

            context.QuestionEntity.AddRange(questions);
            await context.SaveChangesAsync();
            return questions;
        }

        private static async Task CreateQuestionOptions(DataContext context, List<QuestionEntity> questions)
        {
            var options = new List<QuestionOptionEntity>
            {
                // Opções para questão 1 (const em C#)
                new QuestionOptionEntity { QuestionId = questions[0].Id, Text = "const", IsCorrect = true },
                new QuestionOptionEntity { QuestionId = questions[0].Id, Text = "var", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[0].Id, Text = "let", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[0].Id, Text = "final", IsCorrect = false },

                // Opções para questão 2 (decimal em C#)
                new QuestionOptionEntity { QuestionId = questions[1].Id, Text = "decimal", IsCorrect = true },
                new QuestionOptionEntity { QuestionId = questions[1].Id, Text = "float", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[1].Id, Text = "double", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[1].Id, Text = "int", IsCorrect = false },

                // Opções para questão 3 (useState no React)
                new QuestionOptionEntity { QuestionId = questions[2].Id, Text = "useState", IsCorrect = true },
                new QuestionOptionEntity { QuestionId = questions[2].Id, Text = "useEffect", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[2].Id, Text = "useContext", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[2].Id, Text = "useReducer", IsCorrect = false },

                // Opções para questão 4 (props vs state)
                new QuestionOptionEntity { QuestionId = questions[3].Id, Text = "Props são imutáveis, state é mutável", IsCorrect = true },
                new QuestionOptionEntity { QuestionId = questions[3].Id, Text = "Props são mutáveis, state é imutável", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[3].Id, Text = "Não há diferença entre props e state", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[3].Id, Text = "Props são locais, state é global", IsCorrect = false },

                // Opções para questão 5 (SELECT em SQL)
                new QuestionOptionEntity { QuestionId = questions[4].Id, Text = "SELECT", IsCorrect = true },
                new QuestionOptionEntity { QuestionId = questions[4].Id, Text = "GET", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[4].Id, Text = "FETCH", IsCorrect = false },
                new QuestionOptionEntity { QuestionId = questions[4].Id, Text = "RETRIEVE", IsCorrect = false }
            };

            context.QuestionOptionEntity.AddRange(options);
            await context.SaveChangesAsync();
        }

        private static async Task<List<ExamEntity>> CreateExams(DataContext context, List<CourseEntity> courses)
        {
            var exams = new List<ExamEntity>
            {
                new ExamEntity
                {
                    Title = "Avaliação Final - C# e .NET",
                    Instructions = "Responda todas as questões com atenção. Tempo limite: 60 minutos.",
                    StartAt = DateTime.UtcNow.AddDays(5),
                    EndAt = DateTime.UtcNow.AddDays(5).AddHours(1),
                    IsPublished = true,
                    CourseId = courses[0].Id
                },
                new ExamEntity
                {
                    Title = "Avaliação Final - React",
                    Instructions = "Demonstre seus conhecimentos em React. Tempo limite: 90 minutos.",
                    StartAt = DateTime.UtcNow.AddDays(20),
                    EndAt = DateTime.UtcNow.AddDays(20).AddHours(1.5),
                    IsPublished = true,
                    CourseId = courses[1].Id
                },
                new ExamEntity
                {
                    Title = "Avaliação Final - Banco de Dados",
                    Instructions = "Teste seus conhecimentos em SQL. Tempo limite: 45 minutos.",
                    StartAt = DateTime.UtcNow.AddDays(15),
                    EndAt = DateTime.UtcNow.AddDays(15).AddMinutes(45),
                    IsPublished = true,
                    CourseId = courses[2].Id
                }
            };

            context.ExamEntity.AddRange(exams);
            await context.SaveChangesAsync();
            return exams;
        }

        private static async Task CreateExamQuestions(DataContext context, List<ExamEntity> exams, List<QuestionEntity> questions)
        {
            var examQuestions = new List<ExamQuestionEntity>
            {
                // Questões para o exame de C#
                new ExamQuestionEntity
                {
                    ExamId = exams[0].Id,
                    QuestionId = questions[0].Id,
                    Order = 1,
                    Points = 10
                },
                new ExamQuestionEntity
                {
                    ExamId = exams[0].Id,
                    QuestionId = questions[1].Id,
                    Order = 2,
                    Points = 10
                },
                // Questões para o exame de React
                new ExamQuestionEntity
                {
                    ExamId = exams[1].Id,
                    QuestionId = questions[2].Id,
                    Order = 1,
                    Points = 15
                },
                new ExamQuestionEntity
                {
                    ExamId = exams[1].Id,
                    QuestionId = questions[3].Id,
                    Order = 2,
                    Points = 15
                },
                // Questões para o exame de Banco de Dados
                new ExamQuestionEntity
                {
                    ExamId = exams[2].Id,
                    QuestionId = questions[4].Id,
                    Order = 1,
                    Points = 10
                }
            };

            context.ExamQuestionEntity.AddRange(examQuestions);
            await context.SaveChangesAsync();
        }

        private static async Task CreateCourseFeedbacks(DataContext context, List<CourseEntity> courses, List<UserEntity> users)
        {
            var feedbacks = new List<CourseFeedbackEntity>
            {
                new CourseFeedbackEntity
                {
                    CourseId = courses[0].Id,
                    StudentId = users[2].Id,
                    Rating = 5,
                    Comment = "Excelente curso! Conteúdo muito bem explicado e prático."
                },
                new CourseFeedbackEntity
                {
                    CourseId = courses[0].Id,
                    StudentId = users[3].Id,
                    Rating = 4,
                    Comment = "Muito bom, mas poderia ter mais exercícios práticos."
                },
                new CourseFeedbackEntity
                {
                    CourseId = courses[1].Id,
                    StudentId = users[2].Id,
                    Rating = 5,
                    Comment = "Curso fantástico! Aprendi muito sobre React."
                },
                new CourseFeedbackEntity
                {
                    CourseId = courses[2].Id,
                    StudentId = users[3].Id,
                    Rating = 4,
                    Comment = "Conteúdo sólido sobre fundamentos de banco de dados."
                }
            };

            context.CourseFeedbackEntity.AddRange(feedbacks);
            await context.SaveChangesAsync();
        }

        private static async Task CreateCourseAvaliations(DataContext context, List<CourseEntity> courses)
        {
            var avalations = new List<CourseAvaliationEntity>
            {
                new CourseAvaliationEntity
                {
                    Rating = 5,
                    Comment = "Curso muito bem estruturado e didático.",
                    ReviewDate = DateTime.UtcNow.AddDays(-5),
                    CourseId = courses[0].Id
                },
                new CourseAvaliationEntity
                {
                    Rating = 4,
                    Comment = "Bom conteúdo, mas poderia ter mais exemplos práticos.",
                    ReviewDate = DateTime.UtcNow.AddDays(-3),
                    CourseId = courses[1].Id
                },
                new CourseAvaliationEntity
                {
                    Rating = 5,
                    Comment = "Excelente curso para iniciantes em banco de dados.",
                    ReviewDate = DateTime.UtcNow.AddDays(-2),
                    CourseId = courses[2].Id
                }
            };

            context.CourseAvaliationEntity.AddRange(avalations);
            await context.SaveChangesAsync();
        }

        private static async Task CreateNotifications(DataContext context, List<CourseEntity> courses)
        {
            var notifications = new List<NotificationEntity>
            {
                new NotificationEntity
                {
                    Description = "Nova atividade disponível: Exercício de Variáveis e Tipos de Dados",
                    CourseId = courses[0].Id
                },
                new NotificationEntity
                {
                    Description = "Lembrete: Projeto Calculadora Simples vence em 3 dias",
                    CourseId = courses[0].Id
                },
                new NotificationEntity
                {
                    Description = "Nova atividade disponível: Componentes React",
                    CourseId = courses[1].Id
                },
                new NotificationEntity
                {
                    Description = "Avaliação Final de Banco de Dados será realizada amanhã",
                    CourseId = courses[2].Id
                }
            };

            context.NotificationEntity.AddRange(notifications);
            await context.SaveChangesAsync();
        }

        private static async Task CreateUserBadges(DataContext context, List<UserEntity> users, List<BadgeEntity> badges)
        {
            var userBadges = new List<UserBadgeEntity>
            {
                new UserBadgeEntity
                {
                    UserId = users[2].Id,
                    BadgeId = badges[0].Id,
                    EarnedAt = DateTime.UtcNow.AddDays(-10)
                },
                new UserBadgeEntity
                {
                    UserId = users[3].Id,
                    BadgeId = badges[0].Id,
                    EarnedAt = DateTime.UtcNow.AddDays(-8)
                },
                new UserBadgeEntity
                {
                    UserId = users[1].Id,
                    BadgeId = badges[5].Id,
                    EarnedAt = DateTime.UtcNow.AddDays(-20)
                }
            };

            context.UserBadgeEntity.AddRange(userBadges);
            await context.SaveChangesAsync();
        }

        private static async Task CreateHistoryPasswords(DataContext context, List<UserEntity> users)
        {
            var historyPasswords = new List<HistoryPasswordEntity>
            {
                new HistoryPasswordEntity
                {
                    OldPassword = BCrypt.Net.BCrypt.HashPassword("oldpassword123"),
                    UserId = users[0].Id
                },
                new HistoryPasswordEntity
                {
                    OldPassword = BCrypt.Net.BCrypt.HashPassword("previouspass456"),
                    UserId = users[1].Id
                }
            };

            context.HistoryPasswordEntity.AddRange(historyPasswords);
            await context.SaveChangesAsync();
        }

        private static async Task CreateTeams(DataContext context, List<DepartmentEntity> departments)
        {
            var teams = new List<TeamEntity>
            {
                new TeamEntity
                {
                    Name = "Desenvolvimento Frontend",
                    Description = "Equipe responsável pelo desenvolvimento da interface do usuário",
                    IsActive = true,
                    DepartmentId = departments[0].Id
                },
                new TeamEntity
                {
                    Name = "Desenvolvimento Backend",
                    Description = "Equipe responsável pelo desenvolvimento da API e lógica de negócio",
                    IsActive = true,
                    DepartmentId = departments[0].Id
                },
                new TeamEntity
                {
                    Name = "Treinamento e Desenvolvimento",
                    Description = "Equipe responsável por criar e ministrar treinamentos",
                    IsActive = true,
                    DepartmentId = departments[1].Id
                },
                new TeamEntity
                {
                    Name = "Vendas Internas",
                    Description = "Equipe de vendas para clientes corporativos",
                    IsActive = true,
                    DepartmentId = departments[2].Id
                }
            };

            context.TeamEntity.AddRange(teams);
            await context.SaveChangesAsync();
        }
    }
}
