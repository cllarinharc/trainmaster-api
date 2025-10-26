using Microsoft.AspNetCore.Mvc;
using Serilog;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Api.Controllers
{
    [ApiController]
    [Route("api/exams")]
    [Produces("application/json")]
    public class ExamController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public ExamController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Add([FromBody] ExamAddDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var entity = new ExamEntity
            {
                Title = dto.Title?.Trim() ?? string.Empty,
                Instructions = dto.Instructions,
                StartAt = dto.StartAt.UtcDateTime,
                EndAt = dto.EndAt.UtcDateTime,
                IsPublished = dto.IsPublished,
                CourseId = dto.CourseId,
                CreateDate = DateTime.UtcNow,
                ModificationDate = DateTime.UtcNow
            };

            var result = await _uow.ExamService.Add(entity);
            return result.Success
                ? Ok(result)
                : BadRequest(new { message = result.Message ?? "Erro ao adicionar prova." });
        }


        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var list = await _uow.ExamService.GetAll();
                return Ok(list);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, "Erro ao listar provas");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var entity = await _uow.ExamService.GetById(id);

                if (entity is null)
                    return NotFound(new { message = "Prova não encontrada." });

                return Ok(entity);
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, "Erro ao buscar prova {ExamId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}