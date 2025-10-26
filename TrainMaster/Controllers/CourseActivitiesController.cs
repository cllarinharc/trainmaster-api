using Microsoft.AspNetCore.Mvc;
using Serilog;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Api.Controllers
{
    [ApiController]
    [Route("api/course-activities")]
    [Produces("application/json")]
    public class CourseActivitiesController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public CourseActivitiesController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpGet("{courseId:int}/questions")]
        public async Task<IActionResult> GetQuestionsByCourseId([FromRoute] int courseId)
        {
            if (courseId <= 0)
                return BadRequest(new { message = "CourseId inválido." });

            var activities = await _uow.CourseActivitieService.GetByCourseId(courseId);
            if (activities is null || !activities.Any())
                return NotFound(new { message = "Nenhuma atividade encontrada para este curso." });

            var allQuestions = new List<QuestionEntity>();
            foreach (var activity in activities)
            {
                var qs = await _uow.QuestionService.GetByActivityId(activity.Id);
                if (qs is { Count: > 0 })
                    allQuestions.AddRange(qs);
            }

            if (!allQuestions.Any())
                return NotFound(new { message = "Nenhuma questão encontrada para as atividades deste curso." });

            var result = allQuestions
                .OrderBy(q => q.CourseActivitieId)
                .ThenBy(q => q.Order)
                .Select(q => new QuestionDto(
                    q.Id,
                    (int)q.CourseActivitieId,
                    q.Statement,
                    q.Order,
                    q.Points,
                    (q.Options ?? new List<QuestionOptionEntity>())
                        .Select(o => new QuestionOptionDto(o.Id, o.Text, o.IsCorrect))
                        .ToList()
                ))
                .ToList();

            return Ok(result);
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Add([FromBody] CourseActivitieAddDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState); 

            var entity = new CourseActivitieEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                DueDate = DateTime.SpecifyKind(dto.DueDate, DateTimeKind.Utc),
                MaxScore = dto.MaxScore,    
                CourseId = dto.CourseId,
                ModificationDate = DateTime.UtcNow
            };

            var result = await _uow.CourseActivitieService.Add(entity);
            return result.Success
                ? Ok(result)
                : BadRequest(new { message = result.Message ?? "Erro ao adicionar atividade." });
        }

        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _uow.CourseActivitieService.GetAll();
                return Ok(list);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, "Erro ao listar atividades");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var entity = await _uow.CourseActivitieService.GetById(id);

                if (entity is null)
                    return NotFound(new { message = "Atividade não encontrada." });

                return Ok(entity);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, "Erro ao buscar atividade {ActivityId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CourseActivitieEntity entity)
        {
            if (entity is null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            entity.Id = id;

            var result = await _uow.CourseActivitieService.Update(entity);

            if (result.Success)
                return Ok(result);

            if (!string.IsNullOrWhiteSpace(result.Message) && result.Message.Contains("não encontrada", StringComparison.OrdinalIgnoreCase))
                return NotFound(new { message = result.Message });

            return BadRequest(new { message = result.Message ?? "Erro ao atualizar atividade." });
        }
    }
}