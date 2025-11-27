using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TrainMaster.Domain.Entity;
using TrainMaster.Infrastracture.Connections;

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), ".."))
    .AddJsonFile("TrainMaster/appsettings.json", optional: false)
    .AddJsonFile("TrainMaster/appsettings.Development.json", optional: true);

var configuration = configBuilder.Build();

using var context = new DataContext(configuration);

Console.WriteLine("🔍 Conectando ao banco de dados...");

try
{
    if (!await context.Database.CanConnectAsync())
    {
        Console.WriteLine("❌ Não foi possível conectar ao banco de dados.");
        return;
    }

    Console.WriteLine("✅ Conectado com sucesso!\n");
    Console.WriteLine("🔍 Verificando cursos existentes...");

    var courses = await context.CourseEntity
        .Where(c => c.IsActive)
        .ToListAsync();

    if (!courses.Any())
    {
        Console.WriteLine("❌ Nenhum curso ativo encontrado no banco de dados.");
        return;
    }

    Console.WriteLine($"✅ Encontrados {courses.Count} curso(s) ativo(s).\n");

    var activityTemplates = new[]
    {
        new { Title = "Introdução e Conceitos Fundamentais", Description = "Esta atividade introduz os conceitos fundamentais do curso. Você aprenderá os princípios básicos e a teoria essencial para avançar nos próximos módulos." },
        new { Title = "Prática e Aplicação Básica", Description = "Nesta atividade prática, você aplicará os conhecimentos básicos aprendidos. Será uma oportunidade de colocar em prática os conceitos teóricos." },
        new { Title = "Desenvolvimento Intermediário", Description = "Atividade intermediária que aprofunda os conhecimentos. Você trabalhará com conceitos mais complexos e técnicas avançadas." },
        new { Title = "Avançado e Otimização", Description = "Atividade avançada focada em otimização e melhores práticas. Você explorará técnicas profissionais e padrões de mercado." },
        new { Title = "Projeto Final e Consolidação", Description = "Projeto final que consolida todo o aprendizado do curso. Esta atividade integra todos os conceitos estudados ao longo do módulo." }
    };

    var questionTemplates = new[]
    {
        "Qual é o conceito principal abordado nesta atividade?",
        "Qual das seguintes opções representa melhor a aplicação prática deste conteúdo?",
        "Em relação às melhores práticas, qual alternativa está correta?",
        "Qual é a principal vantagem da técnica estudada nesta atividade?",
        "Qual das opções abaixo representa um erro comum a ser evitado?"
    };

    var optionTexts = new[]
    {
        new[] { "Opção correta: Esta é a resposta adequada baseada nos conceitos estudados.", "Opção incorreta: Esta alternativa contém informações parciais mas não está completamente correta.", "Opção incorreta: Esta opção apresenta um conceito relacionado mas não é a resposta adequada.", "Opção incorreta: Esta alternativa está incorreta e pode levar a confusão se selecionada." },
        new[] { "Opção correta: Esta resposta demonstra compreensão adequada dos conceitos fundamentais.", "Opção incorreta: Embora relacionada, esta opção não captura completamente o conceito.", "Opção incorreta: Esta alternativa está parcialmente correta mas falta informação importante.", "Opção incorreta: Esta opção está incorreta e não reflete o conteúdo estudado." },
        new[] { "Opção correta: Esta é a melhor prática recomendada para este cenário.", "Opção incorreta: Esta abordagem pode funcionar mas não é a mais eficiente.", "Opção incorreta: Esta alternativa é válida mas não segue os padrões estudados.", "Opção incorreta: Esta opção está incorreta e pode causar problemas." },
        new[] { "Opção correta: Esta vantagem é uma das principais razões para usar esta técnica.", "Opção incorreta: Embora seja uma vantagem, não é a principal.", "Opção incorreta: Esta é uma desvantagem, não uma vantagem.", "Opção incorreta: Esta opção não está relacionada com as vantagens estudadas." },
        new[] { "Opção correta: Este é um erro comum que deve ser evitado nesta situação.", "Opção incorreta: Esta não é uma prática comum, mas também não é um erro.", "Opção incorreta: Esta é uma prática correta, não um erro.", "Opção incorreta: Embora possa parecer um erro, esta é na verdade uma prática válida." }
    };

    int totalActivities = 0;
    int totalQuestions = 0;
    int totalOptions = 0;

    foreach (var course in courses)
    {
        Console.WriteLine($"📚 Processando curso: {course.Name} (ID: {course.Id})");

        // Verificar se já existem atividades para este curso
        var existingActivities = await context.CourseActivitieEntity
            .Where(ca => ca.CourseId == course.Id)
            .CountAsync();

        if (existingActivities >= 5)
        {
            Console.WriteLine($"   ⏭️  Curso já possui {existingActivities} atividades. Pulando...\n");
            continue;
        }

        var courseDuration = course.EndDate - course.StartDate;

        for (int i = 0; i < 5; i++)
        {
            var activityStart = course.StartDate.AddDays(courseDuration.TotalDays * i / 5.0);
            var activityDue = activityStart.AddDays(7);

            var activity = new CourseActivitieEntity
            {
                Title = $"{activityTemplates[i].Title} - {course.Name}",
                Description = activityTemplates[i].Description,
                StartDate = DateTime.SpecifyKind(activityStart, DateTimeKind.Utc),
                DueDate = DateTime.SpecifyKind(activityDue, DateTimeKind.Utc),
                MaxScore = 100,
                CourseId = course.Id,
                CreateDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            context.CourseActivitieEntity.Add(activity);
            await context.SaveChangesAsync();

            totalActivities++;
            Console.WriteLine($"   ✅ Atividade criada: {activity.Title}");

            // Criar 5 questões para cada atividade
            for (int q = 0; q < 5; q++)
            {
                var question = new QuestionEntity
                {
                    CourseActivitieId = activity.Id,
                    Statement = questionTemplates[q],
                    Order = q + 1,
                    Points = 20.0m,
                    CreateDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow
                };

                context.QuestionEntity.Add(question);
                await context.SaveChangesAsync();

                totalQuestions++;

                // Criar 4 opções para cada questão
                for (int o = 0; o < 4; o++)
                {
                    var option = new QuestionOptionEntity
                    {
                        QuestionId = question.Id,
                        Text = optionTexts[q][o],
                        IsCorrect = o == 0, // Primeira opção é sempre a correta
                        CreateDate = DateTime.UtcNow,
                        ModificationDate = DateTime.UtcNow
                    };

                    context.QuestionOptionEntity.Add(option);
                    totalOptions++;
                }

                await context.SaveChangesAsync();
            }
        }

        Console.WriteLine($"   📊 Total: 5 atividades, 25 questões, 100 opções criadas\n");
    }

    Console.WriteLine("=".PadRight(60, '='));
    Console.WriteLine($"✅ População concluída!");
    Console.WriteLine($"   📚 Cursos processados: {courses.Count}");
    Console.WriteLine($"   📝 Atividades criadas: {totalActivities}");
    Console.WriteLine($"   ❓ Questões criadas: {totalQuestions}");
    Console.WriteLine($"   🔘 Opções criadas: {totalOptions}");
    Console.WriteLine("=".PadRight(60, '='));
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Erro: {ex.Message}");
    Console.WriteLine($"Stack: {ex.StackTrace}");
}


