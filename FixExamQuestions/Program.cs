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

Console.WriteLine("🔍 Verificando questões de exames sem opções...\n");

// Buscar todas as questões que estão em exames
var examQuestions = await context.ExamQuestionEntity
    .Include(eq => eq.Question)
        .ThenInclude(q => q.Options)
    .Include(eq => eq.Exam)
    .ToListAsync();

Console.WriteLine($"📝 Total de questões em exames: {examQuestions.Count}\n");

// Agrupar por QuestionId para evitar duplicatas
var uniqueQuestionIds = examQuestions
    .Select(eq => eq.QuestionId)
    .Distinct()
    .ToList();

var questionsWithoutOptions = new List<QuestionEntity>();

foreach (var questionId in uniqueQuestionIds)
{
    var question = await context.QuestionEntity
        .Include(q => q.Options)
        .FirstOrDefaultAsync(q => q.Id == questionId);

    if (question != null && (!question.Options.Any() || question.Options.Count == 0))
    {
        questionsWithoutOptions.Add(question);
    }
}

Console.WriteLine($"❌ Questões sem opções encontradas: {questionsWithoutOptions.Count}\n");

if (questionsWithoutOptions.Count == 0)
{
    Console.WriteLine("✅ Todas as questões de exames já têm opções!");
    return;
}

// Gerar opções para cada questão
var random = new Random();
var optionTemplates = new[]
{
    new[] {
        "Esta é a resposta correta baseada nos conceitos estudados.",
        "Esta alternativa está parcialmente correta mas não é a resposta completa.",
        "Esta opção contém informações relacionadas mas não está correta.",
        "Esta alternativa está incorreta e não reflete o conteúdo estudado."
    },
    new[] {
        "Correto: Esta resposta demonstra compreensão adequada do tema.",
        "Incorreto: Embora relacionada, esta opção não captura completamente o conceito.",
        "Incorreto: Esta alternativa está parcialmente correta mas falta informação importante.",
        "Incorreto: Esta opção está incorreta e não reflete o conteúdo estudado."
    },
    new[] {
        "Verdadeiro: Esta é a melhor resposta para esta questão.",
        "Falso: Esta abordagem pode funcionar mas não é a mais adequada.",
        "Falso: Esta alternativa é válida mas não segue os padrões estudados.",
        "Falso: Esta opção está incorreta e pode causar problemas."
    },
    new[] {
        "Sim, esta é a resposta correta para esta questão.",
        "Não, embora seja uma opção válida, não é a mais adequada.",
        "Não, esta alternativa está incorreta.",
        "Não, esta opção não está relacionada com a questão."
    },
    new[] {
        "Correto: Esta resposta está de acordo com os conceitos fundamentais.",
        "Incorreto: Esta opção apresenta um conceito relacionado mas não é a resposta adequada.",
        "Incorreto: Embora pareça correta, esta alternativa contém informações incorretas.",
        "Incorreto: Esta opção está completamente incorreta."
    }
};

int totalOptionsCreated = 0;

foreach (var question in questionsWithoutOptions)
{
    Console.WriteLine($"\n📋 Processando questão ID {question.Id}:");
    Console.WriteLine($"   Enunciado: {question.Statement.Substring(0, Math.Min(80, question.Statement.Length))}...");

    // Escolher um template aleatório
    var template = optionTemplates[random.Next(optionTemplates.Length)];

    // Criar 4 opções (1 correta, 3 incorretas)
    for (int i = 0; i < 4; i++)
    {
        var option = new QuestionOptionEntity
        {
            QuestionId = question.Id,
            Text = template[i],
            IsCorrect = i == 0, // Primeira opção é sempre correta
            CreateDate = DateTime.UtcNow,
            ModificationDate = DateTime.UtcNow
        };

        context.QuestionOptionEntity.Add(option);
        totalOptionsCreated++;

        Console.WriteLine($"   ✅ Opção {i + 1} criada: {(i == 0 ? "CORRETA" : "incorreta")}");
    }

    await context.SaveChangesAsync();
}

Console.WriteLine($"\n\n✅ Processo concluído!");
Console.WriteLine($"📊 Total de questões processadas: {questionsWithoutOptions.Count}");
Console.WriteLine($"📊 Total de opções criadas: {totalOptionsCreated}");
Console.WriteLine($"📊 Média de opções por questão: {totalOptionsCreated / (double)questionsWithoutOptions.Count:F1}");

// Verificação final
Console.WriteLine("\n🔍 Verificando resultado final...\n");

var finalExamQuestions = await context.ExamQuestionEntity
    .Select(eq => eq.QuestionId)
    .Distinct()
    .ToListAsync();

var finalQuestionsWithOptions = 0;
var finalQuestionsWithoutOptions = 0;

foreach (var questionId in finalExamQuestions)
{
    var question = await context.QuestionEntity
        .Include(q => q.Options)
        .FirstOrDefaultAsync(q => q.Id == questionId);

    if (question != null)
    {
        var optionCount = question.Options?.Count ?? 0;
        if (optionCount > 0)
        {
            finalQuestionsWithOptions++;
        }
        else
        {
            finalQuestionsWithoutOptions++;
        }
    }
}

Console.WriteLine($"📊 Verificação final:");
Console.WriteLine($"   ✅ Questões com opções: {finalQuestionsWithOptions}");
Console.WriteLine($"   ❌ Questões sem opções: {finalQuestionsWithoutOptions}");

if (finalQuestionsWithoutOptions == 0)
{
    Console.WriteLine("\n✅ Todas as questões de exames agora têm opções!");
}
else
{
    Console.WriteLine($"\n⚠️  Ainda há {finalQuestionsWithoutOptions} questão(ões) sem opções.");
}

