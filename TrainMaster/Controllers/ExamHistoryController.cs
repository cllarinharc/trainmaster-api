using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/exam-histories")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ExamHistoryController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public ExamHistoryController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ExamHistoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] ExamHistoryDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _uow.ExamHistoryService.AddWithStatistics(dto);

            if (result.Success)
                return Ok(result.Data);

            return BadRequest(new { message = result.Message ?? "Erro ao registrar histórico de exame." });
        }

        [HttpGet("user/{userId:long}")]
        [ProducesResponseType(typeof(List<ExamHistoryEntity>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(long userId)
        {
            var histories = await _uow.ExamHistoryService.GetByUserId(userId);

            if (histories == null || !histories.Any())
                return NotFound($"No exam histories found for user ID {userId}.");

            return Ok(histories);
        }
    }
}