using Microsoft.AspNetCore.Mvc;
using Serilog;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;

namespace TrainMaster.Api.Controllers
{
    [ApiController]
    [Route("api/questions")]
    [Produces("application/json")]
    public class QuestionsController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public QuestionsController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuestionAddDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            if (string.IsNullOrWhiteSpace(dto.Statement))
                return BadRequest(new { message = "O enunciado (Statement) é obrigatório." });

            if (dto.Options is null || dto.Options.Count < 2)
                return BadRequest(new { message = "Informe ao menos 2 alternativas." });

            var correctCount = dto.Options.Count(o => o.IsCorrect);
            if (correctCount != 1)
                return BadRequest(new { message = "A questão deve ter exatamente 1 alternativa correta." });

            try
            {
                var result = await _uow.QuestionService.Add(dto);

                if (result.Success)
                    return Ok(result);

                return BadRequest(new { message = result.Message ?? "Erro ao criar questão." });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro ao criar questão");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro interno ao criar questão." });
            }
        }

        [HttpPost("exams/{examId:int}/questions")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateForExam([FromRoute] int examId, [FromBody] QuestionAddDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            if (examId <= 0)
                return BadRequest(new { message = "ExamId inválido." });

            if (string.IsNullOrWhiteSpace(dto.Statement))
                return BadRequest(new { message = "O enunciado (Statement) é obrigatório." });

            if (dto.Options is null || dto.Options.Count < 2)
                return BadRequest(new { message = "Informe ao menos 2 alternativas." });

            var correctCount = dto.Options.Count(o => o.IsCorrect);
            if (correctCount != 1)
                return BadRequest(new { message = "A questão deve ter exatamente 1 alternativa correta." });

            if (dto.Points <= 0)
                return BadRequest(new { message = "Points deve ser maior que 0." });

            try
            {
                var result = await _uow.QuestionService.Add(dto);

                if (result.Success)
                    return Ok(result);

                return BadRequest(new { message = result.Message ?? "Erro ao criar questão e vinculá-la à prova." });
            }
            catch (InvalidOperationException ex)
            {
                Log.Warning(ex, "Falha ao criar questão para a prova {ExamId}", examId);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Erro interno ao criar questão para a prova {ExamId}", examId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Erro interno ao criar questão para a prova." });
            }
        }
    }
}