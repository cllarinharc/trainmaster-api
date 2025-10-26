using Microsoft.AspNetCore.Mvc;
using Serilog;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Dto;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Api.Controllers
{
    [ApiController]
    [Route("api/courses")]
    [Produces("application/json")]
    public class CoursesController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public CoursesController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CourseDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Payload não pode ser nulo." });

            var result = await _uow.CourseService.Add(dto);

            if (result.Success)
                return Ok(result);

            return BadRequest(new { message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _uow.CourseService.Get();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _uow.CourseService.GetById(id);

            if (!result.Success || result.Data is null)
                return NotFound(new { message = result.Message ?? "Curso não encontrado." });

            return Ok(result.Data);
        }

        [HttpGet("GetByUserId")]
        public async Task<IActionResult> GetByUserId([FromQuery] int userId)
        {
            var list = await _uow.CourseService.GetByUserId(userId);
            return Ok(list);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { message = "Parâmetro 'name' é obrigatório." });

            var items = await _uow.CourseService.GetByName(name.Trim());
            return Ok(items);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CourseDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Payload não pode ser nulo." });
            var entity = MapDtoToEntity(dto, id);
            var result = await _uow.CourseService.Update(entity);

            if (result.Success)
                return Ok(result);

            return BadRequest(new { message = result.Message ?? "Erro ao atualizar curso." });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _uow.CourseService.Delete(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, "Erro ao excluir curso {CourseId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        private static CourseEntity MapDtoToEntity(CourseDto dto, int id)
        {
            return new CourseEntity
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UserId = dto.UserId,
                IsActive = true
            };
        }
    }
}