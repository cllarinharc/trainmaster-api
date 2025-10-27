using Microsoft.AspNetCore.Mvc;
using TrainMaster.Application.UnitOfWork;
using TrainMaster.Domain.Entity;

namespace TrainMaster.Controllers
{
    [ApiController]
    [Route("api/badges")]
    public class BadgeController : ControllerBase
    {
        private readonly IUnitOfWorkService _uow;

        public BadgeController(IUnitOfWorkService uow)
        {
            _uow = uow;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BadgeEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _uow.BadgeService.GetById(id);

            if (!result.Success || result.Data is null)
                return NotFound(new { message = result.Message ?? "Badge não encontrado." });

            return Ok(result.Data);
        }
    }
}