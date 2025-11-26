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

Console.WriteLine("🔍 Verificando todos os cursos (ativos e inativos)...\n");

var allCourses = await context.CourseEntity
    .OrderBy(c => c.Id)
    .ToListAsync();

Console.WriteLine($"📚 Total de cursos: {allCourses.Count}\n");

foreach (var course in allCourses.Take(10))
{
    var activities = await context.CourseActivitieEntity
        .Where(ca => ca.CourseId == course.Id)
        .CountAsync();

    Console.WriteLine($"ID {course.Id}: {course.Name} (Ativo: {course.IsActive}, Atividades: {activities})");
}

Console.WriteLine("\n🔍 Verificando curso ID 1 especificamente...\n");

var course1 = await context.CourseEntity
    .FirstOrDefaultAsync(c => c.Id == 1);

if (course1 == null)
{
    Console.WriteLine("❌ Curso com ID 1 não existe!");
    Console.WriteLine("\n💡 Cursos disponíveis com atividades:\n");

    var coursesWithActivities = await context.CourseEntity
        .Where(c => c.IsActive)
        .OrderBy(c => c.Id)
        .ToListAsync();

    foreach (var course in coursesWithActivities)
    {
        var activities = await context.CourseActivitieEntity
            .Where(ca => ca.CourseId == course.Id)
            .CountAsync();

        var questions = await context.QuestionEntity
            .Where(q => context.CourseActivitieEntity
                .Where(ca => ca.CourseId == course.Id)
                .Select(ca => ca.Id)
                .Contains(q.CourseActivitieId ?? 0))
            .CountAsync();

        Console.WriteLine($"  ID {course.Id}: {course.Name}");
        Console.WriteLine($"    - Atividades: {activities}");
        Console.WriteLine($"    - Questões: {questions}");
        Console.WriteLine();
    }

    Console.WriteLine("💡 Teste o endpoint com um dos IDs acima!");
    Console.WriteLine("   Exemplo: GET /api/course-activities/33/questions");
}
else
{
    Console.WriteLine($"✅ Curso encontrado: {course1.Name} (Ativo: {course1.IsActive})");

    var activities = await context.CourseActivitieEntity
        .Where(ca => ca.CourseId == 1)
        .ToListAsync();

    Console.WriteLine($"📝 Atividades: {activities.Count}\n");

    if (activities.Count == 0)
    {
        Console.WriteLine("⚠️  O curso ID 1 não tem atividades. Criando atividades...\n");

        // Criar atividades aqui se necessário
    }
}
